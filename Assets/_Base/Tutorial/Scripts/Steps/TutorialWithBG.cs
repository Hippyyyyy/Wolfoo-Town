using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class TutorialWithBG : TutorialStep
    {
        [SerializeField] Image blackBgImg;
        [SerializeField] Transform pointer;
        [SerializeField] Animator animator;
        [SerializeField] string stopName;
        [SerializeField] string playName;
        [SerializeField] AnimatorHelper animatorHelper;
        [SerializeField] Canvas canvas;
        private Transform startObjectParent;
        private Transform highlightTarget;

        private void Awake()
        {
            canvas.worldCamera = Camera.main;
        }
        private void Start()
        {
            animatorHelper.OnCloseComplete = OnAnimComplete;
        }
        public override void Play()
        {
            IsPlaying = true;
            gameObject.SetActive(true);
            animator.Play(playName, 0, 0);
        }

        public override void Stop()
        {
            IsPlaying = false;
            animator.Play(stopName, 0, 0);
        }
        public void OnAnimComplete()
        {
            OnTutorialComplete?.Invoke();
            highlightTarget.SetParent(startObjectParent);
            gameObject.SetActive(false);
        }

        public void Setup(Transform highlightTarget)
        {
            gameObject.SetActive(false);
            startObjectParent = highlightTarget.transform.parent;
            this.highlightTarget = highlightTarget;
            highlightTarget.transform.SetParent(blackBgImg.transform);
            SetPointer(highlightTarget.transform.position);
        }
        private void SetPointer(Vector3 endPos)
        {
            pointer.position = endPos;

            if (pointer.position.x < 0 && pointer.position.y < 0)
            {
                pointer.rotation = Quaternion.Euler(Vector3.forward * 120f);
            }
            else if (pointer.position.x > 0 && pointer.position.y < 0)
            {
                pointer.rotation = Quaternion.Euler(Vector3.forward * -160f);
            }
            else if (pointer.position.x > 0 && pointer.position.y > 0)
            {
                pointer.rotation = Quaternion.Euler(Vector3.forward * -40);
            }
            else
            {
                pointer.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
}
