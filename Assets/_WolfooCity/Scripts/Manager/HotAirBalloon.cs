using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

namespace _WolfooCity
{
    public class HotAirBalloon : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] Transform movingPathHolder;
        [SerializeField] Transform videoHolder;
        [SerializeField] CityVideo videoManagerPb;
        private int countPath;
        private Tweener _tweenMove;

        public void OnPressBalloon()
        {
            var videoManager = Instantiate(videoManagerPb, videoHolder);
        }

        private void Start()
        {
            BeginMove();
        }
        private void OnDestroy()
        {
            if (_tweenMove != null) _tweenMove?.Kill();
        }
        private void BeginMove()
        {
            countPath = Random.Range(0, movingPathHolder.childCount);
            transform.localPosition = movingPathHolder.GetChild(countPath).localPosition;
            Moving();
        }
        private void Moving()
        {
            countPath++;
            if (countPath >= movingPathHolder.childCount) countPath = 0;
            var nextPos = movingPathHolder.GetChild(countPath).localPosition;
            _tweenMove = transform.DOLocalMove(nextPos, speed).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                Moving();
            });
        }
    }
}
