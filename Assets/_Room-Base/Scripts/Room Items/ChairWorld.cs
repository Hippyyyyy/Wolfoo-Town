using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ChairWorld : BackItemWorld
    {
        [SerializeField] SeatWorld[] seats;

        protected override void GetBeginDragBackItem(BackItemWorld obj)
        {
            base.GetBeginDragBackItem(obj);

            if (!obj.IsCharacter) return;

            foreach (var seat in seats)
            {
                seat.ReleaseItem(obj);
            }
        }


        protected override void GetEndDragBackItem(BackItemWorld obj)
        {
            base.GetEndDragBackItem(obj);

            if (obj.IsCharacter)
            {
                foreach (var seat in seats)
                {
                    var isSuccess = seat.PutItem(obj);
                    if(isSuccess)
                    {
                        EventDispatcher.Instance.Dispatch(new RoomEventKey.Chair
                        {
                            seat = seat,
                            chair = this,
                            newCharacter = obj.GetComponent<CharacterWorld>(),
                            wolfooWorld = obj.GetComponent<CharacterWolfooWorld>()
                        });
                        break;
                    }
                }
            }
        }

        internal void ReLeaseCharacter()
        {
            foreach (var seat in seats)
            {
                seat.ReleaseItem();
            }
        }

#if UNITY_EDITOR
        [NaughtyAttributes.Button]
        private void GetAllSeat()
        {
            seats = GetComponentsInChildren<SeatWorld>();
        }
#endif
    }
}
