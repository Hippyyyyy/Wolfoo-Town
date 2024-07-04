using DG.Tweening;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCN.Tutorial
{
    public class FocusController : MonoBehaviour
    {
        [SerializeField] RectTransform center;
        [SerializeField] RectTransform bottomLeft;
        [SerializeField] RectTransform bottomRight;
        [SerializeField] RectTransform topRight;
        [SerializeField] RectTransform topLeft;
        [SerializeField] RectTransform left;
        [SerializeField] RectTransform right;
        [SerializeField] RectTransform top;
        [SerializeField] RectTransform bottom;

        [Space(10)]
        [SerializeField] GameObject blockPanel;
		[SerializeField] CanvasGroup canvasGroup;

        [SerializeField] RectTransform parentCanvas;

		const float animTime = 0.25f;

        public void Execute(RectTransform target, float alpha, bool hasAnim = true
            , System.Action onComplete = null, int expand = 50)
        {
            blockPanel.SetActive(true);
            SetPosition(target.position);

            canvasGroup.DOKill();
            center.DOKill();

            if (hasAnim)
            {
                DOTweenManager.Instance.TweenChangeAlphaCanvasGroup(canvasGroup, 0, alpha, animTime);
                DOTweenManager.Instance.TweenSizeDelta(center, target.rect.size + new Vector2(200, 200)
                , target.rect.size + Vector2.one * expand, animTime).OnUpdate(UpdateOutSize)
                .OnComplete(() =>
                {
                    blockPanel.SetActive(false);
                    onComplete?.Invoke();
                });
            }
            else
            {
                canvasGroup.alpha = alpha;

                SetSize(target.rect.size + Vector2.one * expand);
                UpdateOutSize();

                blockPanel.SetActive(false);
                onComplete?.Invoke();
            }
        }

        public void UnFocus(bool hasAnim = true, System.Action onComplete = null)
        {
            if (hasAnim)
            {
                blockPanel.SetActive(true);
                canvasGroup.DOKill();
                DOTweenManager.Instance.TweenChangeAlphaCanvasGroup(canvasGroup, 0, animTime)
                    .OnComplete(() =>
                    {
                        blockPanel.SetActive(false);
                        onComplete?.Invoke();
                    });
            }
            else
            {
                canvasGroup.DOKill();
                canvasGroup.alpha = 0;

                blockPanel.SetActive(false);
                onComplete?.Invoke();
            }
		}

        void SetPosition(Vector3 worldPos)
        {
            center.position = new Vector3(worldPos.x, worldPos.y, 0);
        }

        void SetSize(Vector2 size)
        {
            center.sizeDelta = size;
        }

        void UpdateOutSize()
        {
            var halfWidth = parentCanvas.sizeDelta.x / 2;
            var halfHeight = parentCanvas.sizeDelta.y / 2;

            var lengthLeft = halfWidth + center.anchoredPosition.x - center.sizeDelta.x / 2;
            var lengthBot = halfHeight + center.anchoredPosition.y - center.sizeDelta.y / 2;
            var lengthRight = halfWidth - center.anchoredPosition.x - center.sizeDelta.x / 2;
            var lengthTop = halfHeight - center.anchoredPosition.y - center.sizeDelta.y / 2;

            lengthLeft = lengthLeft > 0 ? lengthLeft : 0;
            lengthBot = lengthBot > 0 ? lengthBot : 0;
            lengthRight = lengthRight > 0 ? lengthRight : 0;
            lengthTop = lengthTop > 0 ? lengthTop : 0;

            bottomLeft.sizeDelta = new Vector2(lengthLeft, lengthBot);
            bottomRight.sizeDelta = new Vector2(lengthRight, lengthBot);
            topLeft.sizeDelta = new Vector2(lengthLeft, lengthTop);
            topRight.sizeDelta = new Vector2(lengthRight, lengthTop);

            left.sizeDelta = new Vector2(lengthLeft, center.sizeDelta.y);
            right.sizeDelta = new Vector2(lengthRight, center.sizeDelta.y);
            top.sizeDelta = new Vector2(center.sizeDelta.x, lengthTop);
            bottom.sizeDelta = new Vector2(center.sizeDelta.x, lengthBot);
        }
    }
}