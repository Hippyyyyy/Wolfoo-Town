using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Lamp : BackItem
    {
        [SerializeField] Sprite[] spriteStates;
        [SerializeField] bool stateOn = true;
        [SerializeField] bool isAuto;
        protected override void InitItem()
        {
            canClick = true;
            ChangeState(stateOn);
        }
        protected override void Start()
        {
            base.Start();
        }

        void ChangeState(bool _state)
        {
            stateOn = _state ;
            image.sprite = spriteStates[stateOn ? 1 : 0];
            image.SetNativeSize();
            UISetupManager.Instance.blackBgNoMask.gameObject.SetActive(!stateOn);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            ChangeState(!stateOn);

            if (SoundManager.instance != null) SoundManager.instance.PlayOtherSfx(myClip);
            else SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Lamp);

            tweenScale?.Kill();
            transform.localScale = startScale;
            tweenScale = transform.DOPunchScale(Vector3.up * 0.1f, 0.5f, 1);

            EventDispatcher.Instance.Dispatch(new EventKey.OnClickBackItem { lamp = this, state = stateOn });
        }

        protected override void GetClickBackItem(EventKey.OnClickBackItem item)
        {
            base.GetClickBackItem(item);
            if (item.lamp == null) return;
            if (item.lamp.id == id) return;

            ChangeState(item.state);
        }
    }
}