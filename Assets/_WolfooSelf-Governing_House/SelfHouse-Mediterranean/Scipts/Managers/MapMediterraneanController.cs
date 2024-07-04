using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class MapMediterraneanController : MonoBehaviour
    {
        [SerializeField] int totalFloor;
        int curFloorIdx;
        private bool isUp = true;
        private bool isStart = true;

        void Start()
        {
            //    backMove.transform.position = coverImgs[curFloorIdx].transform.position;
            EventDispatcher.Instance.RegisterListener<EventKey.OnSelect>(GetSelect);
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnSelect>(GetSelect);
        }

        private void OnEnable()
        {
            if (isStart) return;
            isStart = false;

            OnOpenFloor();
        }

        void OnOpenFloor()
        {
            GUIManager.instance.OnLoading(() =>
            {
                switch (curFloorIdx)
                {
                    case 0:
                        GUIManager.instance.OpenPanel(PanelType.BeachVillaRoom1);
                        break;
                }
            });
            GameManager.instance.CurFloorIdx = curFloorIdx;
        }

        void OpenFloor()
        {
            if (AdsManager.Instance.HasInters)
            {
                _Base.FirebaseManager.instance.LogWatchAds(_Base.AdsLogType.ad_inter_request.ToString(), "wolfoo_beach");
                AdsManager.Instance.ShowInterstitial(() =>
                {
                    _Base.FirebaseManager.instance.LogWatchAds(_Base.AdsLogType.ad_inter_success.ToString(), "wolfoo_beach");
                    OnOpenFloor();
                });
            }
            else
            {
                _Base.FirebaseManager.instance.LogWatchAds(_Base.AdsLogType.ad_inter_fail.ToString(), "wolfoo_beach");
                OnOpenFloor();
            }
        }

        private void GetSelect(EventKey.OnSelect obj)
        {
            if (GUIManager.instance.CurrentMapController != _Base.CityType.BeachVilla) return;

            if (obj.elevatorPanel != null)
            {
                curFloorIdx = obj.idx;
                OpenFloor();
                return;
            }

            if (obj.elevator != null)
            {
                if (isUp)
                    curFloorIdx++;
                else
                    curFloorIdx--;

                if (curFloorIdx >= totalFloor)
                {
                    curFloorIdx -= 2;
                    isUp = false;
                }
                else if (curFloorIdx < 0)
                {
                    curFloorIdx += 2;
                    isUp = true;
                }

                EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { mediterraneanController = this, idx = curFloorIdx, mapControllerType = _Base.CityType.BeachVilla });
                OpenFloor();
            }

            if (obj.floor != null)
            {
                if (obj.direction == Direction.Up)
                {
                    curFloorIdx++;
                    if (curFloorIdx == totalFloor)
                    {
                        curFloorIdx--;
                        return;
                    }

                }
                if (obj.direction == Direction.Down)
                {
                    curFloorIdx--;
                    if (curFloorIdx == totalFloor)
                    {
                        curFloorIdx++;
                        return;
                    }
                }
                OpenFloor();
            }
        }
    }

}