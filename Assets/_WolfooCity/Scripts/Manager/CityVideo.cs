using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

namespace _WolfooCity
{
    public class CityVideo : MonoBehaviour
    {
        [SerializeField] CanvasWebViewPrefab _canvasWebViewPrefab;
        [SerializeField] Transform spawnArea;
        [SerializeField] List<string> videoIds;
        private int curVidIdx;
        private CanvasWebViewPrefab videoView;
        private bool isLoading;

        void Start()
        {
            CreateVideo();
            AdsManager.Instance.ShowBanner();
        }
        public void Close()
        {
            AdsManager.Instance.HideBanner();
            spawnArea.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
        public void PressNextVideo()
        {
            Debug.Log("Press Next Video 1...");
            if (isLoading) return;
            Debug.Log("Press Next Video 2...");
            curVidIdx++;
            Debug.Log("Press Next Video 3...");
            curVidIdx = curVidIdx >= videoIds.Count ? 0 : curVidIdx;
            Debug.Log("Press Next Video 4...");
            LoadVideo();
        }
        public void PressPreviousVideo()
        {
            if (isLoading) return;
            curVidIdx--;
            if (curVidIdx < 0)
            {
                curVidIdx = 0;
                return;
            }
            LoadVideo();
        }

        void LoadVideo()
        {
            //Load link t? DataUrl. Có th? set url cho DataUrl r?i load sau c?ng ???c.
            //      myVideo.WebView.LoadUrl(DataUrl.URL);
            isLoading = true;

            videoView.WebView.LoadUrl(videoIds[curVidIdx]);
            videoView.WebView.UrlChanged += (sender, eventArgs) =>
            {
                Debug.Log("URL changed: " + eventArgs.Url);
                //user click khi?n webview chuy?n link thì làm gì ?ó....
            };
            videoView.WebView.LoadFailed += (sender, eventArgs) =>
            {
                //Page load fail thì làm gì ?ó....
            };
            videoView.WebView.LoadProgressChanged += (sender, eventArgs) =>
            {
                if (eventArgs.Type == ProgressChangeType.Failed)
                {
                    //Page load fail thì làm gì ?ó...
                }
                if (eventArgs.Type == ProgressChangeType.Finished)
                {
                    isLoading = false;
                }
            };
        }

        async void CreateVideo()
        {
            isLoading = true;
            videoView = Instantiate(_canvasWebViewPrefab, spawnArea);
            await videoView.WaitUntilInitialized();
            LoadVideo();
        }
    }
}
