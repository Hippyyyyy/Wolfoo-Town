using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCN;
using DG.Tweening;
using _Base;

namespace _WolfooCity.Minigames.Puzzle
{
    public class SoundManager : SingletonFreeAlive<SoundManager>
    {
        [SerializeField] private AudioSource themeSound;
        [SerializeField] private AudioSource buttonSound;
        [SerializeField] private AudioSource pieceBeginSound;
        [SerializeField] private AudioSource pieceEndSound;
        [SerializeField] private AudioSource pieceCorrectSound;
        [SerializeField] private AudioSource winSound;
        [SerializeField] private AudioSource winSound2;
        private void Start()
        {
            PlaySound(SoundType.Theme);
        }
        public void PlaySound(SoundType soundType)
        {
            if (SoundBaseManager.instance.IsSoundMuted) return;
            switch (soundType)
            {
                case SoundType.Theme:
                    if (themeSound == null) return;
                    themeSound.Play();
                    break;
                case SoundType.ButtonClick:
                    if (buttonSound == null) return;
                    buttonSound.Play();
                    break;
                case SoundType.PieceBeginDrag:
                    if (pieceBeginSound == null) return;
                    pieceBeginSound.Play();
                    break;
                case SoundType.PieceEndDrag:
                    if (pieceEndSound == null) return;
                    pieceEndSound.Play();
                    break;
                case SoundType.PieceCorrect:
                    if (pieceCorrectSound == null) return;
                    pieceCorrectSound.Play();
                    break;
                case SoundType.WinSound:
                    if (themeSound == null) return;
                    winSound2.Play();
                    DOVirtual.DelayedCall(0.5f, () => {
                        winSound2.Play();
                    });
                    break;
            }
        }
    }
    public enum SoundType
    {
        Theme,
        ButtonClick,
        PieceBeginDrag,
        PieceEndDrag,
        PieceCorrect,
        WinSound
    }

}