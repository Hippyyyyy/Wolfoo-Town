using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class BackClick : MonoBehaviour
    {
        private void OnMouseUp()
        {
            EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { background = this });
        }
    }
}