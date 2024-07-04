using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static _WolfooShoppingMall.OperaStageManager;

namespace _WolfooShoppingMall
{
    public class OperaStage : MonoBehaviour
    {
        [SerializeField] BgParticleType _myType;
        [SerializeField] ParticleSystem fxDefault;
        [SerializeField] Transform fxArea;
        [SerializeField] Animator _animator;
        [SerializeField] string animName;
        [SerializeField] Table[] myStandAreas;
        private Tween _tween;

        public BgParticleType Type { get => _myType; }
        public ParticleSystem FxDefault { get => fxDefault; }

        private List<Character> myCharacters = new List<Character>();

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }

        public void MoveCharacterInto(OperaStage _endStage)
        {
            foreach (var character in myCharacters)
            {
                _endStage.OnMoveCharacter(character);
            }
        }

        void OnMoveCharacter(Character character)
        {
            character.transform.SetParent(transform);
            myCharacters.Clear();
            myCharacters.Add(character);
        }

        private void GetEndDragBackItem(EventKey.OnEndDragBackItem item)
        {
            if(item.character != null)
            {
                _tween?.Kill();
                _tween = DOVirtual.DelayedCall(0.2f, () =>
                {
                    myCharacters.Clear();
                    foreach (var standArea in myStandAreas)
                    {
                        foreach (var character in standArea.GetComponentsInChildren<Character>())
                        {
                            myCharacters.Add(character);
                        }
                    }
                });
            }
        }

        public void ChangeFx(ParticleSystem particle)
        {
            particle.transform.SetParent(fxArea);
            particle.Play();
        }

        public void PlayAnim()
        {
            _animator.enabled = true;
            _animator.Play(animName, 0, 0);
        }
        public void StopAnim()
        {
            _animator.enabled = false;
        }
    }
}