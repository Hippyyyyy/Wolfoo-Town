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
    public class DollClothingMode : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Button[] topicBtns;
        [SerializeField] VerticalScroll[] verticalScrolls;
        [SerializeField] Image headImg;
        [SerializeField] Image dressImg;
        [SerializeField] Image accessoryImg;
        [SerializeField] ParticleSystem[] completeFx;
        [SerializeField] Transform characterZone;
        [SerializeField] Image backgroundImg;
        [SerializeField] Button backBtn;
        [SerializeField] Button backBtn2;
        [SerializeField] IngameType ingameSoundType;
        [SerializeField] _WolfooCity.UIPanel uIPanel;
        private AudioClip startClip;

        private DollClothingData data;
        private Vector3 startCharacterScale;
        private Tween delayTween;
        private int curIdx = -1;
        private Tweener rotateTween;
        private Tweener scaleTween;
        private bool canClick;
        private int countSuccess;

        private void Awake()
        {
            backgroundImg.transform.localScale = Vector3.zero;
        }
        private void Start()
        {
     //       AdsManager.Instance.HideBanner();

            startClip = SoundManager.instance.Music.clip;
            SoundManager.instance.PlayIngame(ingameSoundType);

            data = DataSceneManager.Instance.ItemDataSO.DollClothingData;

            startCharacterScale = characterZone.localScale;

            InitEvent();
            InitData();

            OnClickTopic(0);

            scaleTween = backgroundImg.transform.DOScale(Vector3.one * 0.75f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canClick = true;
            });
        }

        private void OnDestroy()
        {
            if (delayTween != null) delayTween?.Kill();
            EventDispatcher.Instance.RemoveListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragItem>(GetEndDragItem);
            SoundManager.instance.PlayIngame(startClip);
        }

        private void InitEvent()
        {
            topicBtns[0].onClick.AddListener(() => OnClickTopic(0));
            topicBtns[1].onClick.AddListener(() => OnClickTopic(1));
            topicBtns[2].onClick.AddListener(() => OnClickTopic(2));

            backBtn.onClick.AddListener(OnBack);
            backBtn2.onClick.AddListener(OnBack);

            EventDispatcher.Instance.RegisterListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }

        private void GetEndDragItem(EventKey.OnEndDragItem obj)
        {
            if (obj.dollClothingItem != null)
            {

            }
        }

        private void GetClickItem(EventKey.OnClickItem obj)
        {
            if (obj.dollClothingItem != null)
            {
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);
                countSuccess++;
                if (countSuccess == 3)
                {
                    SoundCharacterManager.Instance.PlayWolfooInteresting();
                    countSuccess = 0;
                }

                var rd = UnityEngine.Random.Range(0, completeFx.Length);

                data.dollClothingDicts[curIdx].curTopicIdx = curIdx;
                data.dollClothingDicts[curIdx].curItemIdx = obj.dollClothingItem.Id;

                switch (obj.dollClothingItem.TopicIdx)
                {
                    case 0: // Dress
                        dressImg.sprite = data.dressTopicData[obj.dollClothingItem.Id];
                        dressImg.SetNativeSize();
                        dressImg.transform.localPosition = data.dressPosData[obj.dollClothingItem.Id];

                        completeFx[rd].transform.position = headImg.transform.position;
                        completeFx[rd].Play();

                        if (scaleTween != null)
                        {
                            scaleTween?.Kill();
                            characterZone.localScale = startCharacterScale;
                        }
                        scaleTween = characterZone.DOPunchScale(Vector3.one * 0.1f, 0.5f, 1);

                        break;
                    case 2: // Hair
                        headImg.sprite = data.eyeHairTopicData[obj.dollClothingItem.Id];
                        headImg.SetNativeSize();
                        headImg.transform.localPosition = data.hairPosData[obj.dollClothingItem.Id];

                        completeFx[rd].transform.position = headImg.transform.position + Vector3.up * 1.5f;
                        completeFx[rd].Play();

                        if (rotateTween != null) rotateTween?.Kill();
                        headImg.transform.rotation = Quaternion.Euler(Vector3.zero);
                        rotateTween = headImg.transform.DOPunchRotation(Vector3.forward * 10, 0.35f, 1).OnComplete(() =>
                        {
                            rotateTween = headImg.transform.DOPunchRotation(Vector3.forward * -10, 0.35f, 1).OnComplete(() =>
                            {
                                rotateTween = headImg.transform.DOPunchRotation(Vector3.zero, 0.3f, 1)
                                .SetEase(Ease.OutBounce)
                                .OnComplete(() =>
                                {

                                });
                            });
                        });
                        break;
                    case 1: // Accessory
                        if (rotateTween != null) rotateTween?.Kill();
                        headImg.transform.rotation = Quaternion.Euler(Vector3.zero);

                        accessoryImg.sprite = data.accessoryTopicData[obj.dollClothingItem.Id];
                        accessoryImg.SetNativeSize();
                        accessoryImg.transform.localPosition = data.accessoryPosData[obj.dollClothingItem.Id] + Vector3.up * 244;

                        completeFx[rd].transform.position = accessoryImg.transform.position;
                        completeFx[rd].Play();

                        rotateTween = headImg.transform.DOPunchRotation(Vector3.forward * 10, 0.35f, 1).OnComplete(() =>
                        {
                            rotateTween = headImg.transform.DOPunchRotation(Vector3.forward * -10, 0.35f, 1).OnComplete(() =>
                            {
                                rotateTween = headImg.transform.DOPunchRotation(Vector3.zero, 0.3f, 1)
                                .SetEase(Ease.OutBounce)
                                .OnComplete(() =>
                                {

                                });
                            });
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
                    topicBtns[i].image.sprite = data.topicBtnSprites2[i];
                    verticalScrolls[i].gameObject.SetActive(true);
                }
                else
                {
                    topicBtns[i].image.sprite = data.topicBtnSprites[i];
                    verticalScrolls[i].gameObject.SetActive(false);
                }
            }
        }

        private void InitData()
        {
            for (int i = 0; i < verticalScrolls.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        verticalScrolls[i].Setup(data.dressTopicData.Length, this);
                        break;
                    case 1:
                        verticalScrolls[i].Setup(data.accessoryTopicData.Length, this);
                        break;
                    case 2:
                        verticalScrolls[i].Setup(data.hairTopicData.Length, this);
                        break;
                }

                topicBtns[i].image.sprite = data.topicBtnSprites[i];
                topicBtns[i].image.SetNativeSize();
            }


            delayTween = DOVirtual.DelayedCall(0.25f, () =>
            {
                for (int i = 0; i < verticalScrolls.Length; i++)
                {
                    int count = 0;
                    foreach (var item in verticalScrolls[i].MaskTrans.GetComponentsInChildren<DollClothingScrollItem>())
                    {
                        switch (i)
                        {
                            case 0:
                                item.AssignItem(i, count, data.dressTopicData[count]);
                                break;
                            case 1:
                                item.AssignItem(i, count, data.accessoryTopicData[count]);
                                break;
                            case 2:
                                item.AssignItem(i, count, data.hairTopicData[count]);
                                break;
                        }
                        count++;
                    }
                }
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
        void OnBack()
        {
            if (!canClick) return;
            canClick = false;

            UISetupManager.Instance.maskBg.gameObject.SetActive(true);
            uIPanel.Hide(() =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { dollClothing = this });

                UISetupManager.Instance.maskBg.gameObject.SetActive(false);
                Destroy(gameObject);
            });
        }
    }
}