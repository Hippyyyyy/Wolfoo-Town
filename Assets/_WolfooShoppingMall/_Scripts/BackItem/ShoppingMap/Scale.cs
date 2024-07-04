using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Scale : BackItem
    {
        [SerializeField] Type myType;
        [Header("====== Normal Type =====")]
        [SerializeField] Transform plateTrans;
        [SerializeField] Transform moveZone;
        [SerializeField] Transform itemZone;

        [Header("====== Machine Type =====")]
        [SerializeField] TMP_Text heightText;
        [SerializeField] Animator _animation;
        private Tweener moveTween;
        private BackItem myCharacter;
        private Tweener _tweenCompute;

        public enum Type
        {
            Normal,
            Machine,
        }
       
        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public void OnComputeCompleted()
        {
            _tweenCompute?.Kill();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (myType == Type.Normal)
            {
                if (item.backitem == null) return;
                if (item.flower != null) return;
                if (item.shoppingBasket != null) return;
                if (item.character != null) return;
                if (item.waterProvider != null) return;
                if (item.peanut != null && item.peanut.IsGrowth) return;

                if (Vector2.Distance(item.backitem.transform.position, plateTrans.position) > 2) return;
                item.backitem.KillDragging();
                item.backitem.transform.SetParent(itemZone);
                item.backitem.JumpToEndLocalPos(Vector3.zero, null, Ease.Linear, 200);

                if (moveTween != null) moveTween?.Kill();
                plateTrans.position = new Vector3(plateTrans.position.x, moveZone.GetChild(0).position.y, 0);
                moveTween = plateTrans.DOMoveY(moveZone.GetChild(1).position.y, 1).SetEase(Ease.Linear).SetDelay(0.5f);
            }

            else if (myType == Type.Machine)
            {
                if (item.character != null || item.newCharacter != null)
                {
                    if (myCharacter != null) return;
                    BackItem compareCharacter = item.character == null ? item.newCharacter : item.character;
                    if (Vector2.Distance(compareCharacter.transform.position, plateTrans.position) < 1)
                    {
                        myCharacter = item.character;
                        compareCharacter.transform.SetParent(itemZone);
                        compareCharacter.JumpToEndLocalPos(Vector2.zero);
                        _animation.Play("Scale", 0, 0);
                            SoundManager.instance.PlayOtherSfx(myClip);
                        _tweenCompute = DOVirtual.Float(0, 1, 10, (progress) =>
                        {
                            heightText.text = UnityEngine.Random.Range(50f, 200f).ToString();
                        });
                    }
                }
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (myType == Type.Machine)
            {
                if (myCharacter == item.character || myCharacter == item.newCharacter)
                {
                    myCharacter = null;
                    _animation.Play("Scale - Idle", 0, 0);
                }
            }
        }
    }
}