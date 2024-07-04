using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SCN.Tutorial;
using _WolfooShoppingMall;
using _WolfooCity;
using DG.Tweening;

namespace _Base
{
    public class SettingPanel : UIPanel
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button soundBtn;
        [SerializeField] Button musicBtn;
        [SerializeField] List<Sprite> musicSprites;
        [SerializeField] List<Sprite> soundSprites;
        private Tween delayTwen;

        private void Start()
        {
            backBtn.onClick.AddListener(OnBack);
            soundBtn.onClick.AddListener(OnSoundClick);
            musicBtn.onClick.AddListener(OnMusicClick);

            var isMuteSound = BaseDataManager.Instance.IsMuteSound;
            var isMuteMusic = BaseDataManager.Instance.IsMuteMusic;
            soundBtn.image.sprite = soundSprites[isMuteSound ? 1 : 0];
            musicBtn.image.sprite = musicSprites[isMuteMusic ? 1 : 0];
        }
        private void OnDestroy()
        {
            musicBtn.onClick.RemoveAllListeners();
            soundBtn.onClick.RemoveAllListeners();
            backBtn.onClick.RemoveAllListeners();
        }
        void OnBack()
        {
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);
            Hide(() =>
            {
                gameObject.SetActive(false);
            });
        }
        void OnSoundClick()
        {
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);

            var isMuteSound = BaseDataManager.Instance.IsMuteSound;
            if (isMuteSound)
            {
                soundBtn.image.sprite = soundSprites[0];
                SoundBaseManager.instance.EnableSound(SoundType.SFx, true);
            }
            else
            {
                soundBtn.image.sprite = soundSprites[1];
                SoundBaseManager.instance.MuteSound(SoundType.SFx, true);
            }
        }


        void OnMusicClick()
        {
            SoundBaseManager.instance.PlayOtherSfx(SfxOtherType.Click);

            var isMuteMusic = BaseDataManager.Instance.IsMuteMusic;

            if (!isMuteMusic)
            {
                musicBtn.image.sprite = musicSprites[1];
                SoundBaseManager.instance.MuteSound(SoundType.Music, true);
            }
            else
            {
                musicBtn.image.sprite = musicSprites[0];
                SoundBaseManager.instance.EnableSound(SoundType.Music, true);
            }
        }
    }
}