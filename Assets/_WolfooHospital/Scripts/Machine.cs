using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Machine : BackItem
    {
        [SerializeField] Animator _animator;
        [SerializeField] string animName;
        [SerializeField] Transform characterZone;
        [SerializeField] Transform itemZone;
        [SerializeField] GameObject itemPb;


        private BackItem myCharacter;
        private GameObject myItem;

        protected override void Start()
        {
            base.Start();
            _animator.enabled = false;
        }

        public void OnAnimCompleted()
        {
            if (myItem != null)
            {
                var backItem = myItem.GetComponent<BackItem>();
                if (backItem != null) backItem.AssignDrag();
                backItem.transform.SetParent(transform);
            }
        }
        public void OnCreateXPaper()
        {
            if (itemPb != null)
            {
                itemPb.SetActive(false);
                myItem = Instantiate(itemPb, itemZone);
                myItem.gameObject.SetActive(true);
                myItem.transform.localPosition = Vector3.zero;
            }
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (myCharacter != null) return;
            if (item.character != null)
            {
                if(Vector2.Distance(item.character.transform.position, characterZone.position) < 2)
                {
                    myCharacter = item.character;
                    item.character.OnGoToBed(characterZone.position, characterZone);
                    item.character.transform.localRotation = Quaternion.Euler(0,0,0);
                    _animator.enabled = true;
                    _animator.Play(animName, 0, 0);
                    SoundManager.instance.PlayOtherSfx(myClip);
                }
            }
            if (item.newCharacter != null)
            {
                if(Vector2.Distance(item.newCharacter.transform.position, characterZone.position) < 2)
                {
                    myCharacter = item.newCharacter;
                    item.newCharacter.OnGoToBed(characterZone.position, characterZone);
                    item.newCharacter.transform.localRotation = Quaternion.Euler(0,0,0);
                    _animator.enabled = true;
                    _animator.Play(animName, 0, 0);
                    SoundManager.instance.PlayOtherSfx(myClip);
                }
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (myCharacter == null) return;
            if(item.character == myCharacter)
            {
                _animator.Play(animName, 0, 0);
                _animator.enabled = false;
                myCharacter = null;
                item.character.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            if(item.newCharacter == myCharacter)
            {
                _animator.Play(animName, 0, 0);
                _animator.enabled = false;
                myCharacter = null;
                item.newCharacter.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
}