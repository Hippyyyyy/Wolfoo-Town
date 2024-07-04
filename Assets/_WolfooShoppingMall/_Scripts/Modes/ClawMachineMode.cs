using DG.Tweening;
using SCN;
using SCN.Tutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ClawMachineMode : MonoBehaviour
    {
        [SerializeField] ClawControl clawControl;
        [SerializeField] ClawRopeMachine clawRope;
        [SerializeField] SpriteRenderer grabRenderer;
        [SerializeField] Sprite[] grabSprites;
        [SerializeField] Transform[] limitClawRopeZones;
        [SerializeField] Transform boxZone;
        [SerializeField] LightingMachine[] lightings;
        [SerializeField] Transform failZone;
        [Range(0, 101)]
        [SerializeField] int failRate;
        [SerializeField] Transform itemZone;
        [SerializeField] ClawMachineToy[] toys;
        [SerializeField] Transform coinZone;
        [SerializeField] Transform coin;
        [SerializeField] Button backBtn;
        [SerializeField] IngameType ingameSoundType;
        [SerializeField] _WolfooCity.UIPanel uIPanel;
        private AudioClip startClip;

        private bool isPicking;
        private Sequence jumpTween;
        private Tweener scaleTween;
        private ClawMachineToy curToy;
        private Tween delayTweenItem;
        private Tween delayTween2;
        private Tween delayTween;
        private bool isGrabFail;
        private MachineToyData data;
        private int toyClaimedCount;
        private Vector3 startScale;
        private bool canClick = true;
        private Tweener shakeTween;

        private void Awake()
        {
            if (Camera.main.aspect >= 1.7)
            {

            }
            else
            {
                transform.localScale = Vector3.one * 9;
            }
        }
        private void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnCollision>(GetCollision);
            EventDispatcher.Instance.RegisterListener<EventKey.OnDragItem>(GetDragItem);
            _WolfooCity.UIPanel.OnPanelShow += GetPanelShow;

            data = DataSceneManager.Instance.ItemDataSO.MachineToyData;

            //         AdsManager.Instance.HideBanner();
            UISetupManager.Instance.maskBg.gameObject.SetActive(true);

            isPicking = true;
            backBtn.onClick.AddListener(OnTurnOff);

            if (SoundManager.instance.Music != null)
                startClip = SoundManager.instance.Music.clip;
            SoundManager.instance.PlayIngame(ingameSoundType);
        }
        private void OnDestroy()
        {
            //  TutorialManager.Instance.Stop();
            if (delayTween != null) delayTween?.Kill();
            if (delayTween2 != null) delayTween2?.Kill();
            if (delayTweenItem != null) delayTweenItem?.Kill();
            if (scaleTween != null) scaleTween?.Kill();
            if (shakeTween != null) shakeTween?.Kill();
            if (jumpTween != null) jumpTween?.Kill();

            EventDispatcher.Instance.RemoveListener<EventKey.OnClickItem>(GetClickItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnCollision>(GetCollision);
            EventDispatcher.Instance.RemoveListener<EventKey.OnDragItem>(GetDragItem);
            _WolfooCity.UIPanel.OnPanelShow -= GetPanelShow;

            UISetupManager.Instance.maskBg.gameObject.SetActive(false);
            SoundManager.instance.PlayIngame(startClip);
        }

        private void GetPanelShow()
        {
            jumpTween = coin.DOJump(coinZone.position + new Vector3(0, -2, 0), 5, 1, 2).OnComplete(() =>
            {
                shakeTween = transform.DOShakePosition(0.5f, 0.5f, 5, 1).OnComplete(() =>
                {
                    for (int i = 0; i < toys.Length; i++)
                    {
                        toys[i].AssignItem(i);
                        toys[i].AssignRigidbody();
                    }

                    delayTween = DOVirtual.DelayedCall(2, () =>
                    {
                        isPicking = false;
                    });
                });

            });
        }

        private void GetDragItem(EventKey.OnDragItem obj)
        {
            if (obj.clawControl != null)
            {
                if (isPicking) return;
                SoundManager.instance.PlayOtherSfx(SfxOtherType.ControlMoving);

                if (obj.direction == Direction.Right)
                {
                    if (clawRope.transform.position.x >= limitClawRopeZones[1].position.x) return;
                    clawRope.transform.position += Vector3.right * clawRope.Velocity / 2;
                }
                else if (obj.direction == Direction.Left)
                {
                    if (clawRope.transform.position.x <= limitClawRopeZones[0].position.x) return;
                    clawRope.transform.position += Vector3.left * clawRope.Velocity / 2;
                }
            }
        }

        private void GetCollision(EventKey.OnCollision obj)
        {
            if (obj.toy != null)
            {
                if (curToy != null) return;
                curToy = obj.toy;
                delayTween = DOVirtual.DelayedCall(0.15f, () =>
                {
                    SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);
                    curToy.OnGrabbing(clawRope.ItemZone);

                    clawRope.OnPickup(() =>
                    {
                        int rd = UnityEngine.Random.Range(0, 100);
                        if (rd < failRate) // On Fail
                        {
                            // SOund Grab Fail Here
                            SoundManager.instance.PlayOtherSfx(SfxOtherType.Incorrect);

                            clawRope.OnGrabFail();

                            curToy.OnJumpFail();
                            curToy.AssignEndPos(failZone.position, transform);
                            curToy.JumpToEndPos(() =>
                            {
                                curToy = null;

                                DOVirtual.DelayedCall(0.25f, () =>
                                {
                                    isPicking = false;
                                    grabRenderer.sprite = grabSprites[0];
                                });
                            });
                        }
                        else
                        {
                            clawRope.OnReleaseItemIntoBox(boxZone.position, () =>
                            {
                                EventDispatcher.Instance.Dispatch(new EventKey.OnSuccess { toy = curToy, id = curToy.id });
                                curToy.OnReleassing();
                                curToy = null;

                                delayTweenItem = DOVirtual.DelayedCall(1f, () =>
                                {
                                    isPicking = false;
                                    grabRenderer.sprite = grabSprites[0];
                                    //  clawControl.EnableDrag();
                                });

                                delayTween2 = DOVirtual.DelayedCall(1, () =>
                                {
                                    // SOund Congratulation Complete Here
                                    SoundManager.instance.PlayOtherSfx(SfxOtherType.RingLighting);

                                    foreach (var lighting in lightings)
                                    {
                                        lighting.OnTwinkling();
                                    }

                                    toyClaimedCount++;
                                    if (toyClaimedCount == toys.Length)
                                    {
                                        SoundManager.instance.PlayOtherSfx(SfxOtherType.Congratulation);
                                        delayTween2 = DOVirtual.DelayedCall(1, () =>
                                        {
                                            OnEndGame();
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
            }
        }

        private void OnEndGame()
        {
            OnTurnOff();
        }
        void OnTurnOff()
        {
            if (!canClick) return;
            canClick = false;

            UISetupManager.Instance.maskBg.gameObject.SetActive(true);
            uIPanel.Hide(() =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { clawMachineMode = this });

                UISetupManager.Instance.maskBg.gameObject.SetActive(false);
                Destroy(gameObject);
            });
        }

        private void GetClickItem(EventKey.OnClickItem obj)
        {
            if (obj.button != null)
            {
                if (isPicking) return;
                isPicking = true;

                // Sound Click Here
                clawControl.DisableDrag();
                grabRenderer.sprite = grabSprites[1];
                clawRope.PLayPickup(() =>
                {
                    clawRope.OnPickup(() =>
                    {
                        grabRenderer.sprite = grabSprites[0];
                        isPicking = false;
                    });
                });
            }
        }
    }
}