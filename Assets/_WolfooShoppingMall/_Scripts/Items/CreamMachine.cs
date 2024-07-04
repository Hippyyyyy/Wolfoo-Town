using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CreamMachine : ItemMove
    {
        [SerializeField] CreamMachineAnimation machineAnimation;
        [SerializeField] Transform limitItemZone;
        [SerializeField] Transform endParent1;
        [SerializeField] Transform endParent2;
        [SerializeField] SprinkleItem sprinkleItemPb;
        [SerializeField] int totalItem;

        private List<SprinkleItem> sprinkleItems = new List<SprinkleItem>();
        private bool canMove = true;
        private bool isCreamed;
        private bool isMoveDown;
        private Tween delayTween;

        private void Start()
        {
            machineAnimation.PlayIdle();
            SprinkleItem.OnReleaseCompleted += GetReleaseCompleted;
        }
        private void OnDestroy()
        {
        //    SprinkleItem.OnReleaseCompleted -= GetReleaseCompleted;
        }

        public void RemoveRegisterSprinkleEvent()
        {
            SprinkleItem.OnReleaseCompleted -= GetReleaseCompleted;
        }

        private void GetReleaseCompleted(SprinkleItem item)
        {
         //   sprinkleItems.Add(item);
        }

        public void RemoveCreamRemaining()
        {
            foreach (var item in sprinkleItems)
            {
                item.RemoveRemaining();
            }
        }

        public void OnPourCream(Vector3[] limitZones, CreamMachineAnimation.ColorType colorType, System.Action OnComplete)
        {
            if (!canMove) return;
            canMove = false;
            isCreamed = true;

            machineAnimation.ChangeSkin(colorType);

            if (!isMoveDown)
            {
                isMoveDown = true;
                moveTween = transform.DOMoveY(limitZones[0].y, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    OnPouring(OnComplete);
                });
            }
            else
            {
                OnPouring(OnComplete);
            }
        }
        void OnPouring(System.Action OnComplete)
        {
            if (delayTween != null) delayTween?.Kill();

            machineAnimation.PlayExcute();
            delayTween = DOVirtual.DelayedCall(machineAnimation.GetTimeAnimation(CreamMachineAnimation.AnimState.Excute) - 1.5f, () =>
            {
                foreach (var item in sprinkleItems)
                {
                    if (item != null) item.gameObject.SetActive(false);
                }
                sprinkleItems.Clear();
                machineAnimation.PlayIdle();
                canMove = true;
                OnComplete?.Invoke();
            });
        }

        public void OnSprinkle(Sprite[] itemSprites, Vector3[] listToppingPos, Vector3[] limitZones, System.Action OnComplete)
        {
            if (!canMove) return;
            canMove = false;

            if (!isMoveDown)
            {
                isMoveDown = true;
                moveTween = transform.DOMoveY(limitZones[0].y, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    OnExcuting(itemSprites, listToppingPos, limitZones, OnComplete);
                });
            }
            else
            {
                OnExcuting(itemSprites, listToppingPos, limitZones, OnComplete);
            }

        }

        void OnExcuting(Sprite[] itemSprites, Vector3[] listToppingPos, Vector3[] limitZones, System.Action OnComplete)
        {
            var duration = 0.75f;
            startPos = transform.position;

            moveTween = transform.DOMoveX(limitZones[1].x, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                moveTween = transform.DOMoveX(limitZones[2].x, duration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    moveTween = transform.DOMoveX(startPos.x, duration).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        canMove = true;
                        OnComplete?.Invoke();
                    });
                });
            });

            for (int i = 0; i < totalItem; i++)
            {
                int rd = Random.Range(0, 5);
                var item = Instantiate(sprinkleItemPb, endParent1);
                //item.transform.position = new Vector3(
                //    Random.Range(limitItemZone.GetChild(0).position.x, limitItemZone.GetChild(1).position.y),
                //    limitItemZone.position.y, 
                //    0);
                item.transform.localPosition = Vector3.zero;

                if (rd >= 2)
                {
                    if (isCreamed)
                    {
                        item.AssignItem(true,
                            itemSprites[Random.Range(0, itemSprites.Length)],
                            new Vector3(Random.Range(listToppingPos[0].x, listToppingPos[1].x), Random.Range(listToppingPos[0].y, listToppingPos[1].y), 0));
                    }
                    else
                    {
                        item.AssignItem(false,
                            itemSprites[Random.Range(0, itemSprites.Length)],
                            new Vector3(
                                Random.Range(listToppingPos[0].x, listToppingPos[1].x), UISetupManager.Instance.outsideDown.position.y, 0));
                    }
                }
                else
                {
                    item.AssignItem(false,
                        itemSprites[Random.Range(0, itemSprites.Length)],
                        new Vector3(Random.Range(listToppingPos[0].x, listToppingPos[1].x), UISetupManager.Instance.outsideDown.position.y, 0));
                }

                item.OnRelease(Random.Range(duration, duration * 3), endParent2);
                sprinkleItems.Add(item);
            }
        }
    }
}