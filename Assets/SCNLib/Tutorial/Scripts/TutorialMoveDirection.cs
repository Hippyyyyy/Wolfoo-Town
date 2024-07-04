using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCN.Common;
using DG.Tweening;

namespace SCN.Tutorial
{
	public class TutorialMoveDirection : MonoBehaviour
	{
		[SerializeField] RectTransform tutPoint;
		[SerializeField] RectTransform upPoint;
		[SerializeField] RectTransform downPoint;
		[SerializeField] RectTransform leftPoint;
		[SerializeField] RectTransform rightpPoint;

		public void StartTut(Direction dir, Gesture gesture = Gesture.PointAt
			, bool isRight = true, LoopType loopType = LoopType.Restart)
		{
			tutPoint.eulerAngles = Vector3.zero;

			if (dir == Direction.Up)
			{
				TutorialManager.Instance.StartPointer(tutPoint.position
					, upPoint.position, gesture, isRight, loopType);
			}
			else if (dir == Direction.Down)
			{
				TutorialManager.Instance.StartPointer(tutPoint.position
					, downPoint.position, gesture, isRight, loopType);
			}
			else if (dir == Direction.Left)
			{
				TutorialManager.Instance.StartPointer(tutPoint.position
					, leftPoint.position, gesture, isRight, loopType);
			}
			else if (dir == Direction.Right)
			{
				TutorialManager.Instance.StartPointer(tutPoint.position
					, rightpPoint.position, gesture, isRight, loopType);
			}
		}

		public void StartTut(Orientation orientation, Gesture gesture = Gesture.PointAt
			, bool isRight = true, LoopType loopType = LoopType.Yoyo)
		{
			if (orientation == Orientation.Horizontal)
			{
				TutorialManager.Instance.StartPointer(leftPoint.position
					, rightpPoint.position, gesture, isRight, loopType);
			}
			else if (orientation == Orientation.Vertical)
			{
				TutorialManager.Instance.StartPointer(upPoint.position
					, downPoint.position, gesture, isRight, loopType);
			}
		}
	}
}