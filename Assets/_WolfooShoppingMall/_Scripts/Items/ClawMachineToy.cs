using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ClawMachineToy : ItemMove
    {
        private PolygonCollider2D collider2d;
        private Rigidbody2D rigidbody2d;
        private SpriteRenderer spriteRenderer;
        private Tween delayTween;

        private void Awake()
        {
            collider2d = GetComponent<PolygonCollider2D>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var isClaw = collision.gameObject.GetComponentInParent<ClawRopeMachine>() != null ? true : false;
            if (isClaw)
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnCollision { toy = this });
            }

            if (collision.collider.gameObject.tag.Equals("DeathZone"))
            {
                Destroy(gameObject);
               // rigidbody2d.isKinematic = true;
                //gameObject.SetActive(false);
            }
        }

        public void AssignRigidbody()
        {
            //   rigidbody2d.isKinematic = false;
            if (rigidbody2d == null)
                rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        }

        public void OnGrabbing(Transform _parent)
        {
            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;
            collider2d.enabled = false;
            rigidbody2d.isKinematic = true;
        }
        public void OnReleassing()
        {
            collider2d.enabled = false;
            rigidbody2d.isKinematic = false;
            transform.SetParent(startParent);

            if (delayTween != null) delayTween?.Kill();
            delayTween = DOVirtual.DelayedCall(3, () =>
            {
                collider2d.enabled = true;
            });
        }
        public void OnJumpFail()
        {
            collider2d.enabled = true;
            rigidbody2d.isKinematic = false;
            transform.SetParent(startParent);
        }
        public void SetToStartParent()
        {
        }
    }
}