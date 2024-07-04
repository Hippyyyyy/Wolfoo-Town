using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class MediterraneanManager : MonoBehaviour
    {
        [SerializeField] UIMediterraneanManager ui;
        [SerializeField] OptionViews[] optionViews;

        private List<BackItemWorld> charactersInMap = new List<BackItemWorld>();
        private MediterraneanDataSO data;

        private void Start()
        {
            optionViews[0].GetClickOptionView(0);
            EventSelfHouseRoom.OnClickDecorOption += GetClickDecorOption;
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(GetInitItem);
            EventRoomBase.OnEndDragScrollCharacter += OnEndDragScrollCharacter;
        }
        private void OnDestroy()
        {
            for (int i = 0; i < optionViews.Length; i++)
            {
                optionViews[i].OnClick -= GetClickOptionView;
            }
            EventSelfHouseRoom.OnClickDecorOption -= GetClickDecorOption;
            EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(GetInitItem);
            EventRoomBase.OnEndDragScrollCharacter -= OnEndDragScrollCharacter;
        }

        private void OnEndDragScrollCharacter(BackItemWorld character, bool insideScrollView)
        {
            if(insideScrollView)
            {
                if (charactersInMap.Contains(character))
                    charactersInMap.Remove(character);
            }
            else
            {
                charactersInMap.Add(character);
            }
        }

        private void GetInitItem()
        {
            data = DataSceneManager.Instance.MediterraneanData;
            for (int i = 0; i < data.DecorDatas.Length; i++)
            {
                var decorData = data.DecorDatas[i];
                optionViews[i].Assign(i, decorData);
                optionViews[i].OnClick += GetClickOptionView;
            }
            GetClickOptionView(optionViews[0]);
        }

        private void GetClickDecorOption(bool isOpen)
        {
            StartCoroutine(SetStateForCharacterInMap(!isOpen));
        }
        IEnumerator SetStateForCharacterInMap(bool isActive)
        {
            yield return new WaitForEndOfFrame();

            foreach (var item in charactersInMap)
            {
                item.gameObject.SetActive(isActive);
            }
        }

        private void GetClickOptionView(OptionViews obj)
        {
            for (int i = 0; i < optionViews.Length; i++)
            {
                optionViews[i].GetClickOptionView(obj.TopicId);
            }
        }
    }
}
