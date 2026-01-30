using UnityEngine;

namespace TWS.Settings
{
	// Auflösungseinstellung basierend auf den Unity Screen.resolutions.
	public class ResolutionSetting : MonoBehaviour
	{
		[SerializeField] protected string playerPrefKey;

		public Vector2Int[] resolutions;

		private int value;

		private IntValueUI ui;

		void Awake()
		{
			ui = GetComponent<IntValueUI>();
			value = PlayerPrefs.GetInt(playerPrefKey, -1);
			if (value == -1)
			{
				var res = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
				for (int i = 0; i < resolutions.Length; i++)
				{
					if (resolutions[i].y == res.y)
					{
						value = i;
						break;
					}
				}
			}
		}

		void OnEnable()
		{
			ui.OnValueChanged.AddListener(ValueChanged);
			ui.Formatter = value => 
			{
				var res = GetResolution(value);
				return $"{res.x}x{res.y}";
			};
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
			var res = GetResolution(value);
			Screen.SetResolution(res.x, res.y, Screen.fullScreen);
			PlayerPrefs.SetInt(playerPrefKey, value);
		}

		private Vector2Int GetResolution(int value)
		{
			try
			{
				return resolutions[value];
			}
			catch (System.Exception)
			{
				return resolutions[0];
			}
		}
	}
}