using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Griller : BackItem
    {
        [SerializeField] Transform itemZone;
        [SerializeField] ParticleSystem smokeFx;
        [SerializeField] Door door;
        [SerializeField] float delayTime;
        [SerializeField] Transform meatSpawnZone;
        [SerializeField] Food meat;
        [SerializeField] Table grillArea;

        private Tween _delayTween;
        private bool IsOpen;
        private MeatData[] meatData;
        private Food curMeat;
        private Tween _delayTween2;
        private Transform compareItem;
        public List<Food> Foods { get; set; } = new List<Food>();
        public bool IsEnable { get; private set; } = true;

        private void OnDestroy()
        {
            if (_delayTween != null) _delayTween?.Kill();
            if (_delayTween2 != null) _delayTween2?.Kill();
        }
        protected override void Start()
        {
            base.Start();

            meatData = DataSceneManager.Instance.ItemDataSO.MeatData;
            SpawnNewMeat();
        }

        void SpawnNewMeat()
        {
            var rdIdx = UnityEngine.Random.Range(0, meatData.Length);
            curMeat = Instantiate(meat, meatSpawnZone);
            curMeat.AssignMeat(meatData[rdIdx]);
        }

        private void GetTouchDoor()
        {
            canClick = false;

            IsEnable = false;
            itemZone.gameObject.SetActive(false);
            smokeFx.Play();

            foreach (var food in Foods)
            {
                food.OnGrilling();
            }
                
            _delayTween = DOVirtual.DelayedCall(delayTime, () =>
            {
                smokeFx.Stop();
                IsEnable = true;
                door.ChangeStateSprite(false);
                itemZone.gameObject.SetActive(true);
                canClick = true;
            });
        }

        protected override void InitItem()
        {
            canClick = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            door.ChangeStateSprite(true);
            GetTouchDoor();
            SoundManager.instance.PlayOtherSfx(myClip);
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.food == null) return;
            if (!door.IsOpen) return;

            var isInside = grillArea.Is_inside(item.food.transform.position);

            if (isInside)
            {
                if (item.food != null) Foods.Add(item.food);
            }
            else
            {
                if (Foods.Contains(item.food)) Foods.Remove(item.food);
            }
        }

        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.food != null && curMeat != null)
            {
                if (item.food == curMeat)
                {
                    _delayTween2?.Kill();
                    _delayTween2 = DOVirtual.DelayedCall(0.1f, () =>
                    {
                        SpawnNewMeat();
                    });
                }
            }
        }
    }
}