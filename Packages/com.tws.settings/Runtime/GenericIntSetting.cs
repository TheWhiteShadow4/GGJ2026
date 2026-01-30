using UnityEngine;

namespace TWS.Settings
{
	/// <summary>
	/// Generische Settings Klasse für Integer.
	/// Speichert den Wert in PlayerPrefs und aktualisiert die UI.
	/// </summary>
	public class GenericIntSetting : MonoBehaviour, ISetting
	{
		public string playerPrefKey;

		public int defaultValue = 10;
		
		private int value;

		private IntValueUI ui;

		public int Value => value;

		public IntValueUI UI
		{
			get 
			{
				if (ui == null) Awake();
				return ui;
			}
		}

		protected virtual void Awake()
		{
			ui = GetComponent<IntValueUI>();
			value = PlayerPrefs.GetInt(playerPrefKey, defaultValue);
		}

		public void InitValue()
		{
			value = PlayerPrefs.GetInt(playerPrefKey, defaultValue);
			ValueChanged(value);
		}

		void OnEnable()
		{
			ui.OnValueChanged.AddListener(ValueChanged);
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
			PlayerPrefs.SetInt(playerPrefKey, value);
			Apply(this.value);
		}

		// Kann von Unterklassen überschrieben werden für eigene Implementierungen.
		protected virtual void Apply(int value) {}
	}
}
