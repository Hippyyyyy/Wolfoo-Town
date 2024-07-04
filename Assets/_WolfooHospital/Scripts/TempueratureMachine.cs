using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class TempueratureMachine : BackItem
    {
        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, tempueratureMachine = this });
        }

        public void Scan(Vector3 _endPos)
        {
            transform.position = _endPos;
            //SoundManager.instance.PlayHospitalSfx(SfxHospitalType.)
        }
    }
}