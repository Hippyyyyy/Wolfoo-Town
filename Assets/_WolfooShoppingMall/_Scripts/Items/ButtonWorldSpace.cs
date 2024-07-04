using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ButtonWorldSpace : MonoBehaviour
    {
        private void OnMouseDown()
        {
            EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { button = this });
        }
        private void OnMouseUp()
        {

        }
    }
}