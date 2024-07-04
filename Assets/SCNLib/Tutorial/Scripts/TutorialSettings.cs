using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace SCN.Tutorial
{
	[CreateAssetMenu(fileName = AssetName, menuName = "SCN/Scriptable Objects/Tutorial setting")]
	public class TutorialSettings : ScriptableObject
	{
		public const string AssetName = "Tutorial setting";

		[Header("Setings")]

		[Tooltip("Layer cua ban tay")]
		[SerializeField] string sortingLayer;
		[Tooltip("Layer cua ban tay")]
		[SerializeField] int orderInLayer;

		[Space(2)]
		[Header("Bat buoc phai dien gia tri nay")]
		[Tooltip("Ti le man hinh Ref tren CanvasScaler")]
		[SerializeField] Vector2 referenceResolution = new Vector2(2208, 1242);

		[Space(10)]
		[Header("Pointer setting")]
		[SerializeField] float delayStartTime = 3;
		[SerializeField] float noReactTime = 5;
		[SerializeField] float pointerScale = 1f;

		[SerializeField] float pointerVelocity = 7;

		[Space(10)]
		[SerializeField] Sprite spinArrowSprite;

		[Space(10)]
		[Header("Focus setting")]
		[Tooltip("Focus vao target, cach target bao nhieu")]
		[SerializeField] int focusExpand = 50;
		[Range(0, 1)]
		[SerializeField] float focusAlpha = 0.66f;

		public string SortingLayer => sortingLayer;
		public int OrderInLayer => orderInLayer;
		public Vector2 ReferenceResolution => referenceResolution;

		public float DelayStartTime
		{
			get => delayStartTime;
			set => delayStartTime = value;
		}

		public float NoReactTime
		{
			get => noReactTime;
			set => noReactTime = value;
		}
		public float PointerScale => pointerScale;

		public float PointerVelocity
		{
			get => pointerVelocity;
			set => pointerVelocity = value;
		}
		public Sprite SpinArrowSprite
		{
			get => spinArrowSprite;
			set => spinArrowSprite = value;
		}
		public int FocusExpand
		{
			get => focusExpand;
			set => focusExpand = value;
		}
		public float FocusAlpha
		{
			get => focusAlpha;
			set => focusAlpha = value;
		}

		public void Preload()
		{
			TutorialManager.Instance.Preload();
		}

#if UNITY_EDITOR
		[MenuItem("SCN/Tutorial/Settings")]
		static void SelectSettingTutorialConfig()
		{
			Master.CreateAndSelectAssetInResource<TutorialSettings>(AssetName);
		}
#endif
	}
}