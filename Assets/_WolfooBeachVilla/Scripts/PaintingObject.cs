using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class PaintingObject : MonoBehaviour
    {
        [SerializeField] Image[] myImgs;
        [SerializeField] Animator _anim;
        [SerializeField] SpriteRenderer mySpriteRender;

        public void Setup(Color color)
        {
            if (myImgs != null)
            {
                foreach (var item in myImgs)
                {
                    item.color = color;
                }
            }
            mySpriteRender.color = color;
        }
        public void Setup(Sprite sprite)
        {
            if (myImgs != null)
            {
                foreach (var item in myImgs)
                {
                    item.sprite = sprite;
                }
            }
            mySpriteRender.sprite = sprite;
        }

        public void Play()
        {
            //   PlayAnim();
        }

        private void PlayAnim()
        {
            _anim.Play("Play", 0, 0);
        }
    }
}
