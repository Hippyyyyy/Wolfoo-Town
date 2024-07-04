using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SCN.Common;

namespace SCN.Draw
{
    public class DemoManager : MonoBehaviour
    {
        [SerializeField] DrawTex drawTexIns;

		[SerializeField] BrushStats brush0;
		[SerializeField] BrushStats brush1;
		[SerializeField] BrushStats brush2;

		[SerializeField] EventTrigger eventTrigger;

		private void Awake()
		{
			Master.AddEventTriggerListener(eventTrigger, EventTriggerType.PointerDown
				, data =>
				{
					drawTexIns.StartDrawSession();
				});

			Master.AddEventTriggerListener(eventTrigger, EventTriggerType.Drag
				, data =>
				{
					drawTexIns.UpdateDraw();
				});
		}

		private void Start()
		{
			drawTexIns.Setup(new Vector2(1000, 800), brush0);
			drawTexIns.SetBrushSize(0.3f);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				drawTexIns.SetBrush(brush0);
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
				drawTexIns.SetBrush(brush1);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				drawTexIns.SetBrush(brush2);
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				drawTexIns.SetColor(Color.red);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				drawTexIns.SetColor(Color.yellow);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				drawTexIns.SetColor(Color.green);
			}
		}
	}
}