using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace _WolfooSchool
{
    public class ShapeBoard : BackItem
    {
        [SerializeField] List<Image> items;
        [SerializeField] Button playGameBtn;
        [SerializeField] GameObject modePb;
        [SerializeField] ParticleSystem rainbowFx;
        [SerializeField] Panel parentPanel;
        [SerializeField] PanelType nextMode;
        [SerializeField] ParticleSystem smokeFx;

        GameObject curMode;
        bool isHasGift;
        private Vector3 startBtnScale;
        private Tweener punchTween;
        private List<Sprite> myData;
        private List<Sprite> initSpriteData = new List<Sprite>();

        protected override void Start()
        {
            base.Start();
            playGameBtn.onClick.AddListener(Playgame);
            //      EventManager.OnEndgame += GetNextShape;
            EventManager.OnUpdateShapeBoard += GetNextShape;

            startBtnScale = playGameBtn.transform.localScale;
            punchTween = playGameBtn.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 2).SetLoops(-1, LoopType.Restart);
        }
        protected override void InitData()
        {
            base.InitData();

            if (isInited) return;
            myData = GameManager.instance.ItemDataSO.shapeBoards;
            InitMyData();
        }
        private void OnEnable()
        {
            if (isHasGift)
            {
                isHasGift = false;
                Debug.Log("Get NExt Shape");
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Lighting);
                SoundManager.instance.PlayWolfooSfx(SfxWolfooType.Wow);

                int rd = UnityEngine.Random.Range(0, myData.Count);
                if (DataSceneManager.Instance.LocalDataStorage.unlockShapesBoard.Contains(false))
                {
                    if (myData.Count == 0) return;
                    while (DataSceneManager.Instance.LocalDataStorage.unlockShapesBoard[rd])
                    {
                        rd = UnityEngine.Random.Range(0, myData.Count);
                    }
                }

                DataSceneManager.Instance.UpdateNextBoardShape(rd);
                items[rd].sprite = myData[rd];
                items[rd].SetNativeSize();

                rainbowFx.transform.position = items[rd].transform.position;
                rainbowFx.Play();

                delayTween = DOVirtual.DelayedCall(2, () =>
                {
                    if (!DataSceneManager.Instance.LocalDataStorage.unlockShapesBoard.Contains(false))
                    {
                        smokeFx.Play();
                        delayTween = DOVirtual.DelayedCall(smokeFx.main.duration - 0.5f, () =>
                        {
                            EventManager.OnUpdateGift?.Invoke();
                        });

                        DataSceneManager.Instance.ResetBoardShape();
                        for (int i = 0; i < DataSceneManager.Instance.LocalDataStorage.unlockShapesBoard.Count; i++)
                        {
                            items[i].sprite = initSpriteData[i];
                            items[i].SetNativeSize();
                        }
                    }
                });
            }
        }

        private void OnDestroy()
        {
            if (punchTween != null) punchTween?.Kill();
            EventManager.OnUpdateShapeBoard -= GetNextShape;
            //     EventManager.OnEndgame -= GetNextShape;
        }

        private void InitMyData()
        {

            for (int i = 0; i < items.Count; i++)
            {
                initSpriteData.Add(items[i].sprite);
                if (DataSceneManager.Instance.LocalDataStorage.unlockShapesBoard[i])
                {
                    items[i].sprite = myData[i];
                    items[i].SetNativeSize();
                }
            }
        }
        private void Playgame()
        {
            EventManager.OnPlaygame?.Invoke(parentPanel, nextMode);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnPunchScale();
        }

        void GetNextShape(int coin, Action OnComplete)
        {
            isHasGift = true;
        }
    }
}