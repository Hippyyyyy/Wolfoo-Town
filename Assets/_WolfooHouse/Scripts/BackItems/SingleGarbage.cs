using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SingleGarbage : BackItem
    {
        [SerializeField] Transform lidZone;
        [SerializeField] ParticleSystem smokeFx;
        [SerializeField] List<Transform> exceptionItems;
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.character != null) return;
            if (item.newCharacter != null) return;
            if (exceptionItems != null && exceptionItems.Contains(item.backitem.transform)) return;

            if(Vector2.Distance(item.backitem.transform.position, lidZone.position) < 1)
            {
                item.backitem.transform.SetParent(lidZone);
                item.backitem.JumpToEndLocalPos(lidZone.position, () =>
                {
                    item.backitem.gameObject.SetActive(false);
                    Dancing();
                    smokeFx.Play();

                    if (SoundBaseRoomManager.Instance != null) SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Garbage);
                });
            }
        }

        void Dancing()
        {
            tweenPunch?.Kill();
            transform.localScale = startScale;
            tweenPunch = transform.DOPunchScale(new Vector3(0.01f, -0.01f, 0), 0.3f, 1);
        }
    }
}