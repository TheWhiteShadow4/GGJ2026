using UnityEngine.Events;
using System;


namespace TWS.Settings
{
	/// <summary>
	/// Generisches Interface für Ganzezahlen Value Provider.
	/// Alle UI Klassen aus dem Package verwenden dieses Interface.
	/// </summary>
	public interface IntValueUI
	{
		public int Value { get; }
		public UnityEvent<int> OnValueChanged { get; }
		public void SetValue(int value);
		public Func<int, string> Formatter { get; set; }
	}
}