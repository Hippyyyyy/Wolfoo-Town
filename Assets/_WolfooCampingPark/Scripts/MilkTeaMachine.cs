using _WolfooShoppingMall.Minigame.MilkTea;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class MilkTeaMachine : BackItemWorld
    {
        [SerializeField] MilkTeaGameManager milkTeaGamePb;
        [SerializeField] CampingParkDataSO parkData;
        [SerializeField] MilkTeaCup milkTeaCupPb;
        [SerializeField] Transform cupHolder;
        [SerializeField] DoorWorld myDoor;
        [SerializeField] BoxCollider2D cupHolderCollider;
        [SerializeField] Transform cupSpawn;
        [SerializeField] Animator animator;
        [SerializeField] string releaseCupName;

        private bool isCreating;
        private MilkTeaGameManager milkTeaGame;
        private CampingParkDataSO.MilkTeaData[] myData;

        public static Action<BackItemWorld, MilkTeaMachine> OnVerified;
        private int totalClaimedCup;
        private int countReleaseCup;
        private MilkTeaCup myCurcup;

        public Transform CupHolder { get => cupHolder; }

        public override void Setup()
        {
            IsClick = true;
            base.Setup();
            myData = parkData.MilkTeas;
            cupHolderCollider.isTrigger = !myDoor.IsOpen;
        }
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            myDoor.OnChangeState += OnDoorChanged;
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            myDoor.OnChangeState -= OnDoorChanged;
        }

        private void OnDoorChanged(bool isOpen)
        {
            cupHolderCollider.enabled = !isOpen;
        }

        protected override void GetEndDragBackItem(BackItemWorld obj)
        {
            base.GetEndDragBackItem(obj);

            if(myDoor.IsOpen)
            {
                var distance = Vector2.Distance(obj.transform.position, cupHolder.position);
                if (distance < 1)
                {
                    OnVerified?.Invoke(obj, this);
                }
            }
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (isCreating) return;
            StartCoroutine(CreateMinigame());
        }
        IEnumerator CreateMinigame()
        {
            isCreating = true;

            milkTeaGame = Instantiate(milkTeaGamePb);
            milkTeaGame.OnCompleteMap = OnCompleteMapMilkTea;

            yield return new WaitForSeconds(1);
            isCreating = false;
        }

        void CreateMilkTeaCup(int index)
        {
            myCurcup = Instantiate(milkTeaCupPb, cupSpawn);
            myCurcup.Assign(myData[index]);
            StartCoroutine(AssignCup(myCurcup));
        }
        IEnumerator AssignCup(MilkTeaCup cup)
        {
            yield return new WaitForEndOfFrame();

            cup.transform.localPosition = Vector3.zero;
          //  cup.SetInBox();
            cupHolderCollider.enabled = myDoor.IsOpen;
        }

        private void OnCompleteMapMilkTea(int countCup)
        {
            if (countCup <= 0) return;
            totalClaimedCup = countCup;
            countReleaseCup = 0;
            PlayAnimReleaseCup();
        }
        void PlayAnimReleaseCup()
        {
            CreateMilkTeaCup(countReleaseCup);
            animator.Play(releaseCupName, 0, 0);
            SoundCampingParkManager.Instance.PlayOtherSfx(SoundTown<SoundCampingParkManager>.SFXType.Printing);
        }

        public void OnReleaseComplete()
        {
            Debug.Log("Test: On Release Completed");
            myCurcup.transform.SetParent(GameManager.instance.UiManager.ItemContent);
            countReleaseCup++;
            if(countReleaseCup < totalClaimedCup)
            {
                PlayAnimReleaseCup();
            }
            else
            {
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Lighting);
            }
        }
    }
}
