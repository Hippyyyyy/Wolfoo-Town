using _Base;
using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace _WolfooShoppingMall
{
    public class FilmMachine : BackItem
    {
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] Image lightingImg;
        [SerializeField] FilmMachineAnimation machineAnimation;
        [SerializeField] Button videoBannerBtn;
        [SerializeField] Button watchAdsBtn;
        [SerializeField] Image shutdownImg;
        [SerializeField] Image blackMaskImg;
        [SerializeField] Speaker speaker;
        [SerializeField] Button playBtn;

        private bool isTurnOn;
        private int curIdxClip;
        private int curIdxEpisode;
        private List<UnlockEpisode> localData;

        private Tweener scaleTween;
        private Tweener fillTween;
        private Tweener fadeTween;
        private Tween delayItemTween;
        private Tweener scaleItemTween;
        private Tweener fadeMaskTween;

        protected override void Start()
        {
            base.Start();
            videoBannerBtn.onClick.AddListener(OnOpenPanel);
            watchAdsBtn.onClick.AddListener(OnWatchAds);
            playBtn.onClick.AddListener(OnWatching);
            watchAdsBtn.transform.localScale = Vector3.zero;

            shutdownImg.DOFillAmount(0, 0);

            //   videoPlayer.clip = data.clipsData[0].episodeClips[0];

            scaleItemTween = playBtn.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 3).SetDelay(1).SetLoops(-1, LoopType.Restart);

        }
        protected override void InitData()
        {
            base.InitData();
            localData = DataSceneManager.Instance.LocalDataStorage.unlockEpisodes;

            videoPlayer.clip = data.filmData.clipsData[curIdxClip].episodeClips[curIdxEpisode];
            PauseVideo();
            SetStatusAudio();

            speaker.StopMusic();
        }

        protected override void InitItem()
        {
            isDance = true;
            canClick = true;
            isComparePos = true;
        }
        private void SetStatusAudio()
        {
            for (int i = 0; i < videoPlayer.length; i++)
            {
                videoPlayer.SetDirectAudioMute((ushort)i, BaseDataManager.Instance.playerMe.IsMuteMusic);
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            SetStatusAudio();

            EventDispatcher.Instance.RegisterListener<EventKey.OnSelect>(GetSelect);
            EventDispatcher.Instance.RegisterListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(GetWatchAds);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnSelect>(GetSelect);
            EventDispatcher.Instance.RemoveListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(GetWatchAds);

            if (delayItemTween != null) delayItemTween?.Kill();
            if (delayTween != null) delayTween?.Kill();
        }

        private void GetClickItem(EventKey.OnClickItem obj)
        {
            if (obj.videoPanel != null)
            {
                if (obj.idx < 0)
                {
                    //    ResumeVideo();

                }
                else
                    PlayVideo();
            }
        }

        private void OnOpenPanel()
        {
            PauseVideo();
            GUIManager.instance.OpenPanel(PanelType.VideoBanner);
        }

        private void GetSelect(EventKey.OnSelect obj)
        {
            if (obj.clipItem == null) return;
            StopVideo();
            curIdxClip = obj.idx;
            curIdxEpisode = obj.subIdx;
        }

        private void LoopLighting()
        {
            fadeTween = lightingImg.DOFade(.8f, 1)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    fadeTween = lightingImg.DOFade(0.3f, 1)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        LoopLighting();
                    });
                });
        }

        private void PlayVideo()
        {
            if (fadeTween != null) fadeTween?.Kill();
            if (delayItemTween != null) delayItemTween?.Kill();
            if (fillTween != null) fillTween?.Kill();

            if (scaleItemTween != null) scaleItemTween?.Kill();
            playBtn.gameObject.SetActive(false);

            isTurnOn = true;

            fillTween = shutdownImg.DOFillAmount(0, 0.5f);
            LoopLighting();
            machineAnimation.PlayExcute();

            speaker.PlayMusic();
            OnPlay();
        }

        void OnPlay()
        {
            videoPlayer.clip = data.filmData.clipsData[curIdxClip].episodeClips[curIdxEpisode];
            EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { filmMachine = this, idx = curIdxClip, subIdx = curIdxEpisode });

            if (localData[curIdxClip].unlockVideos[curIdxEpisode] || AdsManager.Instance.IsRemovedAds)
            {
                videoPlayer.Play();
                delayItemTween = DOVirtual.DelayedCall((float)videoPlayer.length, () =>
                {
                    OnNextClip();
                });
            }
            else
            {
                OnLockClip();
            }
        }

        private void StopVideo()
        {
            if (fadeTween != null) fadeTween?.Kill();
            if (delayItemTween != null) delayItemTween?.Kill();
            if (fillTween != null) fillTween?.Kill();
            if (scaleTween != null) scaleTween?.Kill();
            if (fadeMaskTween != null) fadeMaskTween?.Kill();
            blackMaskImg.DOFade(0, 0);

            curIdxClip = curIdxEpisode = 0;
            isTurnOn = false;

            scaleTween = watchAdsBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            fillTween = shutdownImg.DOFillAmount(1, 0.25f);
            fadeTween = lightingImg.DOFade(0, 0.5f).SetEase(Ease.Linear);
            machineAnimation.PlayIdle();
            videoPlayer.Stop();
            speaker.StopMusic();
        }
        private void PauseVideo()
        {
            speaker.StopMusic();
            videoPlayer.Pause();
        }
        private void ResumeVideo()
        {
            speaker.PlayMusic();
            videoPlayer.Play();
        }

        private void OnNextClip()
        {
            curIdxEpisode++;
            if (curIdxEpisode >= data.filmData.clipsData[curIdxClip].episodeClips.Length)
            {
                curIdxClip++;
                if (curIdxClip == data.filmData.clipsData.Length)
                {
                    curIdxClip = 0;
                }
                curIdxEpisode = 0;
            }
            if (curIdxEpisode == 0)
            {
                fadeMaskTween = blackMaskImg.DOFade(0.7f, 0.25f).SetLoops(-1, LoopType.Yoyo);
                delayItemTween = DOVirtual.DelayedCall(1, () =>
                {
                    fadeMaskTween?.Kill();
                    blackMaskImg.DOFade(0, 0);
                    OnPlay();
                });
            }
            else
            {
                fadeMaskTween = blackMaskImg.DOFade(0.7f, 0.5f).OnComplete(() =>
                {
                    fadeMaskTween = blackMaskImg.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        OnPlay();
                    });
                });
            }
        }

        private void OnLockClip()
        {
            SoundManager.instance.EnableSound(SoundType.Music);
            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = watchAdsBtn.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    scaleTween = watchAdsBtn.transform.DOPunchScale(Vector3.one * 0.1f, 1, 5).SetLoops(-1);
                });
        }

        private void OnWatchAds()
        {
            EventDispatcher.Instance.Dispatch(
                new EventKey.InitAdsPanel
                {
                    idxItem = curIdxClip,
                    idxSubItem = curIdxEpisode,
                    instanceID = GetInstanceID(),
                    filmMachine = this,
                    spriteItem = data.filmData.thumbnailSprites[curIdxClip * data.filmData.clipsData.Length + curIdxEpisode],
                    nameObj = name,
                    curPanel = "cinema_movie"
                }); ;
            GUIManager.instance.OpenPanel(PanelType.Ads);
        }

        private void GetWatchAds(EventKey.OnWatchAds obj)
        {
            if (obj.instanceID != GetInstanceID()) return;

            watchAdsBtn.interactable = false;
            DataSceneManager.Instance.UnlockVideo(obj.idxItem, obj.idxSubItem);

            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = watchAdsBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                watchAdsBtn.interactable = true;
            });

            PlayVideo();
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            OnWatching();
        }

        void OnWatching()
        {
            if (!canClick) return;
            canClick = false;
            isTurnOn = !isTurnOn;

            if (isTurnOn) PlayVideo();
            else StopVideo();

            DisableDance();

            DOVirtual.DelayedCall(1, () =>
            {
                canClick = true;
            });
        }
    }
}