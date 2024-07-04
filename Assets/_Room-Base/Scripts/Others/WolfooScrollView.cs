using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class WolfooScrollView : PlayerScrollView
    {
        [SerializeField] Transform scrollItemViewPb;

        public void Assign(NewCharacterDataSO data, NewPlayerPanel playerPanel)
        {
            for (int i = 0; i < data.WolfooSprites.Length; i++)
            {
                var itemView = Instantiate(scrollItemViewPb, scrollview.content);
                var item = itemView.GetComponentInChildren<NewCharacterWolfooScrollItem>(true);
                if (item != null)
                {
                    item.Assign(this,
                        data.WolfooPrefabs[i].GetComponent<CharacterWolfooWorld>(),
                        data.WolfooSprites[i]);
                }
            }
        }

        public override void Setup(UIManager.LimitArea limitArea)
        {
            myLimit = new Transform[4] { limitArea.downLimit, limitArea.rightLimit, limitArea.upLimit, limitArea.leftLimit };
        }
    }
}
