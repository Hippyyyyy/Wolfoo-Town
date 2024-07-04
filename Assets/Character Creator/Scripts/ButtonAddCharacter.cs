using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ButtonAddCharacter : MonoBehaviour
    {
        [SerializeField] Image img;
        [SerializeField] Button btn;
        [SerializeField] Sprite currentSprite;
        [SerializeField] Sprite changeSprite;
        

        public Button Btn { get => btn; set => btn = value; }

        private void Awake()
        {

        }

        private void Start()
        {
            
        }

        public void ChangeSprite()
        {
            img.sprite = changeSprite;
            img.SetNativeSize();
        }
        public void ChangeCurrentSprite()
        {
            img.sprite = currentSprite;
            img.SetNativeSize();
        }
    }
}
