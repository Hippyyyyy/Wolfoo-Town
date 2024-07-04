using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class TestRaycast : MonoBehaviour
    {
        bool isDrag;
        private void Start()
        {
            isDrag = true;
        }
        private void FixedUpdate()
        {
            if (isDrag)
            {
                RaycastHit raycastHit2;
                Physics.Raycast( transform.position, Vector3.forward, out raycastHit2, 100);
                Debug.DrawRay(transform.position, Vector3.forward);
                if (raycastHit2.collider != null)
                {
                    Debug.Log("hit " + raycastHit2.collider.gameObject.name);
                }

            }
        }
    }
}
