using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SeatWorld : MonoBehaviour
    {
        public bool IsOpen = true;
        private BackItemWorld itemInChair;

        public void Assign(bool isOpen)
        {
            IsOpen = isOpen;
        }
        public bool HasItem(BackItemWorld backItem)
        {
            return backItem == itemInChair;
        }
        public void ReleaseItem()
        {
            itemInChair.SetToBackground();
            itemInChair = null;
        }
        public void ReleaseItem(BackItemWorld backItem)
        {
            if (backItem == itemInChair)
            {
                itemInChair.SetToBackground();
                itemInChair = null;
            }
        }
        public bool PutItem(BackItemWorld backItem)
        {
            if(IsOpen && itemInChair == null)
            {
                if (Vector2.Distance(backItem.transform.position, transform.position) < 1f)
                {
                    itemInChair = backItem;
                    transform.SetAsLastSibling();
                    return true;
                }
            }

            return false;
        }
    }
}
