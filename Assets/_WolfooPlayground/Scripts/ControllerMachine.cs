using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ControllerMachine : BackItem
    {
        [SerializeField] Button playBtn;
        [SerializeField] Button stopBtn;
        [SerializeField] ControllerCarry controllerCarry;
        [SerializeField] Image[] levelBars;
        [SerializeField] Transform levelLayout;

        public int CurLevel { get; private set; } = 1;
        public System.Action OnLevelChanged;
        public System.Action<bool> OnStateChanged;
        private Tween _tween;

        protected override void OnEnable()
        {
            base.OnEnable();
            controllerCarry.OnPlayRight += PlayNextLevel;
            controllerCarry.OnPlayleft += PlayPrevLevel;
            playBtn.onClick.AddListener(TurnOn);
            stopBtn.onClick.AddListener(TurnOff);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            controllerCarry.OnPlayRight -= PlayNextLevel;
            controllerCarry.OnPlayleft -= PlayPrevLevel;
            playBtn.onClick.RemoveListener(TurnOn);
            stopBtn.onClick.RemoveListener(TurnOff);
        }

        public void TurnOff()
        {
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            OnStateChanged?.Invoke(false);
            levelLayout.gameObject.SetActive(false);
            stopBtn.transform.localScale = new Vector3(1, 0.6f, 1);
            playBtn.transform.localScale = Vector3.one;

            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(0.25f, () =>
            {
                stopBtn.transform.localScale = Vector3.one;
            });
        }
        public void TurnOn()
        {
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            OnStateChanged?.Invoke(true);
            _tween?.Kill();
            playBtn.transform.localScale = new Vector3(1, 0.6f, 1);
            stopBtn.transform.localScale = Vector3.one;

            levelLayout.gameObject.SetActive(true);
            ChangeState();
        }
        private void ChangeState()
        {
            for (int i = 0; i < levelBars.Length; i++)
            {
                levelBars[i].gameObject.SetActive(i < CurLevel);
            }
        }
        public void PlayNextLevel()
        {
            CurLevel++;
            if(CurLevel > levelBars.Length)
            {
                CurLevel = levelBars.Length;
                return;
            }
            ChangeState();
            OnLevelChanged?.Invoke();
        }
        public void PlayPrevLevel()
        {
            CurLevel--;
            if (CurLevel < 0)
            {
                CurLevel = 0;
                return;
            }
            ChangeState();
            OnLevelChanged?.Invoke();
        }
    }
}