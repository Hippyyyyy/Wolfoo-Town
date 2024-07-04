using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CandyCar : BackItem
    {
        [SerializeField] Transform candyArea;
        [SerializeField] Animator _animator;
        [SerializeField] string idleName;
        [SerializeField] string playName;
        [SerializeField] Sprite[] candySprites;
        [SerializeField] Transform[] spawnAreas;
        [SerializeField] StickerBackItem stickerPb;
        [SerializeField] Transform plasticCupArea;
        private Tween _tween;
        private StickerBackItem makedSticker;

        protected override void InitData()
        {
            base.InitData();
            for (int i = 0; i < spawnAreas.Length; i++)
            {
                var sticker = Instantiate(stickerPb, spawnAreas[i]);
                sticker.Spawn();
                sticker.Setup(i, candySprites[i]);
            }
        }

        public void OnMakingCompleted()
        {
            SoundPlaygroundManager.Instance.PlayOtherSfx(SoundTown<SoundPlaygroundManager>.SFXType.MagicLighting);
            makedSticker.transform.SetParent(transform);
        }


        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.sticker != null)
            {
                if (Vector2.Distance(item.sticker.transform.position, candyArea.position) < 1)
                {
                    if (makedSticker != null) makedSticker.MoveToGround();
                        _tween?.Kill();
                    _tween = DOVirtual.DelayedCall(0.1f, () =>
                    {
                        makedSticker = item.sticker;
                        item.sticker.OnMakingCandy(candyArea);
                        _animator.Play(playName, 0, 0);
                    });
                }
                else
                {
                    var idx = item.sticker.SpawnIdx;
                    if (Vector2.Distance(item.sticker.transform.position, plasticCupArea.position) < 0.5f)
                    {
                        item.sticker.JumpToEndPos(spawnAreas[idx].position, spawnAreas[idx], null, assignPriorityy);
                    }
                    else
                    {
                        _tween?.Kill();
                        _tween = DOVirtual.DelayedCall(0.1f, () =>
                        {
                            var sticker = Instantiate(stickerPb, spawnAreas[idx]);
                            sticker.Spawn();
                            sticker.Setup(idx, candySprites[UnityEngine.Random.Range(0, candySprites.Length)]);
                        });
                    }
                }

            }
        }
    }
}