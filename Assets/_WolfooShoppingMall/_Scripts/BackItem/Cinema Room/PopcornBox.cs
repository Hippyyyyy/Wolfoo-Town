using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class PopcornBox : BackItem
    {
        [SerializeField] Transform popcornZone;

        private List<Transform> childPopcornZone = new List<Transform>();
        private List<Popcorn> curPopcorns = new List<Popcorn>();

        private int curIdxPopcorn;
        private float distance;

        public bool IsHasPopcorn { get; private set; }

        public List<Transform> ChildPopcornZone { get => childPopcornZone; }

        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < popcornZone.childCount; i++)
            {
                childPopcornZone.Add(popcornZone.GetChild(i));
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, popcornBox = this });
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.popcorn != null)
            {
                distance = Vector2.Distance(item.popcorn.transform.position, popcornZone.position);
                if (distance <= 2f)
                {
                    for (int i = 0; i < popcornZone.childCount; i++)
                    {
                        if (popcornZone.GetChild(i).childCount == 0)
                        {
                            item.popcorn.transform.SetParent(popcornZone.GetChild(i));
                            item.popcorn.JumpToEndLocalPos(Vector3.zero, null, Ease.Flash);
                            return;
                        }
                    }
                    item.popcorn.transform.SetParent(popcornZone.GetChild(0));
                    item.popcorn.JumpToEndLocalPos(Vector3.zero, null, Ease.Flash);
                }
            }
        }

        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            IsCarry = true;
        }

        public void CheckHasPopcorn()
        {
            for (int i = 0; i < childPopcornZone.Count; i++)
            {
                if (childPopcornZone[i].childCount > 0)
                {
                    IsHasPopcorn = true;
                    return;
                }
            }
            IsHasPopcorn = false;
        }

        public void GetPopcorn(Popcorn popcorn)
        {
            curPopcorns.Add(popcorn);
            popcorn.gameObject.SetActive(true);
            popcorn.transform.SetParent(childPopcornZone[curIdxPopcorn]);
            popcorn.JumpToEndLocalPos(
                Vector3.zero,
                () =>
                {
                    curPopcorns[curIdxPopcorn].AssignDrag();
                });

            curIdxPopcorn++;
            if (curIdxPopcorn >= popcornZone.childCount) curIdxPopcorn = 0;
        }
        public void GetPopcorn(List<Popcorn> popcorns)
        {
            IsHasPopcorn = true;
            IsFood = true;

            for (int i = 0; i < popcorns.Count; i++)
            {
                int idx = i;
                popcorns[i].gameObject.SetActive(true);

                var idxVerified = idx;
                if (idx >= childPopcornZone.Count)
                {
                    idxVerified = popcorns.Count - idx;
                }
                popcorns[idx].transform.SetParent(childPopcornZone[idxVerified]);
                popcorns[idx].JumpToEndLocalPos(
                    Vector3.zero,
                    () =>
                    {
                        popcorns[idx].AssignDrag();
                    },
                    Ease.Flash);
                curPopcorns.Add(popcorns[idx]);
            }
        }
    }
}