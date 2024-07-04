using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooCity.Minigames
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] DicerAnimation[] dicerAnimations;
        [SerializeField] Animator animator;
        [SerializeField] DiceGameplay gameplay;

        private Tweener _tween;

        private Action OnDiced;
        private int dicerIndex;
        private Tween _tweenDelay;

        private void Start()
        {
            EventManager.OnDicerInScreen = GetIntoScreen;
      //      EventManager.OnDiced = OnDiced;
        }

        private void GetIntoScreen()
        {
            Debug.Log("GetIntoScreen");
            dicerAnimations[dicerIndex].gameObject.SetActive(true);
            dicerAnimations[dicerIndex].PlayAnim(() =>
            {
            Debug.Log("OnDiced");
                OnDiced?.Invoke();
                foreach (var item in dicerAnimations)
                {
                    item.gameObject.SetActive(false);
                }
                _WolfooShoppingMall.SoundManager.instance.TurnOffLoop();
            });
        }

        private void OnDestroy()
        {
            _tween?.Kill();
            _tweenDelay?.Kill();
        }

        public void GetReady(System.Action OnComplete)
        {
            switch (gameplay.Turn)
            {
                case DiceGameplay.PlayTurn.Wolfoo:
                    animator.SetTrigger("LeftHand Ready");
                    break;
                case DiceGameplay.PlayTurn.Lucy:
                    animator.SetTrigger("RightHand Ready");
                    break;
            }

            _tweenDelay = DOVirtual.DelayedCall(0.2f, () =>
            {
                OnComplete?.Invoke();
            });
        }

        public void OnDicing(int dicerIndex, System.Action OnComplete)
        {
            Debug.Log("OnDicing");
            this.dicerIndex = dicerIndex;
            _WolfooShoppingMall.SoundManager.instance.PlayLoopingSfx(_WolfooShoppingMall.SfxOtherType.Dicing);

            switch (gameplay.Turn)
            {
                case DiceGameplay.PlayTurn.Wolfoo:
                    //animator.Play(rightHandDicingClip.name, 0, 0);
                    animator.SetTrigger("RightHand");
                    break;
                case DiceGameplay.PlayTurn.Lucy:
                    //animator.Play(leftHandDicingClip.name, 0, 0);
                    animator.SetTrigger("LeftHand");
                    break;
            }

            //overlayImg.gameObject.SetActive(true);
            //overlayImg.raycastTarget = true;
            //_tween = overlayImg.DOFade(0.3f, 0.25f).OnComplete(() =>
            //{
            //});

            OnDiced = OnComplete;
        }
    }
}