using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class NumberMode : Panel
    {
        [SerializeField] List<GameObject> sessions;
        [SerializeField] Transform endSessionTrans;
        [SerializeField] Transform endOtherBallTrans;
        [SerializeField] Transform endMainBallTrans;
        [SerializeField] Button backBtn;

        int countSession = 0;
        GameObject curSession;
        private Tweener _tweenMove;
        private Tweener _scaleTween;
        private Tweener _tweenMove2;

        protected override void Start()
        {
            EventManager.OnEndSession += GetEndSession;
            EventManager.OnMoveBall += GetMoveBall;
            backBtn.onClick.AddListener(OnBack);

            curSession = sessions[0];
            for (int i = 0; i < sessions.Count; i++)
            {
                sessions[i].SetActive(i == 0);
            }
        }

        private void OnBack()
        {
            EventManager.OnBackPanel?.Invoke(this, PanelType.Room1, true);

        }

        private void GetMoveBall(List<BallAnimation> otherBalls, BallAnimation ball)
        {
            ball.transform.SetParent(transform);
            foreach (var item in otherBalls)
            {
                item.transform.SetParent(transform);
                item.PlayAnim();
                item.transform.DOMoveY(endOtherBallTrans.position.y, 5).SetSpeedBased(true);
            }
            ball.PlayAnim();
            _scaleTween = ball.transform.DOScale(ball.transform.localScale - Vector3.one * 0.1f, 1);
            _tweenMove2 = ball.transform.DOMove(endMainBallTrans.position, 1);
        }

        private void OnDisable()
        {
            EventManager.OnEndSession -= GetEndSession;
            EventManager.OnMoveBall -= GetMoveBall;
        }
        private void OnDestroy()
        {
            if (_tweenMove != null) _tweenMove?.Kill();
            if (_tweenMove2 != null) _tweenMove2?.Kill();
            if (_scaleTween != null) _scaleTween?.Kill();
        }

        private void GetEndSession()
        {
            countSession++;
            if (countSession == 1)
            {
               _tweenMove = curSession.transform.DOMoveY(endSessionTrans.position.y, 1)
                .OnStart(() =>
                {

                    sessions[countSession - 1].SetActive(false);
                    var session = sessions[countSession];
                    session.SetActive(true);
                    curSession = session;
                //session.transform.position = Vector3.down * endSessionTrans.position.y;
                //session.transform.DOMoveY(0, 1).OnComplete(() =>
                //{
                //    curSession = session;
                //});
                });
            }
            else if (countSession == 2)
            {
                OnEndgame();
            }
        }
        void OnEndgame()
        {
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Congratulation);
            EventManager.OnEndgame?.Invoke(gameObject, PanelType.Room1, true, null);
        }
    }

}