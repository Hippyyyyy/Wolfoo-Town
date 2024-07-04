using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames.Puzzle
{
    public class PuzzleGameplay : MonoBehaviour
    {
        [SerializeField] LevelManager levelManager;
        [SerializeField] PuzzleGameSetting _setting;
        [SerializeField] UIPanel _panel;
        [SerializeField] ParticleSystem[] conffetiFxs;

        int curIdxPicutre;
        private Tween _tween;

        private void Start()
        {
            curIdxPicutre = _setting.startIdxPicture;
            OnChangePicture();
            levelManager.OnEndGame = GetEndGame;
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }

        private void GetEndGame()
        {
            foreach (var fx in conffetiFxs)
            {
                fx.Play();
            }

            _WolfooShoppingMall.SoundManager.instance.PlayOtherSfx(_WolfooShoppingMall.SfxOtherType.Congratulation, true);
            _tween = DOVirtual.DelayedCall(conffetiFxs[0].main.duration, () =>
            {
                OnClickNext();
            });
        }

        void OnChangePicture()
        {
            _setting.startIdxPicture = curIdxPicutre;
            levelManager.OnLevelStart(_setting.Sprites[curIdxPicutre]);
        }


        #region INPUT EVENT

        public void OnClose()
        {
            _panel.Hide(() => Destroy(this.gameObject));
        }
        public void OnClickNext()
        {
            curIdxPicutre++;
            if(curIdxPicutre >= _setting.Sprites.Length)
            {
                curIdxPicutre = 0;
            }
            OnChangePicture();
        }
        public void OnClickPrev()
        {
            curIdxPicutre--;
            if (curIdxPicutre < 0)
            {
                curIdxPicutre = _setting.Sprites.Length - 1;
            }
            OnChangePicture();
        }
        #endregion
    }
}