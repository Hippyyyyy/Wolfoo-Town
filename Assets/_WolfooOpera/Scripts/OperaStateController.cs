using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class OperaStateController : BackItem
    {
        [SerializeField] Sprite[] iconStageSprites;
        [SerializeField] Image iconStageImg;
        [SerializeField] OperaStageManager.BgParticleType[] types;
        [SerializeField] Button[] buttons;

        int curStageIdx;

        protected override void Start()
        {
            base.Start();

            for (int i = 0; i < buttons.Length; i++)
            {
                var idx = i;
                buttons[idx].onClick.AddListener(() => OnClickParticleButton(idx));
            }
        }

        #region UI INPUT METHODS
        public void OnClickNext()
        {
            curStageIdx++;
            if(curStageIdx >= types.Length) curStageIdx = 0;
            iconStageImg.sprite = iconStageSprites[curStageIdx];
            iconStageImg.SetNativeSize();
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            OperaEventManager.OnChangeStage?.Invoke(types[curStageIdx]);
        }
        public void OnClickPrevious()
        {
            curStageIdx--;
            if(curStageIdx < 0) curStageIdx = iconStageSprites.Length;
            iconStageImg.sprite = iconStageSprites[curStageIdx];
            iconStageImg.SetNativeSize();
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            OperaEventManager.OnChangeStage?.Invoke(types[curStageIdx]);
        }
        public void OnClickParticleButton(int id)
        {
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            OperaEventManager.OnChangeParticle?.Invoke(types[id]);
        }
        #endregion
    }
}