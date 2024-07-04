using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class TeaPotWorld : BackItemWorld, WaterProviderWorld
    {
        [SerializeField] FxHolder fx;
        [SerializeField] Transform pouringArea;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tweenRotate;

        public Vector3 PouringPos { get => pouringArea.position; }

        public void Pour()
        {
        }
        private void OnPour()
        {
            _tweenRotate?.Kill();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            _tweenRotate = transform.DORotate(Vector3.forward * 35, 0.5f).OnComplete(() =>
            {
                fx.Particle.Play();
            });
        }

        public override void Setup()
        {
            IsDragable = true;
            IsCarryItem = true;
            IsStandingOnTable = true;
            base.Setup();
            fx.Particle.Stop();
        }

        protected override void OnBeginDrag()
        {
            base.OnBeginDrag();

            OnPour();
        }
        protected override void OnEndDrag()
        {
            fx.Particle.Stop();
            _tweenRotate?.Kill();
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
