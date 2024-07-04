using _Base;
using DG.Tweening;
using SCN;
using SCN.IAP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace _WolfooShoppingMall
{
    public class VideoBannerPanel : Panel, IPointerClickHandler
    {
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] Transform clipItemZone;
        [SerializeField] ClipItem clipItemPb;
        [SerializeField] Button playBtn;
        [SerializeField] Button playBtn2;
        [SerializeField] Button exitBtn;

        private FilmData data;
        private List<UnlockEpisode> localData;
        private List<ClipItem> curClips = new List<ClipItem>();
        private Tween delayTween;
        private Tweener scaleTween;
        private Tweener scaleTween2;
        private bool isStart = true;
        private int curIdx;
        private int curSubIdx;

        protected override void Start()
        {
            playBtn.transform.localScale = Vector3.zero;
            playBtn2.transform.localScale = Vector3.zero;

            base.Start();
            playBtn.onClick.AddListener(() =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { videoPanel = this, idx = 1 });
                base.Hide();
            });
            playBtn2.onClick.AddListener(() =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { videoPanel = this, idx = 1 });
                base.Hide();
            });
            exitBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.EnableSound(SoundType.Music);

                Debug.Log("UI Panel: " + uiPanel);
                uiPanel.Hide(() =>
                {
                    base.Hide();
                    EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { videoPanel = this, idx = -1 });
                });
            });

            data = DataSceneManager.Instance.BackItemDataSO.filmData;

            localData = DataSceneManager.Instance.LocalDataStorage.unlockEpisodes;

            for (int i = 0; i < data.clipsData.Length; i++)
            {
                for (int j = 0; j < data.clipsData[i].episodeClips.Length; j++)
                {
                    var clip = Instantiate(clipItemPb, clipItemZone);
                    clip.AssignItem(i, j, data.thumbnailSprites[i * data.clipsData.Length + j], !localData[i].unlockVideos[j]);
                    curClips.Add(clip);

                    if (AdsManager.Instance.IsRemovedAds) clip.OnUnlock();

                    if (j == 0 && i == 0)
                    {
                        EventDispatcher.Instance.Dispatch(
                            new EventKey.OnSelect { idx = i, subIdx = j, clipItem = clip });
                        OnChoose(i, j);
                    }
                }
            }

            IAPManager.Instance.OnBuyDone = GetBuyDone;

            SetStatusAudio();

            //for (int i = 0; i < data.thumbnailSprites.Length; i++)
            //{
            //    var clip = Instantiate(clipItemPb, clipItemZone);
            //    clip.AssignItem(i, data.thumbnailSprites[i]);
            //}
        }

        private void SetStatusAudio()
        {
            for (int i = 0; i < videoPlayer.length; i++)
            {
                videoPlayer.SetDirectAudioMute((ushort)i, BaseDataManager.Instance.IsMuteMusic);
                Debug.Log("SetDirectAudioMute: " + BaseDataManager.Instance.IsMuteMusic);
            }
        }

        private void GetBuyDone()
        {
            for (int i = 0; i < curClips.Count; i++)
            {
                curClips[i].OnUnlock();
            }
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnSelect>(GetSelect);
            SetStatusAudio();

            if (!isStart)
            {
                for (int i = 0; i < data.clipsData.Length; i++)
                {
                    for (int j = 0; j < data.clipsData[i].episodeClips.Length; j++)
                    {
                        if (localData[i].unlockVideos[j] || AdsManager.Instance.IsRemovedAds)
                        {
                            curClips[i * data.clipsData.Length + j].OnUnlock();
                        }
                        else
                        {
                            curClips[i * data.clipsData.Length + j].OnLock();
                        }
                    }
                }
                //    OnChoose();
            }
            isStart = false;
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnSelect>(GetSelect);

            if (delayTween != null) delayTween?.Kill();
            if (scaleTween != null) scaleTween?.Kill();
            playBtn.transform.localScale = Vector3.zero;
            //    SoundManager.instance.EnableSound(SoundType.Music);
        }

        private void GetSelect(EventKey.OnSelect obj)
        {
            if (obj.clipItem != null)
            {
                OnChoose(obj.idx, obj.subIdx);

                if (scaleTween != null) scaleTween?.Kill();
                scaleTween = playBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack);

                if (scaleTween2 != null) scaleTween2?.Kill();
                scaleTween2 = playBtn2.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        scaleTween2 = playBtn2.transform.DOPunchScale(Vector3.one * 0.1f, 1, 3).SetLoops(-1, LoopType.Restart);
                    });
            }

            if (obj.filmMachine)
            {
                curIdx = obj.idx;
                curSubIdx = obj.subIdx;
            }
        }

        void OnChoose()
        {
            videoPlayer.clip = data.clipsData[curIdx].episodeClips[curSubIdx];
            //  videoPlayer.Play();
        }

        private void OnChoose(int idx, int subIdx)
        {
            if (!SoundManager.instance.IsMuted)
                SoundManager.instance.MuteSound(SoundType.Music);

            if (videoPlayer.isPlaying) videoPlayer.Stop();
            if (scaleTween != null) scaleTween?.Kill();
            playBtn.transform.localScale = Vector3.zero;

            videoPlayer.clip = data.clipsData[idx].episodeClips[subIdx];
            videoPlayer.Play();

            if (delayTween != null) delayTween?.Kill();
            delayTween = DOVirtual.DelayedCall((float)videoPlayer.length, () =>
            {
                if (scaleTween2 != null) scaleTween2?.Kill();
                scaleTween2 = playBtn2.transform.DOScale(Vector3.zero, 0.5f);

                scaleTween = playBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}