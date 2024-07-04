using DG.Tweening;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCN.ActionLib
{
    public class DividePathDemo : MonoBehaviour
    {
        [SerializeField] Transform canvasTrans;
        [SerializeField] GameObject pointObj;

        GameObject[] obj;
        [SerializeField] Vector3[] dataPos;

		private void Start()
		{
            obj = new GameObject[20];
            for (int i = 0; i < 20; i++)
            {
                obj[i] = Instantiate(pointObj, canvasTrans);
                obj[i].transform.position = dataPos[i * 3];
            }
        }

		private void Update()
		{
            if (Input.GetKeyDown(KeyCode.M))
			{
				for (int i = 0; i < 20; i++)
				{
                    MoveElement(obj[i].transform, i * 3);
				}
			}
		}

        void MoveElement(Transform obj, int currentIndex)
        {
            currentIndex++;
            if (currentIndex >= 60)
            {
                currentIndex = 0;
            }

            DOTweenManager.Instance.TweenMoveTime(obj, dataPos[currentIndex], 0.2f, false)
                .OnComplete(() =>
                {
                    MoveElement(obj, currentIndex);
                });
        }

#if UNITY_EDITOR
        [SerializeField] CustomPath path;

        [ContextMenu(nameof(CalculatePos))]
        void CalculatePos()
		{
			for (int i = 0; i < path.DivisionSegments.Length; i++)
			{
                path.DivisionSegments[i] = 3000;
			}

            // Neu can 20 diem di chuyen tren duong cong, thi chia duong cong thanh 40, 60, 80, ... tuy duong cong
            // sau do spawn xen ke, khi di chuyen thi chi can cho diem nay di chuyen den diem tiep theo
            // toan bo qua trinh tinh toan cac diem se dung trong editor de dam bao performance
            var data = path.DividePath(60);
            dataPos = new Vector3[60];

            for (int i = 0; i < data.Length; i++)
            {
                dataPos[i] = path.GetPos(data[i]);
            }
        }
#endif
    }
}