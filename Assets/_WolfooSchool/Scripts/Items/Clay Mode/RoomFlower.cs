using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class RoomFlower : CarryItem
    {
        [SerializeField] ParticleSystem lightingFx;

        protected override void Start()
        {
            base.Start();
            isDrag = true;
            isInBag = true;
            isCarryItem = true;
            isStandTable = true;
            isClick = true;
            isDrag = true;

            if (lightingFx == null)
            {
                lightingFx = Instantiate(GUIManager.instance.lightingFx, transform);
            }
        }
        public void Init()
        {
            GetMainPanel(GUIManager.instance.CurPanel, GUIManager.instance.CurGround);
            Invoke("TurnOnRayCast", 1);
        }
        private void TurnOnRayCast()
        {
            GetComponent<Image>().raycastTarget = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            lightingFx.Play();
        }

    }
}
