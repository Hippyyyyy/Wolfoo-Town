using _Base;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class TutorialController : SingletonBind<TutorialController>
    {
        private TutorialConfigData data;
        private bool isOrdered;

        private void Start()
        {
            OrderData();
        }
        private void OrderData()
        {
            if (isOrdered) return;
            isOrdered = true;

            data = DataSceneManager.instance.TutorialData;
        }
        public Tutorial CreateTutorial()
        {
            var tutorial = new Tutorial();
            return tutorial;
        }
        public T CreateStep<T>() where T : TutorialStep
        {
            var tutPb = data.GetData<T>();
            if(tutPb == null)
            {
                Debug.LogError("Your Tutorial Step class is Not Declare !!!");
                return null;
            }
            var tut = Instantiate(tutPb);
            return tut.GetComponent<T>();
        }
    }
}
