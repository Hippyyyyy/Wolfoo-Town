using _Base;
using _Base.Helper;
using DG.Tweening;
using SCN;
using SCN.IAP;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooCity
{
    public class CityManager : MonoBehaviour
    {
        [SerializeField] GUIManager _guiManager;
        [SerializeField] Transform _city;
        [SerializeField] CityTown[] cityTowns;
        [SerializeField] ScrollRect scrollView;
        [SerializeField] CitySetting citySetting;
        [SerializeField] Image blockInput;
        [SerializeField] Animator cityAnimator;
        [SerializeField] Button characterBtn;
        [SerializeField] Button premiumBtn;

        private Tween _tween;
        private Tweener _tweenMoveMap;
        private Animator premiumAnimator;

        private void Awake()
        {
            _guiManager.gameObject.SetActive(true);
            premiumAnimator = premiumBtn.GetComponentInChildren<Animator>();
            premiumAnimator.enabled = false;
        }
        private void OnEnable()
        {
            CityAnimationEvent.OnAnimComplete += ScenePlay;
            _WolfooShoppingMall.RemoveAdsOneDayPanel.OnOpenPremium += DisablePremiumButton;
        }
        private void OnDisable()
        {
            CityAnimationEvent.OnAnimComplete -= ScenePlay;
            _WolfooShoppingMall.RemoveAdsOneDayPanel.OnOpenPremium -= DisablePremiumButton;
        }

        private void DisablePremiumButton()
        {
            premiumBtn.gameObject.SetActive(false);
        }

        private void Start()
        {
            //_Base.GameController.Instance.CheckPremiumDay();
            // var hasPreium =_Base.GameController.Instance.HasPremiumDay;
            premiumBtn.gameObject.SetActive(false);

            if (Camera.main.aspect >= 1.7)
            {
                //     Debug.Log("16:9");
                _city.localScale = Vector3.one;
            }
            else if (Camera.main.aspect >= 1.5)
            {
                //    Debug.Log("3:2");
                _city.localScale = Vector3.one;
            }
            else
            {
                //    Debug.Log("4:3");
                _city.localScale = new Vector3(1.33f, 1.33f, 0);
            }
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
            EventDispatcher.Instance.Dispatch(new EventKey.OnDestroyScene());
        }
        private void ScenePlay()
        {
            cityAnimator.enabled = true;
            premiumAnimator.enabled = true;

             var hasPreium =_Base.GameController.Instance.HasPremiumDay;

            // Play Anim House
            foreach (var house in cityTowns)
            {
                house.Play();
            }
            
            if (!BaseDataManager.Instance.playerMe.IsCityShowed(CityType.CityTown))
            {
                blockInput.gameObject.SetActive(true);
                _tween = scrollView.DOHorizontalNormalizedPos(1, 3).SetEase(Ease.Linear).OnComplete(() =>
                {
                    var tutorialSaveLoad = BaseDataManager.Instance.tutorialSaveLoadData;
                    if (tutorialSaveLoad.HasTutorial1)
                    {
                        var tutorialController = _WolfooShoppingMall.TutorialController.Instance;
                        var tutorial = tutorialController.CreateTutorial();
                        var step1 = tutorialController.CreateStep<_WolfooShoppingMall.TutorialWithBG>();

                        step1.Setup(characterBtn.transform);
                        step1.Play();
                    }
                    blockInput.gameObject.SetActive(false);
                });
            }
            else
            {
                if (!hasPreium)
                {
               //     Invoke("OpenPremiumPopup", 1);
                }
            }
        }

        public void ScrollTo(Transform endTarget, System.Action OnCompleted)
        {
            Canvas.ForceUpdateCanvases();

            var endPos = (Vector2)scrollView.transform.InverseTransformPoint(scrollView.content.position)
                    - (Vector2)scrollView.transform.InverseTransformPoint(endTarget.position)
                    + Vector2.right * (Screen.width / 4f);
            endPos.y = 0;

            _tweenMoveMap = DOVirtual.Float(0, 100, 100, (progress) =>
            {
                OnScroll(endPos, () =>
                {
                    _tweenMoveMap?.Kill();
                    OnCompleted?.Invoke();
                });
            });
        }
        private void OnScroll(Vector3 endPos, System.Action OnCompleted)
        {
            if (scrollView.content.anchoredPosition.x < endPos.x)
            {
                OnCompleted?.Invoke();
                return;
            }

            scrollView.content.anchoredPosition += Vector2.left * 100;
        }

        #region INPUT METHOD

        public void OpenStore()
        {
            _Base.FirebaseManager.instance.LogClick("iap", "city");
            IAPManager.Instance.OpenSubscriptionPanel();
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);
        }
        public void OpenCharacterScene()
        {
            _Base.FirebaseManager.instance.LogClick("character", "city");
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);
            LoadSceneManager.Instance.GoToCharacterScene();
        }

        public void OpenCommingSoonPanel()
        {
            //      _WolfooSchool.GUIManager.instance.OpenPanel(_WolfooSchool.PanelType.CommingSoon);
            GUIManager.Instance.OpenPanel(PanelType.CommingSoon);
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);
        }
        public void OnClickSetting()
        {
            _Base.FirebaseManager.instance.LogClick("setting", "city");
            GUIManager.Instance.OpenPanel(PanelType.Setting);
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);
        }
        public void OpenPremiumPopup()
        {
            _Base.FirebaseManager.instance.LogClick("PremiumOneDay", "city");
            PopupManager.Instance.OpenPanel(PanelType.PremiumOneDay);
        }
        #endregion
    }
}