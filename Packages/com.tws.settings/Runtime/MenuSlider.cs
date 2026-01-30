using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

namespace TWS.Settings
{
	/// <summary>
	/// Basisklasse für alle Settings (Audio, Grafik, Schwierigkeit, etc.)
	/// </summary>
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(Slider))]
	public class MenuSlider : MonoBehaviour, IntValueUI
	{
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private string formatString = "{0}";

		protected Slider slider;

		public UnityEvent<int> OnValueChanged => onValueChanged;
		public Func<int, string> Formatter { get; set; }

		private UnityEvent<int> onValueChanged = new UnityEvent<int>();

		public int Value => (int)slider.value;

		public void SetValue(int value)
		{
			slider.value = value;
			if (valueText != null)
			{
				if (Formatter != null)
					this.valueText.text = Formatter(value);
				else
					this.valueText.text = string.Format(formatString, value.ToString());
			}
		}

		void Awake()
		{
			slider = GetComponent<Slider>();
			slider.onValueChanged.AddListener(OnSliderValueChanged);
		}
		
		void OnSliderValueChanged(float sliderValue)
		{
			int intValue = (int)sliderValue;
			onValueChanged.Invoke(intValue);
			if (valueText != null)
			{
				if (Formatter != null)
					this.valueText.text = Formatter(intValue);
				else
					this.valueText.text = string.Format(formatString, sliderValue.ToString());
			}
		}
	}
} 