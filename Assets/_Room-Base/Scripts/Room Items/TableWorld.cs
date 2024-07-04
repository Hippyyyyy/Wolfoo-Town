using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [RequireComponent(typeof(Collider2D))]
    public class TableWorld : MonoBehaviour
    {
#if UNITY_EDITOR
        [NaughtyAttributes.Required("Require for sorting Order object get into Table")]
#endif
        [SerializeField] BackItemWorld backItem;

#if UNITY_EDITOR
        [NaughtyAttributes.Button]
        private void AutoAssign()
        {
            if (backItem == null)
            {
                backItem = GetComponentInParent<BackItemWorld>();
                var colliders = GetComponents<Collider2D>();
                foreach (var collider in colliders)
                {
                    collider.isTrigger = true;
                }
            }
        }
#endif
        public int LayerOrder
        {
            get
            {
                return backItem == null ? 0 : backItem.LayerOrder;
            }
        }
        private void Start()
        {
            gameObject.layer = GameManager.instance.RoomConfig.TABLE_LAYER;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var backItem = collision.gameObject.GetComponent<BackItemWorld>();
            if (backItem && backItem.IsStandingOnTable)
            {
                backItem.triggleTable = null;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var backItem = collision.gameObject.GetComponent<BackItemWorld>();
            if (backItem && backItem.IsStandingOnTable)
            {
                backItem.triggleTable = this;
            }
        }
    }
}
