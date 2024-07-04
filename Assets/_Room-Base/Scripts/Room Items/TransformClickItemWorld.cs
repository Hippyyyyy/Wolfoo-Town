using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class TransformClickItemWorld : BackItemWorld
    {
        [SerializeField] Sprite idleSprite;
        [SerializeField] Sprite transformSprite;
        [SerializeField] Vector3 idlePos;
        [SerializeField] Vector3 transformPos;

        protected bool CanTransform;
        public bool IsTransformed { get; private set; }
        public override void Setup()
        {
            CanTransform = true;
            IsClick = true;
            base.Setup();
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (!CanTransform) return;
            IsTransformed = !IsTransformed;
            if (IsTransformed)
            {
                MySprite.sprite = transformSprite;
                transform.localPosition = new Vector3(transformPos.x, transformPos.y, -1);
            }
            else
            {
                MySprite.sprite = idleSprite;
                transform.localPosition = new Vector3(idlePos.x, idlePos.y, -1);
            }
        }

        protected void ChangeState(bool isOpen, bool isMove = true)
        {
            if (isOpen)
            {
                MySprite.sprite = transformSprite;
                if (isMove)
                transform.localPosition = new Vector3(transformPos.x, transformPos.y, -1);
            }
            else
            {
                MySprite.sprite = idleSprite;
                if (isMove)
                    transform.localPosition = new Vector3(idlePos.x, idlePos.y, -1);
            }
        }
    }
}
