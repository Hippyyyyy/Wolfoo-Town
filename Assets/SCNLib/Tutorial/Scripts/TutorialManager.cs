using DG.Tweening;
using SCN.Animation;
using SCN.Common;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SCN.Tutorial
{
    public enum Gesture
    {
        Hold = 0,
        Click = 1,
        PointAt = 2,
        Spin = 3,
        SpinFlat = 4,
    }

    public class TutorialManager : MonoBehaviour
    {
        static TutorialManager instance;
        public static TutorialManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Setup();
                }

                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public static System.Action OnUserNoReact;

        TutorialSettings setting;
        public TutorialSettings Setting => setting;

        static void Setup()
        {
            instance = Instantiate(LoadSource.LoadObject<GameObject>("Tutorial Canvas"), DDOL.Instance.transform)
                .GetComponent<TutorialManager>();
            instance.setting = LoadSource.LoadObject<TutorialSettings>(TutorialSettings.AssetName);

            instance.pointerAnim.InitValue();

            // Canvas
            var canvas = instance.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = instance.setting.SortingLayer;
            canvas.sortingOrder = instance.setting.OrderInLayer;
            instance.GetComponent<CanvasScaler>().referenceResolution= instance.setting.ReferenceResolution;

            instance.pars = instance.parTrans.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < instance.pars.Length; i++)
            {
                var renderer = instance.pars[i].GetComponent<Renderer>();
                renderer.sortingLayerName = instance.setting.SortingLayer;
                renderer.sortingOrder = instance.setting.OrderInLayer + 1;
            }

            instance.pointerTrans.GetChild(0).localScale = Vector3.one * instance.setting.PointerScale;
            instance.pointerTrans.gameObject.SetActive(false);
            instance.spinArrowTrans.gameObject.SetActive(false);
            if (instance.setting.SpinArrowSprite != null)
            {
                for (int i = 0; i < instance.arrowImage.Length; i++)
                {
                    instance.arrowImage[i].sprite = instance.setting.SpinArrowSprite;
                }
            }

            instance.focusController.gameObject.SetActive(false);
        }

        [Header("Pointer")]
        [SerializeField] RectTransform pointerTrans;
        [SerializeField] Transform parTrans;
        ParticleSystem[] pars;

        [SerializeField] AnimationSpineController pointerAnim;
        [SerializeField] PointerAnimName animName;
        
        [Header("Spin")]
        [SerializeField] RectTransform spinArrowTrans;
        [SerializeField] UnityEngine.Animation spinAnimation;
        [SerializeField] Image[] arrowImage;

        [Header("Focus")]
        [SerializeField] FocusController focusController;

        bool isFirstTime;

        private void Awake()
        {
            SceneManager.sceneLoaded += Event_sceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= Event_sceneLoaded;
        }

        private void Start()
        {
            AttachCamera();
        }

        public void Preload()
		{

		}

        void Event_sceneLoaded(Scene _null1, LoadSceneMode _null2)
        {
            AttachCamera();
        }

        void AttachCamera()
        {
            if (Camera.main != null)
            {
                GetComponent<Canvas>().worldCamera = Camera.main;
            }
        }

		#region Focus public methob
        public void StartFocus(RectTransform target, bool hasAnim = true
            , System.Action onComplete = null)
		{
            Stop();
            focusController.gameObject.SetActive(true);
            focusController.Execute(target, setting.FocusAlpha, hasAnim, onComplete, setting.FocusExpand);
        }

        public void StopFocus(bool hasAnim = true, System.Action onComplete = null)
		{
            focusController.UnFocus(hasAnim, ()=> 
            {
                focusController.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
		}
		#endregion

		#region Pointer public methob
		/// <summary>
		/// Ngón tay chỉ vào vị trí nào đó cố định
		/// </summary>
		/// <param name="pos">Vị trí chỉ ngón tay</param>
		/// <param name="isRight">Bàn tay bên phải hay bên trái vị trí đó</param>
		public void StartPointer(Vector3 pos, Gesture gesture, bool isRight = true)
        {
            isFirstTime = true;
            StartPointerLoop(pos, gesture, isRight);
        }
        void StartPointerLoop(Vector3 pos, Gesture gesture, bool isRight = true)
        {
            WaitUserNoReact(() =>
            {
				SetupPointer(pos, gesture, isRight);
				pointerTrans.position = pos;

				_ = StartCoroutine(IEWaitUserClick(() =>
				{
					StopPointer();
					StartPointerLoop(pos, gesture, isRight);
				}));
			});
        }

        /// <summary>
        /// Ngón tay chỉ theo 1 GameObject, di chuyển follow nếu GameObject đó di chuyển
        /// </summary>
        /// <param name="transform">Object được follow</param>
        /// <param name="isRight">Bàn tay bên phải hay bên trái Object đó</param>
        public void StartPointer(Transform transform, Gesture gesture, bool isRight = true)
        {
            isFirstTime = true;
            StartPointerLoop(transform, gesture, isRight);
        }
        void StartPointerLoop(Transform transform, Gesture gesture, bool isRight = true)
        {
            WaitUserNoReact(() =>
            {
                SetupPointer(transform.position, gesture, isRight);
                PlayParticles(true);
                var corou = StartCoroutine(IEFollowTrans(transform));

                _ = StartCoroutine(IEWaitUserClick(() =>
                {
                    StopCoroutine(corou);
                    StopPointer();

                    StartPointerLoop(transform, gesture, isRight);
                }));
            });
        }

        IEnumerator IEFollowTrans(Transform trans)
        {
            while (true)
            {
                pointerTrans.position = trans.position;
                yield return null;
            }
        }

        /// <summary>
        /// Ngón tay chỉ từ vị trí A đến vị trí B
        /// </summary>
        /// <param name="startPos">Vị trí đầu</param>
        /// <param name="endPos">Vị trí cuối</param>
        /// <param name="gesture">Kiểu ngón tay</param>
        /// <param name="isRight">Bàn tay quay bên phải hay bên trái</param>
        public void StartPointer(Vector3 startPos, Vector3 endPos
            , Gesture gesture, bool isRight = true, LoopType loopType = LoopType.Restart)
        {
            isFirstTime = true;
            StartPointerLoop(startPos, endPos, gesture, isRight, loopType);
        }
        void StartPointerLoop(Vector3 startPos, Vector3 endPos
            , Gesture gesture, bool isRight = true, LoopType loopType = LoopType.Restart)
        {
            WaitUserNoReact(() =>
            {
                SetupPointer(transform.position, gesture, isRight);

                if (loopType == LoopType.Restart)
                {
                    MovePointerRestart(startPos, endPos, () =>
                    {
                        PlayParticles(true);
                    }, () =>
                    {
                        PlayParticles(false);
                    });
                }
                else if (loopType == LoopType.Yoyo)
                {
                    MovePointerYoyo(startPos, endPos);
                    PlayParticles(true);
                }

                _ = StartCoroutine(IEWaitUserClick(() =>
                {
                    StopPointer();
                    StartPointerLoop(startPos, endPos, gesture, isRight, loopType);
                }));
            });
        }
        void MovePointerRestart(Vector3 startPos, Vector3 endPos
        , TweenCallback onStepStart = null, TweenCallback onStepComplete = null)
        {
            pointerTrans.position = startPos;
            onStepStart?.Invoke();

            _ = DOTweenManager.Instance.TweenMoveVelocity(pointerTrans, endPos,
                setting.PointerVelocity, false).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    onStepComplete?.Invoke();
                    MovePointerRestart(startPos, endPos, onStepStart, onStepComplete);
                });
        }
        void MovePointerYoyo(Vector3 startPos, Vector3 endPos)
        {
            _ = DOTweenManager.Instance.TweenMoveVelocity(pointerTrans, startPos, endPos,
                setting.PointerVelocity, false).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// Ngón tay chỉ từ vị trí A đến vị trí B
        /// </summary>
        /// <param name="startTrans">Vị trí đầu</param>
        /// <param name="endTrans">Vị trí cuối</param>
        /// <param name="gesture">Kiểu ngón tay</param>
        /// <param name="isRight">Bàn tay quay bên phải hay bên trái</param>
        public void StartPointer(Transform startTrans, Transform endTrans, Gesture gesture, bool isRight = true)
        {
            isFirstTime = true;
            StartPointerLoop(startTrans, endTrans, gesture, isRight);
        }
        void StartPointerLoop(Transform startTrans, Transform endTrans, Gesture gesture, bool isRight = true)
        {
            WaitUserNoReact(() =>
            {
                SetupPointer(transform.position, gesture, isRight);
                MovePointer(startTrans, endTrans, () =>
                {
                    PlayParticles(true);
                }, () =>
                {
                    PlayParticles(false);
                });

                _ = StartCoroutine(IEWaitUserClick(() =>
                {
                    StopPointer();
                    StartPointerLoop(startTrans, endTrans, gesture, isRight);
                }));
            });
        }
        void MovePointer(Transform startTrans, Transform endTrans
        , TweenCallback onStepStart = null, TweenCallback onStepComplete = null)
        {
            pointerTrans.position = startTrans.position;
            onStepStart?.Invoke();

            _ = DOTweenManager.Instance.TweenMoveVelocity(pointerTrans, endTrans.position,
                setting.PointerVelocity, false).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    onStepComplete?.Invoke();
                    MovePointer(startTrans, endTrans, onStepStart, onStepComplete);
                });
        }

        public void Stop()
        {
            _ = StartCoroutine(IEStop());
        }
        IEnumerator IEStop()
        {
            yield return new WaitForEndOfFrame();
            StopAllCoroutines();

            pointerTrans.gameObject.SetActive(false);
            pointerTrans.DOKill();
            pointerAnim.DOKill();
        }
		#endregion

		#region Pointer private methob
		Coroutine waitUserNoReactCorou;
        void WaitUserNoReact(System.Action onNoReact)
        {
            if (waitUserNoReactCorou != null)
            {
                StopCoroutine(waitUserNoReactCorou);
            }
            waitUserNoReactCorou = StartCoroutine(IEWaitUserNoReact(onNoReact));
        }
        IEnumerator IEWaitUserNoReact(System.Action onNoReact)
        {
            var timer = 0f;
            var waitTime = isFirstTime ? setting.DelayStartTime : setting.NoReactTime;

            while (true)
            {
                if (CheckUserClick())
                {
                    yield return new WaitForEndOfFrame();
                    WaitUserNoReact(onNoReact);
                    break;
                }
                timer += Time.unscaledDeltaTime;

                if (timer > waitTime)
                {
                    isFirstTime = false;

                    onNoReact?.Invoke();
                    break;
                }

                yield return null;
            }
        }

        IEnumerator IEWaitUserClick(System.Action onUserClick)
        {
            while (true)
            {
                if (CheckUserClick())
                {
                    onUserClick?.Invoke();
                    break;
                }

                yield return null;
            }
        }

        bool CheckUserClick()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
			if (Input.touchCount > 0)
			{
				foreach (var touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						return true;
					}
				}
			}

			return false;
