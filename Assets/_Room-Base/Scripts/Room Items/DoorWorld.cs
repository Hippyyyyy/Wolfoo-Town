using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class DoorWorld : TransformClickItemWorld
    {
        [SerializeField] bool canTransform;

        public bool IsOpen;
        /// <summary>
        /// Bool params is "IsOpen"
        /// </summary>
        public System.Action<bool> OnChangeState;

        public override void Setup()
        {
            base.Setup();
            CanTransform = canTransform;
            name = "Door - " + Id;
        }
        protected override void OnClick()
        {
            Debug.Log("Door Click");
            base.OnClick();
            IsOpen = !IsOpen;
            OnChangeState?.Invoke(IsOpen);
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.ClickDoor);
        }
        public void ForceClick(bool isOpen)
        {
            ChangeState(isOpen, canTransform);
        }

#if UNITY_EDITOR
        [NaughtyAttributes.Button]
        private void Open()
        {
            ChangeState(true, true);
            IsOpen = true;
        }
        [NaughtyAttributes.Button]
        private void Close()
        {
            ChangeState(false, true);
            IsOpen = false;
        }
#endif
    }
}
