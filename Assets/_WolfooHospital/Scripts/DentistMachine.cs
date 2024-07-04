using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class DentistMachine : BackItem
    {
        [SerializeField] Transform sitZone;
        [SerializeField] Animator animator;
        private BackItem myCharacter;

        protected override void InitData()
        {
            base.InitData();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.character != null)
            {
                if (myCharacter != null) return;
                if (Vector2.Distance(item.character.transform.position, sitZone.position) < 2)
                {
                    SoundManager.instance.PlayLoopingSfx(myClip);
                    myCharacter = item.character;
                    item.character.OnSitToChair(sitZone.position, sitZone);
                    animator.Play("Run", 0, 0);
                }
            }
            if (item.newCharacter != null)
            {
                if (myCharacter != null) return;
                if (Vector2.Distance(item.newCharacter.transform.position, sitZone.position) < 2)
                {
                    SoundManager.instance.PlayLoopingSfx(myClip);
                    myCharacter = item.newCharacter;
                    item.newCharacter.OnSitToChair(sitZone.position, sitZone);
                    animator.Play("Run", 0, 0);
                }
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            SoundManager.instance.TurnOffLoop();
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.character != null)
            {
                if (item.character != myCharacter   ) return;
                myCharacter = null;
                animator.Play("Idle", 0, 0);
                SoundManager.instance.TurnOffLoop();
            }
            if (item.newCharacter != null)
            {
                if (item.newCharacter != myCharacter   ) return;
                myCharacter = null;
                animator.Play("Idle", 0, 0);
                SoundManager.instance.TurnOffLoop();
            }
        }

    }
}