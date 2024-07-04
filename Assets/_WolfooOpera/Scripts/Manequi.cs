using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Manequi : BackItem
    {
        [SerializeField] Transform hangZone;
        [SerializeField] Clothing clothingPb;
        [SerializeField] Transform hatArea;

        private Clothing curClothing;
        private CharacterData myData;
        private int totalClothing;
        private int curClothingIdx;
        private Tween _tween;
        private Hat curHat;

        protected override void InitData()
        {
            base.InitData();
            if (hangZone.childCount > 0) curClothing = hangZone.GetChild(0).GetComponent<Clothing>();
            myData = DataSceneManager.Instance.MainCharacterData.CharacterData;
            totalClothing = myData.foldSkinSprite.Length;
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            //   if (item.clothing != null) { CheckWearing(item.clothing); }
            if (item.clothing != null) item.clothing.SetFolding();
            if(item.hat != null) { CheckWearing(item.hat); }
        }
        void CheckWearing(Clothing clothing)
        {
            if (Vector2.Distance(clothing.transform.position, transform.position) < 2)
            {
                if (curClothing != null) return;
                curClothing = clothing;
                clothing.OnHanging(hangZone.position, hangZone);
            }
            else
            {
                if (curClothing != null && curClothing == clothing)
                {
                    curClothing = null;
                }
                clothing.SetFolding();
            }
        }
        void CheckWearing(Hat clothing)
        {
            if (Vector2.Distance(clothing.transform.position, hatArea.position) < 1.5f)
            {
                if (curHat != null) { curHat.MoveToGround(); }
                curHat = clothing;
                clothing.Hanging(hatArea);
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);

            if (item.clothing != null)
            {
                if (item.clothing != curClothing) return;

                _tween?.Kill();
                _tween = DOVirtual.DelayedCall(0.2f, () =>
                {
                    SpawnClothing();
                });
            }

            if (item.hat != null && curHat != null)
            {
                if (item.hat == curHat) curHat = null;
            }
        }

        void SpawnClothing()
        {
            if (curClothingIdx >= myData.frontSkinSprite.Length) curClothingIdx = 0;

            var clothing = Instantiate(clothingPb, hangZone);
            curClothing = clothing;
            clothing.AssignItem(curClothingIdx,
                 myData.frontSkinSprite[curClothingIdx],
                myData.behindSkinSprite[curClothingIdx],
                 myData.foldSkinSprite[curClothingIdx],
                Clothing.State.TestWear);
            clothing.OnGeneration();


            curClothingIdx++;
            if (curClothingIdx >= totalClothing) curClothingIdx = 0;
        }
    }
}