using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class BedWorld : BackItemWorld
    {
        [SerializeField] Transform sleepArea;
        [SerializeField] Transform cushionArea;
        private BackItemWorld myCharacter;
        private float compareDistance;

        public static System.Action<BedWorld, CushionWorld> OnCushionVerified;

        public Transform SleepArea { get => sleepArea; }
        public Transform CushionArea { get => cushionArea; }

        public override void Setup()
        {
            IsClick = true;
            base.Setup();
        }
        protected override void GetEndDragBackItem(BackItemWorld obj)
        {
            base.GetEndDragBackItem(obj);

            var isCharacter = obj.IsCharacter;
            if(isCharacter)
            {
                if (myCharacter == null || obj == myCharacter)
                {
                    compareDistance = Vector2.Distance(obj.transform.position, transform.position);
                    if (compareDistance <= 2)
                    {
                        myCharacter = obj;
                        EventDispatcher.Instance.Dispatch(new RoomEventKey.BedWithCharacter() { 
                            bed = this, 
                            newCharacter = obj.GetComponent<CharacterWorld>(),
                            wolfooWorld = obj.GetComponent<CharacterWolfooWorld>(),
                        });
                    }
                }
                else
                {
                    compareDistance = Vector2.Distance(obj.transform.position, transform.position);
                    if(compareDistance > 1)
                    {
                        myCharacter = null;
                    }
                }
            }

            var cushion = obj.GetComponent<CushionWorld>();
            if(cushion != null)
            {
                compareDistance = Vector2.Distance(cushion.transform.position, cushionArea.position);
                if(compareDistance < 1)
                {
                    OnCushionVerified?.Invoke(this, cushion);
                }
            }
        }
    }
}
