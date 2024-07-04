using DG.Tweening;
using SCN;
using SCN.Tutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class HandmadeCakeMode : MonoBehaviour
    {
        [SerializeField] Transform background;
        [SerializeField] Transform cakeZone;
        [SerializeField] VerticalScroll verticalScroll;
        [SerializeField] MakingCreamAnimation makingCreamAnimation;
        [SerializeField] ParticleSystem rainbowFx;
        [SerializeField] Button closeZoneBtn;
        [SerializeField] Button closeBtn;
        [SerializeField] IngameType ingameSoundType;
        [SerializeField] _WolfooCity.UIPanel uIPanel;
        private AudioClip startClip;

        private HandmadeCakeData data;
        private int curTopicIdx;
        private int curMainIdx;
        private HandMadeCake mainCake;
        private bool isColoring;
        private bool isNextItem = true;
        private Tween delayTween;
        private bool canClick;

        public HandMadeCake MainCake { get => mainCake; }

        private void Awake()
        {
            makingCreamAnimation.gameObject.SetActive(false);
        }

        private void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnDragItem>(GetDragItem);
            closeZoneBtn.onClick.AddListener(OnBack);
            closeBtn.onClick.AddListener(OnBack);

            //     AdsManager.Instance.HideBanner();
            if (SoundManager.instance.Music != null)
                startClip = SoundManager.instance.Music.clip;
            SoundManager.instance.PlayIngame(ingameSoundType);

            data = DataSceneManager.Instance.ItemDataSO.HandmadeCakeData;

            verticalScroll.Setup(data.creamSprites.Length, this);

            curMainIdx = UnityEngine.Random.Range(0, data.handMadeCakePbs.Length);
            mainCake = Instantiate(data.handMadeCakePbs[curMainIdx], cakeZone);

            int count = 0;
            foreach (var item in verticalScroll.MaskTrans.GetComponentsInChildren<MakingCreamScrollItem>())
            {
                item.AssignItem(count, data.creamSprites[count]);
                count++;
            }

            mainCake.GetHint(curTopicIdx);

            TutorialManager.Instance.StartPointer(verticalScroll.transform.position, mainCake.ItemImgs[curTopicIdx].transform.position, Gesture.Hold);

            delayTween = DOVirtual.DelayedCall(1, () =>
            {
                canClick = true;
            });
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnDragItem>(GetDragItem);
            if (delayTween != null) delayTween?.Kill();
            TutorialManager.Instance.Stop();
            SoundManager.instance.PlayIngame(startClip);
        }

        public void OnMaking()
        {

        }

        private void GetDragItem(EventKey.OnDragItem obj)
        {
            if (obj.makingCream != null)
            {
                if (obj.direction == Direction.Up)
                {
                    obj.makingCream.transform.localScale = Vector3.one;
                    isNextItem = true;
                    return;
                }

                if (isColoring || !isNextItem) return;
                if (Vector2.Distance(obj.makingCream.transform.position, mainCake.ItemImgs[curTopicIdx].transform.position) > 3) return;

                TutorialManager.Instance.Stop();
                isColoring = true;
                isNextItem = false;
                obj.makingCream.transform.localScale = Vector3.zero;
                mainCake.OnColoring(curTopicIdx);
                // Sound Scratch Here
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Scratch);

                makingCreamAnimation.ChangeSkin(obj.makingCream.Order);
                makingCreamAnimation.transform.position = mainCake.ItemImgs[curTopicIdx].transform.position;
                makingCreamAnimation.gameObject.SetActive(true);
                makingCreamAnimation.PlayExcute();

                mainCake.ItemImgs[curTopicIdx].transform.localScale = Vector3.zero;
                mainCake.ItemImgs[curTopicIdx].sprite = data.components[curMainIdx].items[curTopicIdx].sprites[obj.makingCream.Id];
                mainCake.ItemImgs[curTopicIdx].SetNativeSize();
                mainCake.ItemImgs[curTopicIdx].transform.DOScale(1, 1)
                .OnComplete(() =>
                {
                    curTopicIdx++;
                    makingCreamAnimation.gameObject.SetActive(false);
                    makingCreamAnimation.PlayIdle();

                    if (curTopicIdx == mainCake.ItemImgs.Length)
                    {
                        canClick = false;
                        rainbowFx.Play();
                        // Sound Lighting Here
                        // Sound Wolfoo Wow Here
                        SoundManager.instance.PlayOtherSfx(SfxOtherType.Lighting);
                        SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Wow);

                        delayTween = DOVirtual.DelayedCall(2, () =>
                        {
                            uIPanel.Hide(() =>
                            {
                                EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { handmadeCake = this });
                                Destroy(gameObject);
                            });
                        });
                    }
                    else
                    {
                        isColoring = false;
                        mainCake.GetHint(curTopicIdx);
                        TutorialManager.Instance.StartPointer(
                            verticalScroll.transform.position,
                            mainCake.ItemImgs[curTopicIdx].transform.position,
                            Gesture.Hold);
                    }
                });
            }
        }

        private void OnBack()
        {
            if (!canClick) return;

            canClick = false;
            uIPanel.Hide(() =>
            {
                Destroy(gameObject);
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}