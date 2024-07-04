using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SCN.Tutorial
{
    public class Demo4 : MonoBehaviour
    {
        [SerializeField] Button btn0;
        [SerializeField] Button btn1;

		private void Start()
		{
			btn0.onClick.AddListener(Callback_Btn0);
			btn1.onClick.AddListener(Callback_Btn1);

			TutorialManager.Instance.Preload(); // Phai goi ham nay truoc it nhat 1 frame khi goi Focus
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				TutorialManager.Instance.StartFocus(btn0.GetComponent<RectTransform>(), false);
			}
            if (Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene("Demo 4");
            }
        }

		void Callback_Btn0()
		{
			TutorialManager.Instance.StopFocus(true, () =>
			{
				TutorialManager.Instance.StartFocus(btn1.GetComponent<RectTransform>());
			});
		}

		void Callback_Btn1()
		{
			TutorialManager.Instance.StopFocus();
		}
	}
}