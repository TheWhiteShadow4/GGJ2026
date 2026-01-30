using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

namespace TWS.Settings
{
	/// <summary>
	/// Basisklasse für binäre Settings als Integer (0 = Aus, 1 = Ein).
	/// </summary>
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(Toggle))]
	public class MenuToggle : MonoBehaviour, IntValueUI
	{
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private string offText = "Aus";
		[SerializeField] private string onText = "Ein";

		protected Toggle toggle;

		public UnityEvent<int> OnValueChanged => onValueChanged;
		public Func<int, string> Formatter { get; set; }

		private UnityEvent<int> onValueChanged = new UnityEvent<int>();

		public int Value => toggle.isOn ? 1 : 0;

		public void SetValue(int value)
		{
			toggle.isOn = value > 0;
			if (valueText != null)
			{
				if (Formatter != null)
					this.valueText.text = Formatter(value);
				else
					this.valueText.text = value > 0 ? onText : offText;
			}
		}
		
		void Awake()
		{
			toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(OnToggleValueChanged);
		}
		
		void OnToggleValueChanged(bool value)
		{
			int intValue = value ? 1 : 0;
			onValueChanged.Invoke(intValue);
			if (valueText != null)
			{
				if (Formatter != null)
					this.valueText.text = Formatter(intValue);
				else
					this.valueText.text = value ? onText : offText;
			}
		}
	}
} 