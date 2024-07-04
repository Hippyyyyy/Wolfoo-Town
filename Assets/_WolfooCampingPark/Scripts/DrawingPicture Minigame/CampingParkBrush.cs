using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class CampingParkBrush : MonoBehaviour
    {
        [SerializeField] LineRenderer brush;
        [SerializeField] Transform brushHead;
        [SerializeField] CoverDragObject dragObject;
        [SerializeField] ParticleSystem rainbowFx;
        [SerializeField] Image brushColorImg;
        [SerializeField] private AudioClip myAudioClip;

        public Action OnCreateBrush;
        public Action OnBeginErase;

        #region ===== POOLING =====
        IObjectPool<LineRenderer> m_Pool;
        public IObjectPool<LineRenderer> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    m_Pool = new ObjectPool<LineRenderer>(
                        CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                }
                return m_Pool;
            }
        }

        private void OnDestroyPoolObject(LineRenderer obj)
        {
        }

        private void OnReturnedToPool(LineRenderer obj)
        {
        }

        private void OnTakeFromPool(LineRenderer obj)
        {
        }

        private LineRenderer CreatePooledItem()
        {
            return null;
        }
        #endregion

        [System.Serializable]
        public class LimitArea
        {
            public Vector2 leftLimit;
            public Vector2 rightLimit;
            public Vector2 upLimit;
            public Vector2 downLimit;

            public LimitArea(Vector2 leftLimit, Vector2 rightLimit, Vector2 upLimit, Vector2 downLimit)
            {
                this.leftLimit = leftLimit;
                this.rightLimit = rightLimit;
                this.upLimit = upLimit;
                this.downLimit = downLimit;
            }
        }

        private Camera m_camera;
        LineRenderer currentLineRenderer;
        Vector2 lastPos;
        protected Color curColor;
        private Transform spawnArea;
        private LimitArea myLimit;
        private bool collectionChecks;
        private int maxPoolSize;
        private int currentOrder;
        private AudioSource myAus;

        protected virtual void Start()
        {
            if(brushColorImg != null) curColor = brushColorImg.color;
            m_camera = Camera.main;
            dragObject.BeginDrag += GetBeginDrag;
            dragObject.Drag += GetDrag;
            dragObject.EndDrag += GetEndDrag;

            myAus = SoundCampingParkManager.Instance.CreateNewAus(new SoundBase<SoundCampingParkManager>.Item(GetInstanceID(), "Erase " + GetInstanceID(), myAudioClip, true));
        }
        private void OnDestroy()
        {
            dragObject.BeginDrag -= GetBeginDrag;
            dragObject.Drag -= GetDrag;
            dragObject.EndDrag -= GetEndDrag;

            Destroy(myAus.gameObject);
        }

        public void ChangeOrder(int order)
        {
            currentOrder = order;
        }

        protected virtual void GetEndDrag()
        {
            currentLineRenderer = null;
            myAus.Stop();
        }

        private void GetDrag()
        {
            PointToMousePos();
        }

        protected virtual void GetBeginDrag()
        {
            CreateBrush();
            myAus.Play();
        }
        public void Setup(Transform spawnArea, LimitArea limitArea)
        {
            this.spawnArea = spawnArea;
            myLimit = limitArea;
        }
        public void ChangeColor(Color color)
        {
            curColor = color;
            brushColorImg.color = color;
            rainbowFx.Play();
        }
        void CreateBrush()
        {
            OnCreateBrush?.Invoke();
            currentLineRenderer = Instantiate(brush, spawnArea);
            currentLineRenderer.startColor = curColor;
            currentLineRenderer.endColor = curColor;
            currentLineRenderer.sortingOrder = currentOrder;

            //because you gotta have 2 points to start a line renderer, 
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            CheckLimit(mousePos, () =>
            {
                currentLineRenderer.SetPosition(0, brushHead.position);
                currentLineRenderer.SetPosition(1, brushHead.position);
            });
        }

        void AddAPoint(Vector2 pointPos)
        {
            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);
        }

        void CheckLimit(Vector2 mousePos, System.Action OnSuccess)
        {
            if (mousePos.y > myLimit.upLimit.y)
            {
                //    mousePos = new Vector3(mousePos.x, myLimit.upLimit.y, 0);
                return;
            }
            if (mousePos.y < myLimit.downLimit.y)
            {
                //    mousePos = new Vector3(mousePos.x, myLimit.downLimit.y, 0);
                return;
            }
            if (mousePos.x > myLimit.rightLimit.x)
            {
                //     mousePos = new Vector3(myLimit.rightLimit.x, mousePos.y, 0);
                return;
            }
            if (mousePos.x < myLimit.leftLimit.x)
            {
                //    mousePos = new Vector3(myLimit.leftLimit.x, mousePos.y, 0);
                return;
            }

            OnSuccess?.Invoke();
        }

        void PointToMousePos()
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            CheckLimit(mousePos, () =>
            {
                if (lastPos != (Vector2)brushHead.position)
                {
                    AddAPoint(brushHead.position);
                    lastPos = brushHead.position;
                }
            });
        }
    }
}
