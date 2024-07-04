using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static _WolfooShoppingMall.UIManager;

namespace _WolfooShoppingMall
{
    public class NewPlayerPanel : MonoBehaviour
    {
        //  [SerializeField] WolfooScrollView wolfooScrollView
        [SerializeField] CharacterScrollView characterScrollView;
        [SerializeField] Transform scrollNewCharacterItemViewPb;
        [SerializeField] Transform scrollWolfooItemViewPb;
        [SerializeField] Transform[] testData;

        private UIManager myUI;
        private NewCharacterDataSO newCharacterDataSO;


        private void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(GetData);
            EventRoomBase.OnBeginDragScrollCharacter += GetEndDragScrollCharacter;
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(GetData);
            myUI.OnClickCharacterBtn -= OnClickCharacterPanel;
            myUI.OnClickWolfooBtn -= OnClickWolfooPanel;
            myUI.OnChangeStatePanel -= OnChangeState;
            EventRoomBase.OnBeginDragScrollCharacter -= GetEndDragScrollCharacter;
        }

        private void GetEndDragScrollCharacter(BackItemWorld arg1)
        {
            Debug.Log("GetEndDragScrollCharacter");
            characterScrollView.scrollview.velocity = Vector2.zero;
        }

        public void Assign(LimitArea limitArea, UIManager ui)
        {
            characterScrollView.gameObject.SetActive(false);
            //    wolfooScrollView.gameObject.SetActive(false);

            myUI = ui;
            myUI.OnClickCharacterBtn += OnClickCharacterPanel;
            myUI.OnClickWolfooBtn += OnClickWolfooPanel;
            myUI.OnChangeStatePanel += OnChangeState;

            //      wolfooScrollView.Setup(limitArea);
            characterScrollView.Setup(limitArea);

        }

        private void OnChangeState(bool isOpen)
        {
            if (isOpen)
            {
                characterScrollView.Show();
            }
            else
            {
                characterScrollView.Hide();
            }
        }

        public void AssignWolfoo(NewCharacterDataSO data, NewPlayerPanel playerPanel)
        {
            var localSaveLoad = DataSceneManager.Instance.LocalDataStorage.unlockCharacters;

            Debug.Log("Data: " + data);
            for (int i = 0; i < data.WolfooSprites.Length; i++)
            {
                var itemView = Instantiate(scrollWolfooItemViewPb, characterScrollView.scrollview.content);
                var item = itemView.GetComponentInChildren<NewCharacterWolfooScrollItem>(true);
                if (item != null)
                {
                    item.Assign(i, !localSaveLoad[i] && !AdsManager.Instance.IsRemovedAds );
                    item.Assign(characterScrollView,
                        data.WolfooPrefabs[i].GetComponent<CharacterWolfooWorld>(),
                        data.WolfooSprites[i]);
                }
            }
        }
        public void AssignCharacter()
        {
            var totalCharacter = CharacterTransporter.TotalCharacterHolder;
            var characterWorldPb = CharacterTransporter.WorldCharacter;
            var characterUiPb = CharacterTransporter.UICharacter;
            Debug.Log("Data Character: " + totalCharacter);
            
            for (int i = 0; i < totalCharacter; i++)
            {
                var itemView = Instantiate(scrollNewCharacterItemViewPb, characterScrollView.scrollview.content);
                var itemWorld = Instantiate(characterWorldPb, itemView.GetChild(0));
                var itemUi = Instantiate(characterUiPb, itemView.GetChild(0));
                
                var characterDataSet = CharacterTransporter.GetDataSet(i);
                itemWorld.CharacterPartHelper.Assign(characterDataSet);
                itemUi.CharacterPartHelper.Assign(characterDataSet);
                itemWorld.gameObject.SetActive(false);
                itemUi.transform.GetComponentInChildren<NewCharacterScrollItem>(true).Assign(characterScrollView, itemWorld.GetComponent<CharacterWorld>());
            }
        }

        private void OnClickCharacterPanel()
        {
        }

        private void OnClickWolfooPanel()
        {
        }

        private void GetData(EventKey.OnLoadDataCompleted obj)
        {
            newCharacterDataSO = DataSceneManager.Instance.NewCharacterData;
            //       wolfooScrollView.Assign(newCharacterDataSO, this);

            AssignCharacter();
            AssignWolfoo(newCharacterDataSO, this);
        }
    }
}
