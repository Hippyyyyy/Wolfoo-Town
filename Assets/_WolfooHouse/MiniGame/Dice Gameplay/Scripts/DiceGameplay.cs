using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames
{
    public class DiceGameplay : MonoBehaviour
    {
        [SerializeField] DiceManager diceManager;
        [SerializeField] TileMap[] tileMaps;
        [SerializeField] DiceGameSetting gameSetting;
        [SerializeField] Transform spawnMapZone;
        [SerializeField] Transform multipleCharacterZone;
        [SerializeField] Animator animator;

        private bool isMoving = true;
        private Tween _tween;
        private TileMap tileMap;
        private Tile[] tiles;
        private Chessman chessman;
        private Chessman chessman2;
        private Chessman curChess;

        public enum PlayTurn
        {
            Wolfoo,
            Lucy,
        }
        public PlayTurn Turn { get; private set; }

        private void Start()
        {
            EventManager.OnChessManJumped = OnJumped;
            EventManager.OnJumpToEndTile = OnCompleteGame;
            EventManager.OnConfettiComplete = OnEndgame;

            InitMap();
            chessman2 = gameSetting.isTest1Chess ? null : chessman2;
            diceManager.GetReady(() => { isMoving = false; });
        }

        private void OnDestroy()
        {
            _tween?.Kill();
            _WolfooShoppingMall.SoundManager.instance.TurnOffLoop();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                GamePlay();
            }
#elif PLATFORM_ANDROID
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    GamePlay();
                }
            }
#endif
        }

        void InitMap()
        {
            tileMap = Instantiate(tileMaps[UnityEngine.Random.Range(0, tileMaps.Length)], spawnMapZone);
            tiles = tileMap.Tiles;
            for (int i = 0; i < tiles.Length; i++) { tiles[i].Id = i; }

            chessman = tileMap.Chessman;
            chessman.Assign(this, tiles);

            chessman2 = tileMap.Chesswoman;
            if (chessman2 != null) chessman2.Assign(this, tiles);

        }

        void GamePlay()
        {
            if (isMoving) return;
            isMoving = true;
            Turn = gameSetting.isTest1Chess ? Turn : (Turn == PlayTurn.Lucy ? PlayTurn.Wolfoo : PlayTurn.Lucy);
            switch (Turn)
            {
                case PlayTurn.Wolfoo:
                    curChess = chessman;
                    break;
                case PlayTurn.Lucy:
                    curChess = chessman2;
                    break;
            }

            var rdStep = UnityEngine.Random.Range(0, 6);
            rdStep = gameSetting.isTest1Chess ? gameSetting.testStepIndex : rdStep;
         //   rdStep = 5;

            diceManager.OnDicing(rdStep, () =>
            {
                Debug.Log("Diced");
                curChess.transform.SetParent(tileMap.transform);
                curChess.transform.SetAsLastSibling();
                curChess.transform.localScale = Vector3.one;
                curChess.JumpStep(rdStep + 1, () =>
                {
                    CheckTile(curChess.CurTileIndx);
                });
            });
        }

        private void CheckMultiCharacterInTile()
        {
            if (gameSetting.isTest1Chess) return;
            if(chessman.CurTileIndx == chessman2.CurTileIndx)
            {
                multipleCharacterZone.transform.parent.position = tiles[chessman.CurTileIndx].transform.position; 
                multipleCharacterZone.transform.localScale = Vector3.one;
                chessman.transform.SetParent(multipleCharacterZone.GetChild(0));
                chessman.transform.localPosition = Vector3.zero;
                chessman2.transform.SetParent(multipleCharacterZone.GetChild(1));
                chessman2.transform.localPosition = Vector3.zero;
                multipleCharacterZone.transform.localScale = Vector3.one * 0.8f;
            }
        }

        private void CheckTile(int index)
        {
            if (tiles[index].SlideTile != null)
            {
                _tween = DOVirtual.DelayedCall(0.5f, () =>
                {
                    _WolfooShoppingMall.SoundCharacterManager.Instance.Play(_WolfooShoppingMall.SoundCharacterManager.SfxWolfooType.Wow);
                    curChess.SlideTo(tiles[index], tiles[index].SlideTile.Id, () =>
                    {
            //            isMoving = false;
                        CheckMultiCharacterInTile();
                        diceManager.GetReady(() => { isMoving = false; });
                    });
                });
            }
            else if (tiles[index].TeleTile != null)
            {
                    _WolfooShoppingMall.SoundCharacterManager.Instance.Play(_WolfooShoppingMall.SoundCharacterManager.SfxWolfooType.Sad);
                _tween = DOVirtual.DelayedCall(0.5f, () =>
                {
                    curChess.TeleTo(tiles[index].TeleTile, () =>
                    {
                   //     isMoving = false;
                        CheckMultiCharacterInTile();
                        diceManager.GetReady(() => { isMoving = false; });
                    });
                });
            }
            else
            {
                Debug.Log("Nothing");
              //  isMoving = false;
                CheckMultiCharacterInTile();
                diceManager.GetReady(() => { isMoving = false; });
            }
        }

        private void OnEndgame()
        {
            _WolfooShoppingMall.SoundManager.instance.TurnOffLoop();
            Close();
        }

        public void Close()
        {
            GetComponent<UIPanel>().Hide(() =>
            {
                Destroy(this.gameObject);
            });
        }
        
        private void OnCompleteGame()
        {
            Debug.Log("End game : " + Turn.ToString() + " Win!");
            if (Turn == PlayTurn.Wolfoo)
                animator.SetTrigger("On Endgame Wolfoo");
            else if (Turn == PlayTurn.Lucy)
                animator.SetTrigger("On Endgame Lucy");

            _WolfooShoppingMall.SoundManager.instance.PlayLoopingSfx(_WolfooShoppingMall.SfxOtherType.Congratulation);
        }

        private void OnJumped()
        {
            _WolfooShoppingMall.SoundManager.instance.PlayOtherSfx(_WolfooShoppingMall.SfxOtherType.Correct);
            tiles[curChess.CurTileIndx].OnDancing();
        }
    }
}

