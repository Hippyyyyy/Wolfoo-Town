using DG.Tweening;
using SCN;
using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CaptureMode : MonoBehaviour
    {
        [SerializeField] Button[] topicBtns;
        [SerializeField] CaptureCharacter[] captureCharacters;
        [SerializeField] VerticalScroll[] verticalScrolls;
        [SerializeField] ParticleSystem[] completeFx;
        [SerializeField] Transform characterZone;
        [SerializeField] RectTransform captureZone;
        [SerializeField] Transform stickerZone;
        [SerializeField] Transform stickerLimitZone;
        [SerializeField] Image frameImg;
        [SerializeField] Image backgroundImg;
        [SerializeField] Button captureBtn;
        [SerializeField] Image picturePreview;
        [SerializeField] Image captureImg;
        [SerializeField] Transform maskZone;
        [SerializeField] Button backZoneBtn;
        [SerializeField] Button backBtn;
        [SerializeField] IngameType ingameSoundType;
        [SerializeField] _WolfooCity.UIPanel uIPanel;

        private List<int> curCharacterIdx = new List<int>();
        private List<int> curCharacterState = new List<int>();
        private CaptureModeData data;
        private Vector3 startCharacterScale;
        private Tween delayTween;
        private int curIdx = -1;
        private Tweener moveTween;
        private Tweener scaleTween;
        private bool canClick;
        private int nextIdx;
        private Vector3 startFrameScale;
        private Transform startParentCharacterZone;
        private Vector3 startPosCharacterZone;
        private Vector3 startScaleCharacterZone;
        private Vector2 startSizeCapture;
        private Tweener punchTween;
        private Texture2D screenCapture;
        private List<Sprite> itemSprites = new List<Sprite>();
        private Image coverImg;
        private Tweener colorTween;
        private AudioClip startClip;

        private void Awake()
        {
            startFrameScale = frameImg.transform.localScale;
            startParentCharacterZone = captureZone.transform.parent;
            startPosCharacterZone = captureZone.transform.position;
            startScaleCharacterZone = captureZone.transform.localScale;
            startSizeCapture = captureZone.sizeDelta;
            backgroundImg.transform.localScale = Vector3.zero;

            coverImg = GetComponent<Image>();
        }
        private void Start()
        {
            data = DataSceneManager.Instance.ItemDataSO.CaptureModeData;

            //   AdsManager.Instance.HideBanner();

            if (SoundManager.instance.Music != null)
                startClip = SoundManager.instance.Music.clip;
            SoundManager.instance.PlayIngame(ingameSoundType);
            //  startCharacterScale = characterZone.localScale;

            InitEvent();
            InitData();

            for (int i = 0; i < captureCharacters.Length; i++)
            {
                curCharacterIdx.Add(0);
                curCharacterState.Add(0);
            }

            scaleTween = backgroundImg.transform.DOScale(Vector3.one * 0.65f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                OnClickTopic(0);
                canClick = true;

                foreach (var item in captureCharacters)
                {
                    item.InitItem();
                }
            });
        }

        private void OnDestroy()
        {
            if (delayTween != null) delayTween?.Kill();
            if (scaleTween != null) scaleTween?.Kill();
            if (moveTween != null) moveTween?.Kill();
            EventDispatcher.Instance.RemoveListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragItem>(GetEndDragItem);

            SoundManager.instance.PlayIngame(startClip);
        }

        private void InitEvent()
        {
            captureBtn.onClick.AddListener(OnCapture);
            backZoneBtn.onClick.AddListener(OnBack);
            backBtn.onClick.AddListener(OnBack);

            topicBtns[0].onClick.AddListener(() => OnClickTopic(0));
            topicBtns[1].onClick.AddListener(() => OnClickTopic(1));
            topicBtns[2].onClick.AddListener(() => OnClickTopic(2));

            EventDispatcher.Instance.RegisterListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }

        private void OnCapture()
        {
            if (!canClick) return;

            canClick = false;
            maskZone.gameObject.SetActive(false);
            backBtn.gameObject.SetActive(false);

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Capture);

            colorTween = coverImg.DOColor(Color.white, 0.4f);
            captureZone.transform.SetParent(transform);
            moveTween = captureZone.DOMove(Vector2.zero, 0.5f);
            scaleTween = captureZone.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                backgroundImg.gameObject.SetActive(false);
                //captureZone.anchorMax = Vector2.one;
                //captureZone.anchorMin = Vector2.zero;
                //captureZone.localPosition = Vector3.zero;
                //captureZone.sizeDelta = Vector3.zero;

                StartCoroutine(SaveScreenshotAndroid());
            });
        }
        IEnumerator SaveScreenshotAndroid()
        {
            yield return new WaitForEndOfFrame();
            captureBtn.gameObject.SetActive(false);

            screenCapture = new Texture2D(Screen.width, Screen.height);
            screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenCapture.Apply();
            //  byte[] imageBytes = screenCapture.EncodeToPNG();
            PreviewCapture(screenCapture);
            // EventDispatcher.Instance.Dispatch(new EventParam.PreviewPicture { image = screenCapture });

            captureBtn.gameObject.SetActive(true);
            coverImg.color = Color.black;
            coverImg.DOFade(0.7f, 0);
        }
        private void PreviewCapture(Texture2D texture2D)
        {
            //   SoundManager.Instance.PlaySoundFX(SFXType.TakePicture);
            //    captureContent.SetActive(true);

            //    picturePreview.gameObject.SetActive(false);

            // Sound Take Picturee here

            var curSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 100);
            itemSprites.Add(curSprite);

            var picture = Instantiate(picturePreview, picturePreview.transform.parent);
            picture.sprite = curSprite;
            picture.SetNativeSize();

            picture.transform.localPosition = Vector3.zero;
            picture.transform.localScale = Vector3.one;
            picture.transform.localEulerAngles = Vector3.zero;

            scaleTween = picture.transform.DOScale(Vector3.one * .65f, .75f)
            .SetEase(Ease.InBack)
            .OnStart(() =>
            {
                //  picture.transform.DORotate(new Vector3(0, 0, 3f), 0.75f);
            }).OnComplete(() =>
            {
                //captureZone.anchorMax = Vector2.one * 0.5f;
                //captureZone.anchorMin = Vector2.one * 0.5f;
                //captureZone.sizeDelta = startSizeCapture;
                captureZone.transform.SetParent(startParentCharacterZone);
                captureZone.transform.SetAsFirstSibling();
                captureZone.transform.position = startPosCharacterZone;
                captureZone.transform.localScale = startScaleCharacterZone;


                maskZone.gameObject.SetActive(true);
                backgroundImg.gameObject.SetActive(true);
                backBtn.gameObject.SetActive(true);

                delayTween = DOVirtual.DelayedCall(1f, () =>
                {
                    picture.gameObject.SetActive(false);
                    //     captureContent.SetActive(false);
                    canClick = true;
                });
            });
        }

        private void GetEndDragItem(EventKey.OnEndDragItem obj)
        {
            if (obj.captureItem != null)
            {
                switch (obj.captureItem.TopicIdx)
                {
                    case 2:
                        if (obj.captureItem.transform.position.x < stickerLimitZone.GetChild(0).position.x) return;
                        if (obj.captureItem.transform.position.x > stickerLimitZone.GetChild(3).position.x) return;
                        if (obj.captureItem.transform.position.y < stickerLimitZone.GetChild(1).position.y) return;
                        if (obj.captureItem.transform.position.y > stickerLimitZone.GetChild(0).position.y) return;

                        obj.captureItem.OnAttach(obj.captureItem.transform.position, stickerZone);
                        break;
                }
            }

            if (obj.sticker != null)
            {
                if (obj.sticker.transform.position.x < stickerLimitZone.GetChild(0).position.x ||
                    obj.sticker.transform.position.x > stickerLimitZone.GetChild(3).position.x ||
                    obj.sticker.transform.position.y < stickerLimitZone.GetChild(1).position.y ||
                    obj.sticker.transform.position.y > stickerLimitZone.GetChild(0).position.y)
                {
                    obj.sticker.OnDefuse();
                    return;
                }
            }
        }

        private void GetClickItem(EventKey.OnClickItem obj)
        {
            if (obj.captureItem != null)
            {
                var rdParticleIdx = UnityEngine.Random.Range(0, completeFx.Length);

                //        data.dollClothingDicts[curIdx].curTopicIdx = curIdx;
                //    data.dollClothingDicts[curIdx].curItemIdx = obj.dollClothingItem.Id;

                switch (obj.captureItem.TopicIdx)
                {
                    case 0: // Character
                            //  captureCharacters[nextIdx].image.sprite = data.characterTopicData[curCharacterState[nextIdx]].toySprites[obj.captureItem.Id];
                        captureCharacters[nextIdx].AssignItem(obj.captureItem.Id, data.characterTopicData[obj.captureItem.Id].toySprites);
                        captureCharacters[nextIdx].OnSpawn();

                        completeFx[1].transform.position = captureCharacters[nextIdx].transform.position;
                        completeFx[1].Play();

                        nextIdx++;
                        if (nextIdx >= captureCharacters.Length) nextIdx = 0;
                        break;
                    case 2: // Sticker
                        break;
                    case 1: // Frame
                        frameImg.enabled = true;
                        frameImg.sprite = data.frameTopicData[obj.captureItem.Id];
                        frameImg.SetNativeSize();

                        if (punchTween != null) punchTween?.Kill();
                        frameImg.transform.localScale = startFrameScale;
                        punchTween = frameImg.transform.DOPunchScale(Vector3.one * -0.1f, 0.5f, 4, 0.5f).SetEase(Ease.OutBounce);


                        break;
                }
            }
        }

        private void OnClickCharacter(int idx)
        {

        }

        private void OnClickTopic(int idx)
        {
            // Sound Click here
            if (curIdx == idx) return;
            curIdx = idx;

            for (int i = 0; i < topicBtns.Length; i++)
            {
                if (i == idx)
                {
                    topicBtns[i].image.sprite = data.topicBtnSprites2[i];
                    verticalScrolls[i].gameObject.SetActive(true);
                }
                else
                {
                    if (i >= verticalScrolls.Length) return;
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
                        verticalScrolls[i].Setup(data.characterAvaTopicData.Length, this);
                        break;
                    case 1:
                        verticalScrolls[i].Setup(data.frameTopicData.Length, this);
                        break;
                    case 2:
                        verticalScrolls[i].Setup(data.stickerTopicData.Length, this);
                        break;
                }

                topicBtns[i].image.sprite = data.topicBtnSprites[i];
                topicBtns[i].image.SetNativeSize();
            }


            delayTween = DOVirtual.DelayedCall(0.15f, () =>
            {
                for (int i = 0; i < verticalScrolls.Length; i++)
                {
                    int count = 0;
                    foreach (var item in verticalScrolls[i].MaskTrans.GetComponentsInChildren<CaptureComponentScrollItem>())
                    {
                        switch (i)
                        {
                            case 0:
                                item.AssignItem(i, count, data.characterAvaTopicData[count]);
                                break;
                            case 1:
                                item.AssignItem(i, count, data.frameTopicData[count], true);
                                break;
                            case 2:
                                item.AssignItem(i, count, data.stickerTopicData[count], true);
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

            UISetupManager.Instance.maskBg.gameObject.SetActive(true);
            uIPanel.Hide(() =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { captureMode = this, sprites = itemSprites });
                Destroy(gameObject);
                UISetupManager.Instance.maskBg.gameObject.SetActive(false);
            });
        }
    }
}