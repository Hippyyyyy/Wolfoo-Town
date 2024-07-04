using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class LightingWorld : BackItemWorld, ILighting
    {
        [SerializeField] Animator animator;
        [SerializeField] string playName;
        [SerializeField] bool isOpen;

        public void OnLighting()
        {
            if (isOpen)
            {
                if (animator != null)
                {
                    animator.Play(playName, 0, 0);
                }
            }
            else
            {
                if (animator != null)
                {
                    animator.Play("Idle", 0, 0);
                }
            }
        }

        public override void Setup()
        {
            IsClick = true;
            base.Setup();
            OnLighting();
        }
        protected override void OnClick()
        {
            base.OnClick();
            isOpen = !isOpen;
            OnLighting();
        }
    }
}
