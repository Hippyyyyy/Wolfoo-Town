using DG.Tweening;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCN.Tutorial
{
    public class Demo5 : MonoBehaviour
    {
		[SerializeField] Transform itemToSpin;
        [SerializeField] Button btn;
        [SerializeField] Button btn1;

		bool isSpinFlat;

		private void Start()
		{
			btn.onClick.AddListener(Callback_Btn0);
			btn1.onClick.AddListener(Callback_Btn1);
            TutorialManager.Instance.StartPointer(itemToSpin, Gesture.Spin);
        }


		void Callback_Btn0()
		{
            Debug.Log("Change");
			if (!isSpinFlat)
			{
                TutorialManager.Instance.Stop();
                StartCoroutine(DelayCallMaster.WaitForFrame(() =>
                {
                    TutorialManager.Instance.StartPointer(itemToSpin, Gesture.SpinFlat);
                }));
                
                isSpinFlat = true;
            }
			else
			{
                TutorialManager.Instance.Stop();
                StartCoroutine(DelayCallMaster.WaitForFrame(() =>
                {
                    TutorialManager.Instance.StartPointer(itemToSpin, Gesture.Spin);
                }));
                isSpinFlat = false;
            }
        }
		void Callback_Btn1()
		{
            Debug.Log("Stop");
            TutorialManager.Instance.Stop();
        }

	}
}