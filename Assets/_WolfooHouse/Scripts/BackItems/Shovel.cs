using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Shovel : BackItem
    {
        [SerializeField] Animator animator;
        [SerializeField] string digAnimName;
        [SerializeField] string digGroundAnimName;
        [SerializeField] string pourAnimName;
        [SerializeField] string pourGroundAnimName;
        [SerializeField] Image plantImg;
        [SerializeField] GameObject soil;
        private Flower shovelingPlant;

        private bool hasSoil;
        public bool HasSoil
        {
            get => hasSoil; 
            
            private set
            {
                hasSoil = value;
                soil.SetActive(value);
            }
        }
        public bool HasPlant
        {
            get => shovelingPlant != null;
        }

        protected override void Start()
        {
            base.Start();
            animator.enabled = false;
        }
        protected override void InitItem()
        {
            canDrag = true;
            base.InitItem();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, shovel = this });
        }

        public void OnDigAnimationComplete()
        {
            canDrag = true;
            animator.enabled = false;
            HasSoil = true;
            transform.SetParent(Content.transform);
        }
        public void OnPourAnimationComplete()
        {
            if (shovelingPlant != null)
            {
                shovelingPlant.gameObject.SetActive(true);
                shovelingPlant = null;
            }

            animator.enabled = false;
            canDrag = true;
            HasSoil = false;
            transform.SetParent(Content.transform);
        }
        public void OnAnimDigging()
        {
            shovelingPlant.transform.SetParent(transform);
            shovelingPlant.gameObject.SetActive(false);
            shovelingPlant.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        public void OnAnimPouring()
        {

        }

        public void Pour(Vector3 endPos, Transform plantParent)
        {
            if (!HasSoil) return;

            canDrag = false;
            IsAssigned = true;
            KillDragging();
            tweenMove = transform.DOMove(endPos, 0.5f).OnComplete(() =>
            {
                animator.enabled = true;
                animator.Play(pourAnimName, 0, 0);

                if(shovelingPlant != null)
                {
                    shovelingPlant.transform.SetParent(plantParent);
                    shovelingPlant.transform.position = endPos;
                }
            });
        }

        public void Dig(Vector3 endPos, Flower flower)
        {
            if (HasSoil) return;

            canDrag = false;
            IsAssigned = true;
            KillDragging();
            tweenMove = transform.DOMove(endPos, 0.5f).OnComplete(() =>
            {
                animator.enabled = true;
                animator.Play(digAnimName, 0, 0);

                plantImg.sprite = flower.GetImage().sprite;
                plantImg.SetNativeSize();

                shovelingPlant = flower;
            });
        }
        public void Dig(Vector3 endPos)
        {
            if (HasSoil) return;

            canDrag = false;
            IsAssigned = true;
            KillDragging();
            tweenMove = transform.DOMove(endPos, 0.5f).OnComplete(() =>
            {
                animator.enabled = true;
                animator.Play(digGroundAnimName, 0, 0);
            });
        }
    }
}