#endif
        }

        void SetupPointer(Vector3 position, Gesture gesture, bool isRight)
        {
            if (gesture == Gesture.Hold
                || gesture == Gesture.Click
                || gesture == Gesture.PointAt)
            {
                pointerTrans.gameObject.SetActive(true);
                pointerTrans.position = position;
                pointerTrans.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
            }
            else if (gesture == Gesture.Spin
                    || gesture == Gesture.SpinFlat)
            {
                spinArrowTrans.gameObject.SetActive(true);
                spinArrowTrans.position = position;
                spinArrowTrans.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
            }

            switch (gesture)
            {
                case Gesture.Hold:
                    pointerAnim.PlayAnimation(animName.Hold, false);
                    break;
                case Gesture.Click:
                    pointerAnim.PlayAnimation(animName.Click, true);
                    break;
                case Gesture.PointAt:
                    pointerAnim.PlayAnimation(animName.Idle, false);
                    break;
                case Gesture.Spin:
                    spinAnimation.Play("SpinArrow");
                    break;
                case Gesture.SpinFlat:
                    spinAnimation.Play("SpinArrowFlat");
                    break;
            }
        }

        void StopPointer()
        {
            pointerAnim.StopCurrentAnimation();
            pointerTrans.DOKill();
            PlayParticles(false);

            pointerTrans.gameObject.SetActive(false);

            spinAnimation.Stop();
            spinArrowTrans.gameObject.SetActive(false);
        }

        void PlayParticles(bool b)
        {
            if (b)
            {
                for (int i = 0; i < pars.Length; i++)
                {
                    pars[i].Play();
                }
            }
            else
            {
                for (int i = 0; i < pars.Length; i++)
                {
                    pars[i].Stop();
                }
            }
        }
		#endregion
	}
}