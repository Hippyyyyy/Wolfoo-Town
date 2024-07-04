using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SlideToy : BackItem
    {
        [SerializeField] Transform locationZone;
        [SerializeField] bool isForCharacter;


        protected override void InitItem()
        {
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.carToy != null)
            {
                if (isForCharacter) return;
                if (item.carToy.transform.position.x < locationZone.GetChild(0).position.x ||
                   item.carToy.transform.position.x > locationZone.GetChild(locationZone.childCount - 2).position.x ||
                   item.carToy.transform.position.y > locationZone.GetChild(0).position.y + 1)
                    return;

                item.carToy.OnSlide(locationZone, transform);
            }
            if (item.character != null)
            {
                if (!isForCharacter) return;

                if (Vector2.Distance(item.character.transform.position, locationZone.position) > 5) return;

                item.character.OnSlide(locationZone, transform);
            }
            if (item.newCharacter != null)
            {
                if (!isForCharacter) return;

                if (Vector2.Distance(item.newCharacter.transform.position, locationZone.position) > 5) return;

                item.newCharacter.OnSlide(locationZone, transform);
            }
        }
    }
}