using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
#if TWS_LOCALISATION
using TWS.Localization;
#endif
using System;

namespace TWS.Settings
{
	[AddComponentMenu("UI/Karussel", 69)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class Karussel : Selectable, IntValueUI
	{
		#if TWS_LOCALISATION
		public LString[] values = new LString[0];
		#else
		public string[] values = new string[0];
		#endif
		[SerializeField] private int currentIndex = 0;
		public bool loop = true;

		[Space]
		public TextMeshProUGUI valueText;
		public KarusselButton leftArrow;
		public KarusselButton rightArrow;

		[SerializeField] private UnityEvent<int> onValueChanged;

		public UnityEvent<int> OnValueChanged => onValueChanged;
		public Func<int, string> Formatter { get; set; }

		public int Value => currentIndex;

		protected override void Start()
		{
			base.Start();
			leftArrow.karussel = this;
			rightArrow.karussel = this;
			leftArrow.isLeft = true;
			rightArrow.isLeft = false;
			leftArrow.disabled = !loop && currentIndex == 0;
			rightArrow.disabled = !loop && currentIndex == values.Length - 1;
		}

		public void SetValue(int value)
		{
			SetValue(value, true);
		}

		public void SetValue(int value, bool sendCallback)
		{
			currentIndex = Mathf.Clamp(value, 0, values.Length - 1);
			UpdateUI();
			if (sendCallback)
			{
				onValueChanged.Invoke(currentIndex);
			}
		}

		void UpdateUI()
		{
			if (values.Length == 0)
			{
				rightArrow.disabled = true;
				leftArrow.disabled = true;
				return;
			}
			rightArrow.disabled = !loop && currentIndex == values.Length - 1;
			leftArrow.disabled = !loop && currentIndex == 0;

			if (valueText != null)
			{
				if (Formatter != null)
					valueText.text = Formatter(currentIndex);
				else
					valueText.text = values[currentIndex];
			}
		}

		internal void OnLeftArrowClick()
		{
			if (values.Length > 0 && (currentIndex > 0 || loop))
			{
				currentIndex = (currentIndex + values.Length - 1) % values.Length;
			}
			UpdateUI();
			onValueChanged.Invoke(currentIndex);
		}

		internal void OnRightArrowClick()
		{
			if (values.Length > 0 && (currentIndex < values.Length - 1 || loop))
			{
				currentIndex = (currentIndex + 1) % values.Length;
			}
			UpdateUI();
			onValueChanged.Invoke(currentIndex);
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			if (valueText != null && currentIndex >= 0 && currentIndex < values.Length)
			{
				if (Formatter != null)
					valueText.text = Formatter(currentIndex);
				else
					valueText.text = values[currentIndex];
			}
		}
#endif

		public override void OnMove(AxisEventData eventData)
		{
			if (eventData.moveDir == MoveDirection.Left)
			{
				OnLeftArrowClick();
			}
			else if (eventData.moveDir == MoveDirection.Right)
			{
				OnRightArrowClick();
			}
			else
			{
				base.OnMove(eventData);
			}
		}
	}
}