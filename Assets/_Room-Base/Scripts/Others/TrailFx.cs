using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class TrailFx : CustomFx
    {
        [SerializeField] float speed;
        [SerializeField] Transform[] paths;
        [SerializeField] Ease ease;

        private float elapsedTime;
        private int countPath;
        Vector3 nextPos;
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenMove;

#if UNITY_EDITOR
        [NaughtyAttributes.Button]
        public void PlayDemo()
        {
            Play();
        }
        [NaughtyAttributes.Button]
        public void StopDemo()
        {
            Stop();
        }
#endif

        private void Start()
        {
            if (isAutoPlay)
            {
                Play();
            }
        }
        public override void Play()
        {
            myFx.Play();
            countPath = 0;
            transform.position = paths[countPath].position;
            countPath++;
            nextPos = paths[countPath].position;
      //      nextPos = new Vector3(nextPos.x, nextPos.y, transform.position.z);
            OnPlay();
        }

        public override void Stop()
        {
            myFx.Stop();
            KillTween();
        }
        private void KillTween()
        {
            if (_tweenMove != null) _tweenMove?.Kill();
        }

        public override void OnPlay()
        {
            KillTween();
            _tweenMove = transform.DOMove(nextPos, speed).SetEase(ease).SetSpeedBased(true).OnComplete(() =>
            {
                countPath++;
                if (countPath >= paths.Length)
                {
                    if (!isLoop) return;

                    countPath = 0;
                }
                nextPos = paths[countPath].position;

                OnPlay();
            });
        }
        public override void OnStop()
        {
        }
        public override void Pause()
        {
        }
    }
}
