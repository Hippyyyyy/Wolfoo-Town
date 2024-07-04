using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ScrollController : MonoBehaviour
    {
        public Transform layoutSpace; // Đối tượng đại diện cho không gian bố cục
        public float scrollSpeed = 1.0f;

        void Update()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            // Di chuyển đối tượng trong không gian bố cục
            layoutSpace.Translate(Vector3.up * scrollInput * scrollSpeed);
        }

    }
}
