using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class CampingParkDrawingPicture : UIMinigame
    {
        [SerializeField] CampingParkBrush brush;
        [SerializeField] Transform limitLeft;
        [SerializeField] Transform limitRight;
        [SerializeField] Transform limitUp;
        [SerializeField] Transform limitDown;
        [SerializeField] Transform brushArea;
        [SerializeField] Transform[] eraseLimits;
        [SerializeField] DrawPictureTopic[] myTopicBtns;
        [SerializeField] Transform[] stickerLimits;
        [SerializeField] Transform myUI;
        [SerializeField] Canvas stickerAreaPb;
        [SerializeField] Transform spawnCanvasStickerArea;
        [SerializeField] Erase erase;
        [SerializeField] GameObject myPicture;
        [SerializeField] Image whiteImg;
        [SerializeField] Image testImg;

        Transform stickerArea;
        State currentState;
        int curOrder;
        private bool isExiting;
        private Tweener _tweenScale;
        private Texture2D screenCapture;

        public Action<Sprite> OnCapturing;

        private enum State
        {
            Drawing,
            Sticking,
            Erase
        }



        protected override void Start()
        {
            base.Start();

            brush.Setup(brushArea, 
                new CampingParkBrush.LimitArea(limitLeft.position, limitRight.position, limitUp.position, limitDown.position));

            erase.Setup(brushArea,
                new CampingParkBrush.LimitArea(eraseLimits[0].position, eraseLimits[1].position, eraseLimits[2].position, eraseLimits[3].position));

            brush.OnCreateBrush = OnCreateBrush;
            erase.OnBeginErase = OnBeginErase;
            foreach (var item in myTopicBtns)
            {
                item.OnClick += GetClickTopic;
            }

            myTopicBtns[0].OnPress();

            DrawingItem.OnBeginDragAct += OnBeginDragItem;
            DrawingItem.OnEndDragAct += OnEndDragItem;
            DrawingItem.OnClickAct += OnClickItem;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var item in myTopicBtns)
            {
                item.OnClick -= GetClickTopic;
            }
            DrawingItem.OnBeginDragAct -= OnBeginDragItem;
            DrawingItem.OnEndDragAct -= OnEndDragItem;
            DrawingItem.OnClickAct -= OnClickItem;

            if (_tweenScale != null) _tweenScale?.Kill();
        }
        protected override void OnBack()
        {
            if (isExiting) return;
            isExiting = true;

            OnCapture();
        }
        private void OnCapture()
        {
            myUI.gameObject.SetActive(false);
            myPicture.transform.localScale = Vector3.one * 1.3f;
            StartCoroutine(SaveScreenshotAndroid());
        }
        IEnumerator SaveScreenshotAndroid()
        {
            yield return new WaitForEndOfFrame();
            screenCapture = new Texture2D(Screen.width, Screen.height);
            screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenCapture.Apply();

            PreviewCapture(screenCapture);
        }
        private void PreviewCapture(Texture2D texture2D)
        {
            // Sound Take Picturee here
            SoundCampingParkManager.Instance.PlayOtherSfx(SoundTown<SoundCampingParkManager>.SFXType.Capture);

            var curSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 100);
            OnCapturing?.Invoke(curSprite);
            OnBackWithAnimation(BackAnimType.Scale, () =>
            {
                Destroy(this.gameObject);
            });
        }

        private void OnClickItem(DrawingItem obj)
        {
            var brushItem = obj.GetComponent<BrushColorItem>();
            if (brushItem != null)
            {
                brush.ChangeColor(brushItem.Color);
            }
            SoundCampingParkManager.Instance.PlayOtherSfx(SoundTown<SoundCampingParkManager>.SFXType.Select);
        }

        private void OnBeginErase()
        {
            if (currentState != State.Erase)
            {
                currentState = State.Erase;
                curOrder++;
                erase.ChangeOrder(curOrder);
            }
        }

        private void OnCreateBrush()
        {
            if(currentState != State.Drawing)
            {
                currentState = State.Drawing;
                curOrder++;
                brush.ChangeOrder(curOrder);
            }
        }

        private void OnBeginDragItem(DrawingItem obj)
        {
            obj.transform.SetParent(myUI);
            var canvas = Instantiate(stickerAreaPb, spawnCanvasStickerArea);
            canvas.sortingOrder = curOrder;
            stickerArea = canvas.transform;
            SoundCampingParkManager.Instance.PlayOtherSfx(SoundTown<SoundCampingParkManager>.SFXType.Select);
        }

        private void OnEndDragItem(DrawingItem obj)
        {
            var isInside = GameManager.instance.Is_inside(obj.transform.position, stickerLimits);
            if(isInside)
            {
                if (currentState != State.Sticking) curOrder++;
                currentState = State.Sticking;
                obj.Stick(stickerArea);
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Correct);
            }
            else
            {
                obj.Release();
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Incorrect);
            }

        }

        private void GetClickTopic(DrawPictureTopic obj)
        {
            foreach (var item in myTopicBtns)
            {
                if(item == obj)
                {
                    item.Show();
                }
                else
                {
                    item.Hide();
                }
            }
        }
    }
}