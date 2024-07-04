using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames
{
    public class Chessman : MonoBehaviour
    {
        [SerializeField] DiceGameplay.PlayTurn myTurn;
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip jumpClip;
        [SerializeField] AnimationClip idleClip;
        [SerializeField] AnimationClip danceClip;
        [SerializeField] AnimationClip slideClip;
        [SerializeField] AnimationClip fallDownClip;

        private Tween _tweenDelay;
        private Tweener _moveTween;
        private int countSlide;
        private Tile slideTile;
        private int totalStep;

        private Action OnJumpToEndStep;
        private Action OnSlidedToEndPos;
        private int curStep;
        private int curTileIndx;
        private DiceGameplay gameplay;
        private Vector3 teleEndPos;

        public Tile[] Tiles { get; private set; }
        public int CurTileIndx { get => curTileIndx; }

        private void Start()
        {
            EventManager.OnChessManJumped += OnJumped;
            EventManager.OnSlideComplete += OnSlided;
            EventManager.OnFallDown += GetFallDown;
        }
        private void OnDestroy()
        {
            _tweenDelay?.Kill();
            _moveTween?.Kill();
            EventManager.OnChessManJumped -= OnJumped;
            EventManager.OnSlideComplete -= OnSlided;
            EventManager.OnFallDown -= GetFallDown;
        }

        private void Update()
        {
            //if (isJump || isSlide)
            //{
            //    transform.Translate((endPos - transform.position) * velocity * Time.deltaTime, Space.World);
            //}
        }

        public void Assign(DiceGameplay _diceGameplay, Tile[] _tiles)
        {
            gameplay = _diceGameplay;
            Tiles = _tiles;
        }

        #region EventAnimation
        private void GetFallDown()
        {
            if (gameplay.Turn != myTurn) return;
            transform.position = teleEndPos;
        }
        private void OnSlided()
        {
            if (gameplay.Turn != myTurn) return;
            Debug.Log("OnSlided " + countSlide);
            if (countSlide >= slideTile.Paths.Length)
            {
                animator.Play(danceClip.name, 0, 0);
                OnSlidedToEndPos?.Invoke();
            }
            else
            {
                OnSlicing(Tiles[curTileIndx].transform.position);
            }
        }
        private void OnJumped()
        {
            if (gameplay.Turn != myTurn) return;
            curTileIndx++;
            if (curTileIndx == Tiles.Length)
            {
                EventManager.OnJumpToEndTile?.Invoke();
                Debug.Log("jump To End Tile");
                return;
            }
            curStep++;
            if (curStep == totalStep)
            {
                curTileIndx--;
                animator.Play(danceClip.name, 0, 0);
                OnJumpToEndStep?.Invoke();
                Debug.Log("jump To End Step");
                return;
            }

            JumpTo(Tiles[curTileIndx].transform.position);
        }
        #endregion

        #region PULIC METHOD
        public void JumpStep(int index, Action OnComplete)
        {
            curStep = 0;
            totalStep = index;
            OnJumpToEndStep = OnComplete;
            curTileIndx++;
            JumpTo(Tiles[curTileIndx].transform.position);
        }

        public void TeleTo(Tile tile, System.Action OnComplete)
        {
            teleEndPos = tile.transform.position;
            curTileIndx = tile.Id;

            animator.Play(fallDownClip.name, 0, 0);
            _tweenDelay = DOVirtual.DelayedCall(fallDownClip.length, () =>
            {
                OnComplete?.Invoke();
            });
        }

        public void SlideTo(Tile tile, int endTileIdx, System.Action OnComplete)
        {
            countSlide = 0;
            OnSlidedToEndPos = OnComplete;
            slideTile = tile;
            curTileIndx = endTileIdx;

            OnSlicing(slideTile.Paths[countSlide].position);
        }
        #endregion

        void OnSlicing(Vector3 _endPos)
        {
            Debug.Log("OnSlicing " + countSlide);
            countSlide++;
            animator.Play(slideClip.name, 0, 0);
            _moveTween = transform.DOMove(_endPos, slideClip.length);
        }

        void JumpTo(Vector3 _endPos)
        {
            animator.Play(jumpClip.name, 0, 0);
            _moveTween = transform.DOMove(_endPos, jumpClip.length);
        }
    }
}