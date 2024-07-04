using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCN.Animation;

namespace SCN.Tutorial
{
	public class PointerAnimName : MonoBehaviour
	{
		[SerializeField] AnimationSpineController.SpineAnim idle;
		[SerializeField] AnimationSpineController.SpineAnim hold;
		[SerializeField] AnimationSpineController.SpineAnim click;

		public AnimationSpineController.SpineAnim Idle => idle;
		public AnimationSpineController.SpineAnim Hold => hold;
		public AnimationSpineController.SpineAnim Click => click;
	}
}