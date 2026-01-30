using UnityEngine;

namespace TWS.Settings
{
	// Quality Einstellung basierend auf den Unity Quality Levels.
	public class QualitySetting : MonoBehaviour
	{
		[SerializeField] protected string playerPrefKey;
		public bool useQualityLevelNames = false;

		private int value;

		private IntValueUI ui;

		void Awake()
		{
			ui = GetComponent<IntValueUI>();
			value = PlayerPrefs.GetInt(playerPrefKey, QualitySettings.GetQualityLevel());
		}

		void OnEnable()
		{
			ui.OnValueChanged.AddListener(ValueChanged);
			if (useQualityLevelNames)
				ui.Formatter = value => QualitySettings.names[value];
			ui.SetValue(value);
			ValueChanged(value);
		}

		void OnDisable()
		{
			ui.OnValueChanged.RemoveListener(ValueChanged);
		}

		public void ValueChanged(int value)
		{
			this.value = value;
			QualitySettings.SetQualityLevel(value, true);
			PlayerPrefs.SetInt(playerPrefKey, value);
		}
	}
}