using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class OperaStageManager : BackItem
    {
        [SerializeField] OperaStage[] myStages;
        [SerializeField] BgParticle[] myFxs;
        [SerializeField] PillarAnimation pillarAnimation;
        [SerializeField] OperaStateController operaStateController;
        [SerializeField] Transform spawnedStageArea;
        [SerializeField] ParticleSystem[] musicFxs;
        [SerializeField] Button[] curtainBtns;
        [SerializeField] Animator animator;
        private bool isOpen;
        private OperaStage curStage;
        private ParticleSystem curFx;

        public enum BgParticleType
        {
            Castle,
            Hometown,
            Christmas,
            Universe,
            Halloween,
            Ghost,
        }

        [System.Serializable]
        public class BgParticle
        {
            public BgParticleType type;
            public ParticleSystem fx;
        }

        protected override void InitData()
        {
            base.InitData();
            canClick = true;

            operaStateController.OnClickNext();
            foreach (var btn in curtainBtns)
            {
                btn.onClick.AddListener(OnClick);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OperaEventManager.OnChangeStage += GetChangeStage;
            OperaEventManager.OnChangeParticle += GetChangeParticle;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            OperaEventManager.OnChangeStage -= GetChangeStage;
            OperaEventManager.OnChangeParticle -= GetChangeParticle;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClick();
        }
        private void OnClick()
        {
            isOpen = !isOpen;
            SoundOperaManager.Instance.PlayOtherSfx(SoundTown<SoundOperaManager>.SFXType.CurtainSlide);
            if (isOpen)
            {
                animator.enabled = true;
                animator.Play("Curtain-Open", 0, 0);
                pillarAnimation.PlayOpenAnim(() =>
                {
                    foreach (var item in musicFxs) { item.Play(); }
                });
            }
            else
            {
                animator.enabled = true;
                animator.Play("Curtain-Close", 0, 0);
                pillarAnimation.PlayCloseAnim(() =>
                {
                    foreach (var item in musicFxs) { item.Stop(); }
                });
            }
        }
        private void GetChangeParticle(BgParticleType type)
        {
            curFx.Stop();
            for (int i = 0; i < myFxs.Length; i++)
            {
                if (myFxs[i].type == type)
                {
                    curFx = myFxs[i].fx;
                    curStage.ChangeFx(myFxs[i].fx);
                }
            }
        }

        private void GetChangeStage(BgParticleType type)
        {
            SoundOperaManager.Instance.PlayOtherSfx(SoundTown<SoundOperaManager>.SFXType.CurtainSlide);
            pillarAnimation.PlayCloseAnim(() =>
            {
                for (int i = 0; i < myStages.Length; i++)
                {
                    if (myStages[i].Type == type)
                    {
                        if (curStage != null)
                            curStage.MoveCharacterInto(myStages[i]);
                        curStage = myStages[i];
                        curStage.gameObject.SetActive(true);
                        curFx = myStages[i].FxDefault;
                        curStage.PlayAnim();
                    }
                    else
                    {
                        myStages[i].gameObject.SetActive(false);
                        myStages[i].StopAnim();
                    }
                }
                SoundOperaManager.Instance.PlayOtherSfx(SoundTown<SoundOperaManager>.SFXType.CurtainSlide);
                pillarAnimation.PlayOpenAnim(() =>
                {
                    foreach (var item in musicFxs) { item.Play(); }
                });
                foreach (var item in musicFxs) { item.Stop(); }
            });
        }
    }
}