using _WolfooSchool;
using DG.Tweening;
using SCN;
using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class IceScreamMode : MonoBehaviour
    {
        [SerializeField] Button[] topicBtns;
        [SerializeField] Image[] topicImgs;
        [SerializeField] HorizontalScroll[] horizontalScrolls;
        [SerializeField] Image coneImg;
        [SerializeField] Image creamImg;
        [SerializeField] ParticleSystem[] completeFx;
        [SerializeField] Transform characterZone;
        [SerializeField] Image backgroundImg;
        [SerializeField] CreamMachine creamMachine;
        [SerializeField] Transform moveMachineZone;
        [SerializeField] Transform[] toppingZones;
        [SerializeField] Transform[] moveConeZOnes;
        [SerializeField] Food food;
        [SerializeField] Button backZoneBtn;
        [SerializeField] Button backBtn;
        [SerializeField] IngameType ingameSoundType;
        [SerializeField] _WolfooCity.UIPanel uIPanel;
        private AudioClip startClip;

        private IceScreamModeData data;
        private Vector3 startCharacterScale;
        private Tween delayTween;
        private int curIdx = -1;
        private Tweener rotateTween;
        private Tweener scaleTween;
        private bool canClick;
        private Tweener moveTween;
        private bool isCreated;


        private void Awake()
        {
            creamMachine.transform.position = moveMachineZone.GetChild(0).position;
            backgroundImg.transform.localScale = Vector3.zero;
            creamImg.gameObject.SetActive(false);
        }
        private void Start()
        {
            data = DataSceneManager.Instance.ItemDataSO.IceScreamModeData;

            //      AdsManager.Instance.HideBanner();
            if (SoundManager.instance.Music != null)
                startClip = SoundManager.instance.Music.clip;
            SoundManager.instance.PlayIngame(ingameSoundType);

            backZoneBtn.onClick.AddListener(OnBack);
            backBtn.onClick.AddListener(OnBack);
            startCharacterScale = characterZone.localScale;

            InitEvent();
            InitData();

            OnClickTopic(0);

            scaleTween = backgroundImg.transform.DOScale(Vector3.one * 0.75f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                foreach (var button in topicBtns)
                {
                    button.interactable = false;
                }
                if (moveTween != null) moveTween?.Kill();
                moveTween = coneImg.transform.DOMoveY(moveConeZOnes[1].position.y, 0.5f).OnComplete(() =>
                {
                    coneImg.sprite = data.coneTopicData[0];
                    coneImg.SetNativeSize();
                    moveTween = coneImg.transform.DOMoveY(moveConeZOnes[0].position.y, 0.5f).OnComplete(() =>
                    {
                        foreach (var button in topicBtns)
                        {
                            button.interactable = true;
                        }
                    });
                });

                canClick = true;
            });
        }

        private void OnDestroy()
        {
            if (delayTween != null) delayTween?.Kill();
            if (moveTween != null) moveTween?.Kill();
            if (rotateTween != null) rotateTween?.Kill();
            if (scaleTween != null) scaleTween?.Kill();
            EventDispatcher.Instance.RemoveListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragItem>(GetEndDragItem);
            SoundManager.instance.PlayIngame(startClip);

            creamMachine.RemoveCreamRemaining();
        }

        private void InitEvent()
        {
            topicBtns[0].onClick.AddListener(() => OnClickTopic(0));
            topicBtns[1].onClick.AddListener(() => OnClickTopic(1));
            topicBtns[2].onClick.AddListener(() => OnClickTopic(2));

            EventDispatcher.Instance.RegisterListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }

        private void GetEndDragItem(EventKey.OnEndDragItem obj)
        {
        }

        private void GetClickItem(EventKey.OnClickItem obj)
        {
            if (obj.iceScreamItem != null)
            {
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);

                var rd = UnityEngine.Random.Range(0, completeFx.Length);

                //data.dollClothingDicts[curIdx].curTopicIdx = curIdx;
                //data.dollClothingDicts[curIdx].curItemIdx = obj.iceScreamItem.Id;

                isCreated = true;

                switch (obj.iceScreamItem.TopicIdx)
                {
                    case 0: // Cone
                        foreach (var button in topicBtns)
                        {
                            button.interactable = false;
                        }
                        if (moveTween != null) moveTween?.Kill();
                        moveTween = coneImg.transform.DOMoveY(moveConeZOnes[1].position.y, 0.5f).OnComplete(() =>
                        {
                            coneImg.sprite = data.coneTopicData[obj.iceScreamItem.Id];
                            coneImg.SetNativeSize();

                            moveTween = coneImg.transform.DOMoveY(moveConeZOnes[0].position.y, 0.5f).OnComplete(() =>
                            {
                                foreach (var button in topicBtns)
                                {
                                    button.interactable = true;
                                }
                            });
                        });

                        break;
                    case 1: // Topping

                        //if (rotateTween != null) rotateTween?.Kill();
                        //coneImg.transform.rotation = Quaternion.Euler(Vector3.zero);

                        foreach (var button in topicBtns)
                        {
                            button.interactable = false;
                        }
                        creamMachine.OnPourCream(
                            new Vector3[] { moveMachineZone.GetChild(1).position, moveMachineZone.GetChild(2).position, moveMachineZone.GetChild(3).position },
                            data.colorAnimTypes[obj.iceScreamItem.Id],
                            () =>
                            {
                                creamImg.sprite = data.creamSprites[obj.iceScreamItem.Id];
                                creamImg.SetNativeSize();
                                creamImg.gameObject.SetActive(true);

                                foreach (var button in topicBtns)
                                {
                                    button.interactable = true;
                                }
                            });

                        break;
                    case 2: // Cream
                        foreach (var button in topicBtns)
                        {
                            button.interactable = false;
                        }
                        creamMachine.OnSprinkle(data.toppingTopicData[obj.iceScreamItem.Id].itemSprites,
                            new Vector3[] { toppingZones[0].position, toppingZones[1].position },
                            new Vector3[] { moveMachineZone.GetChild(1).position, moveMachineZone.GetChild(2).position, moveMachineZone.GetChild(3).position },
                            () =>
                            {
                                foreach (var button in topicBtns)
                                {
                                    button.interactable = true;
                                }
                            });
                        break;
                }
            }
        }

        private void OnClickTopic(int idx)
        {
            if (curIdx == idx) return;
            curIdx = idx;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            for (int i = 0; i < topicBtns.Length; i++)
            {
                if (i == idx)
                {
                    topicImgs[i].sprite = data.topicBtnSprites2[i];
                    topicBtns[i].image.sprite = data.chooseBtnSprites[1];
                    topicImgs[i].SetNativeSize();
                    topicBtns[i].image.SetNativeSize();
                    horizontalScrolls[i].gameObject.SetActive(true);
                }
                else
                {
                    horizontalScrolls[i].gameObject.SetActive(false);
                    topicImgs[i].sprite = data.topicBtnSprites[i];
                    topicBtns[i].image.sprite = data.chooseBtnSprites[0];
                    topicImgs[i].SetNativeSize();
                    topicBtns[i].image.SetNativeSize();
                }
            }
        }

        private void InitData()
        {
            for (int i = 0; i < horizontalScrolls.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        horizontalScrolls[i].Setup(data.coneTopicData.Length, this);
                        break;
                    case 1:
                        horizontalScrolls[i].Setup(data.screamTopicSprites.Length, this);
                        break;
                    case 2:
                        horizontalScrolls[i].Setup(data.toppingTopicData.Length, this);
                        break;
                }

                topicImgs[i].sprite = data.topicBtnSprites[i];
                topicBtns[i].image.sprite = data.chooseBtnSprites[0];
                topicImgs[i].SetNativeSize();
                topicBtns[i].image.SetNativeSize();
            }


            delayTween = DOVirtual.DelayedCall(0.25f, () =>
            {
                for (int i = 0; i < horizontalScrolls.Length; i++)
                {
                    int count = 0;
                    foreach (var item in horizontalScrolls[i].MaskTrans.GetComponentsInChildren<IceScreamScrollItem>())
                    {
                        switch (i)
                        {
                            case 0:
                                item.AssignItem(i, count, data.coneTopicData[count]);
                                break;
                            case 1:
                                item.AssignItem(i, count, data.screamTopicSprites[count]);
                                break;
                            case 2:
                                item.AssignItem(i, count, data.toppingTopicData[count].topicSprite);
                                break;
                        }
                        count++;
                    }
                }
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnBack();
        }
        void OnBack()
        {
            if (!canClick) return;
            canClick = false;


            if (scaleTween != null) scaleTween?.Kill();
            UISetupManager.Instance.maskBg.gameObject.SetActive(true);

            uIPanel.Hide(() =>
            {
                if (isCreated)
                    EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { iceScream = this, food = food });
                Destroy(gameObject);
                UISetupManager.Instance.maskBg.gameObject.SetActive(false);
            });
            //scaleTween = backgroundImg.transform.DOScale(0.5f, 0.25f)
            //.SetEase(Ease.InBack)
            //.OnComplete(() =>
            //{

            //    //UISetupManager.Instance.maskBg.gameObject.SetActive(false);
            //    //Destroy(gameObject);
            //    AdsManager.Instance.ShowBanner();
            //    if (AdsManager.Instance.HasInters)
            //    {
            //        AdsManager.Instance.ShowInterstitial(() =>
            //        {
            //            UISetupManager.Instance.maskBg.gameObject.SetActive(false);
            //        });
            //    }
            //    else
            //    {
            //        UISetupManager.Instance.maskBg.gameObject.SetActive(false);
            //        Destroy(gameObject);
            //    }
            //});
        }
    }
}