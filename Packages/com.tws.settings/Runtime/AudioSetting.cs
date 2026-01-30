using UnityEngine;
using UnityEngine.Audio;

namespace TWS.Settings
{
	/// <summary>
	/// Audio Settings Klasse für Lautstärke-Regelung
	/// Mappt 0-100 Werte zu -80 bis 0 dB für AudioMixer
	/// </summary>
	public class AudioSetting : MonoBehaviour, ISetting
	{
		[SerializeField] protected string playerPrefKey;

		[Header("Audio Settings")]
		[SerializeField] private AudioMixer audioMixer;

		[SerializeField] private int defaultValue = 0;
		
		private const int DB_MIN = -80;
		private const int DB_MAX = 20;
		private const int UI_MIN = 0;
		private const int UI_MAX = 100;

		private int value;

		private IntValueUI ui;

		void Awake()
		{
			ui = GetComponent<IntValueUI>();
			value = PlayerPrefs.GetInt(playerPrefKey, defaultValue);
		}

		public void InitValue()
		{
			value = PlayerPrefs.GetInt(playerPrefKey, defaultValue);
			float logicalValue = (value - UI_MIN) * (DB_MAX - DB_MIN) / (UI_MAX - UI_MIN) + DB_MIN;
			audioMixer.SetFloat(playerPrefKey, logicalValue);
		}

		void OnEnable()
		{
			ui.OnValueChanged.AddListener(ValueChanged);
			ui.Formatter = value => string.Format("{0}%", value);
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
			float logicalValue = (value - UI_MIN) * (DB_MAX - DB_MIN) / (UI_MAX - UI_MIN) + DB_MIN;
			audioMixer.SetFloat(playerPrefKey, logicalValue);
			PlayerPrefs.SetInt(playerPrefKey, value);
		}
	}
} 