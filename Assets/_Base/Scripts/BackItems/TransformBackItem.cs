using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class TransformBackItem : BackItem
    {
        [SerializeField] Sprite transformSprite;
        [SerializeField] Sprite idleSprite;

        [SerializeField] protected bool isTransform;

        protected void SetState()
        {
            if (isTransform)
            {
                image.sprite = transformSprite;
                image.SetNativeSize();
            }
            else
            {
                image.sprite = idleSprite;
                image.SetNativeSize();
            }
        }
        public void PlayTransform()
        {
            SoundManager.instance.PlayOtherSfx(myClip);
            isTransform = true;
            SetState();
        }
    }
}