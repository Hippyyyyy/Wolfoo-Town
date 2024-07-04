using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ScrollingImage : MonoBehaviour
    {
        [SerializeField] RawImage rawImage;
        [SerializeField] float x, y;

        private void Update()
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(x, y) * Time.deltaTime, rawImage.uvRect.size);
            //
        }
    }
}
