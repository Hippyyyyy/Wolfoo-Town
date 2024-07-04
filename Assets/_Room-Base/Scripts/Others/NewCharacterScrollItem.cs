using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class NewCharacterScrollItem : MonoBehaviour
    {
        [SerializeField] Image iconImg;
        [SerializeField] CharacterWorld character;
        [SerializeField] Transform myHolder;

        private EventTrigger trigger;
        private int startSiblingIdx;
        private Transform startParent;
        private PlayerScrollView myPanel;

        private void RemoveAllEvent()
        {
            trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }
        private void RegisterEvent()
        {
            trigger = gameObject.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        private void Start()
        {
            Setup();
            RegisterEvent();
        }
        private void OnDestroy()
        {
            if (trigger.triggers.Count > 0) RemoveAllEvent();
            character.GetEndDrag -= GetCharacterEndDrag;
        }

        public void Assign(PlayerScrollView panel, CharacterWorld characterWorld)
        {
            myPanel = panel;
            myHolder = transform.parent;

            character = characterWorld;
            character.Setup();
            character.GetEndDrag += GetCharacterEndDrag;
        }
        public void CreateCharacter(CharacterWorld characterPb)
        {
            character = Instantiate(characterPb, transform);
            character.AssignToScroll();
            character.GetEndDrag += GetCharacterEndDrag;
            character.Hide(transform);
        }
        private void GetCharacterEndDrag(CharacterWorld character)
        {
            if (!myPanel.IsOpenPanel) return;

            var isInside = myPanel.CheckInSideScrollView(character);
            if(isInside)
            {
                SetToScrollView();
                myPanel.ScrollTo(transform);
            }
            else
            {
                SetToMap();
            }
            EventRoomBase.OnEndDragScrollCharacter?.Invoke(character, isInside);
        }

        public void Setup()
        {
            startSiblingIdx = transform.GetSiblingIndex();
            startParent = transform.parent;
        }

        private void OnPointerDown(PointerEventData data)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
            EventRoomBase.OnBeginDragScrollCharacter?.Invoke(character);
        }
        private void OnDrag(PointerEventData data)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
        private void OnPointerUp(PointerEventData data)
        {
            if (myPanel.IsOpenPanel)
            {
                var isInside = myPanel.CheckInSideScrollView(this);
                if (isInside)
                {
                    SetToScrollView();
                }
                else
                {
                    SetToMap();
                }
                EventRoomBase.OnEndDragScrollCharacter?.Invoke(character, isInside);
            }
            transform.localPosition = Vector3.zero;
        }

        private void SetToScrollView()
        {
            character.Hide(transform);
            myHolder.parent.gameObject.SetActive(true);
            transform.SetParent(myHolder);
        }
        private void SetToMap()
        {
            character.Show();
            myHolder.parent.gameObject.SetActive(false);
        }

        public void TurnBackScroll(int idx = -1)
        {
            transform.SetParent(startParent);
            transform.SetSiblingIndex(idx > 0 ? idx : startSiblingIdx);
            transform.localPosition = Vector3.zero;
        }
    }
}
