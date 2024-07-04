using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Clothesline : BackItem
    {
        [SerializeField] Transform foldingZone;
        [SerializeField] Transform hangItemZone;
        [SerializeField] Transform[] hangZones;
        [SerializeField] Clothing clothingPb;
        [SerializeField] Clothing[] clothingAssigneds;

        private CharacterData myData;
        private float distannce;
        private int curIdx;
        private int curClothingIdx;
        private List<Clothing> curItems = new List<Clothing>();
        private Tween tweenDelay;

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void InitData()
        {
            base.InitData();
            myData = DataSceneManager.Instance.ItemDataSO.CharacterData;

            for (int i = 0; i < foldingZone.childCount; i++)
            {
                var clothing = Instantiate(clothingPb, foldingZone.GetChild(i));
                clothing.AssignItem(curClothingIdx,
                    myData.frontSkinSprite[curClothingIdx],
                    myData.behindSkinSprite[curClothingIdx],
                    myData.foldSkinSprite[curClothingIdx]);
                clothing.OnGeneration();

                curItems.Add(clothing);
                curClothingIdx++;
                if (curClothingIdx >= myData.foldSkinSprite.Length) curClothingIdx = 0;
            }

            foreach (var item in clothingAssigneds)
            {
                item.AssignItem(item.IdAssign,
                    myData.frontSkinSprite[item.IdAssign],
                    myData.behindSkinSprite[item.IdAssign],
                    myData.foldSkinSprite[item.IdAssign],
                    true);
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.clothing != null)
            {
                if (item.clothing.IsHanger) return;

                if (tweenDelay != null) tweenDelay?.Kill();
                tweenDelay = DOVirtual.DelayedCall(0.25f, () =>
                {
                    for (int i = 0; i < foldingZone.childCount; i++)
                    {
                        if (foldingZone.GetChild(i).childCount == 0)
                        {
                            var clothing = Instantiate(clothingPb, foldingZone.GetChild(i));
                            clothing.AssignItem(curClothingIdx,
                                myData.frontSkinSprite[curClothingIdx],
                                myData.behindSkinSprite[curClothingIdx],
                                myData.foldSkinSprite[curClothingIdx]);
                            clothing.OnGeneration();

                            curClothingIdx++;
                            if (curClothingIdx >= myData.foldSkinSprite.Length) curClothingIdx = 0;
                        }
                    }
                });
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.clothing != null)
            {
                var isHanging = false;
                foreach (var hangZone in hangZones)
                {
                    if (item.clothing.transform.position.y > hangZone.GetChild(0).position.y - 2 &&
                        item.clothing.transform.position.x < hangZone.GetChild(1).position.x &&
                        item.clothing.transform.position.x > hangZone.GetChild(0).position.x)
                    {
                        item.clothing.OnHanging(
                            new Vector3(item.clothing.transform.position.x, hangZone.GetChild(0).position.y, 0),
                            hangItemZone);
                        isHanging = true;
                    }
                }

                if (!isHanging)
                {
                    if (item.clothing.IsHanger)
                    {
                        item.clothing.SetFolding();
                    }
                    else
                    {
                        //if(curItems.Contains(item.clothing))
                        //{
                        //    curItems.Remove(item.clothing);

                        //    var clothing = Instantiate(clothingPb, foldingZone.GetChild(curClothingIdx));
                        //    clothing.AssignItem(curClothingIdx,
                        //        myData.frontSkinSprite[curClothingIdx],
                        //        myData.behindSkinSprite[curClothingIdx],
                        //        myData.foldSkinSprite[curClothingIdx]);
                        //    clothing.OnGeneration();

                        //    curItems.Add(clothing);
                        //    curClothingIdx++;
                        //    if (curClothingIdx >= myData.foldSkinSprite.Length) curClothingIdx = 0;
                        //}
                    }
                }
            }
        }

    }
}