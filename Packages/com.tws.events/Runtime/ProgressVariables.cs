using UnityEngine;

namespace TWS.Events
{
	/// <summary>
	/// Schnittstelle für Objekte, die Bit-Namen für Bitset-Variablen bereitstellen.
	/// Ermöglicht flexible Datenquellen (z.B. ProgressVariables selbst oder Dialog-Graphs).
	/// Verwendet Count + GetAt Pattern für allocation-freie Iteration.
	/// </summary>
	public interface IBitNameProvider
	{
		/// <summary>
		/// Gibt die Anzahl der benannten Bits zurück.
		/// </summary>
		int GetBitNameCount();

		/// <summary>
		/// Gibt den Namen eines Bits am angegebenen Index zurück.
		/// </summary>
		string GetBitName(int index);

		/// <summary>
		/// Gibt den Index eines Bits anhand seines Namens zurück, oder -1 wenn nicht gefunden.
		/// </summary>
		int GetBitIndex(string bitName);
	}

	/// <summary>
	/// Dieses ScriptableObject enthält die Namen der Variablen, die in der GameProgress gespeichert werden.
	/// Die Namen sind nur für den Editor relevant.
	/// </summary>
	public class ProgressVariables : ScriptableObject
	{

		public string[] names;
		public VariableInfo[] variables;

		public int Length => variables.Length;

		public VariableInfo Get(int index)
		{
			if (index < 0 || index >= variables.Length) return new VariableInfo();
			return variables[index];
		}

		public int IndexOf(string name)
		{
			for (int i = 0; i < variables.Length; i++)
			{
				if (variables[i].name == name) return i;
			}
			return -1;
		}

		/// <summary>
		/// Gibt den IBitNameProvider für eine Variable zurück, falls es sich um ein Bitset handelt.
		/// </summary>
		public IBitNameProvider GetBitNameProvider(int index)
		{
			if (index < 0 || index >= variables.Length) return null;
			if (variables[index].type != ProgressVariableType.Bitset) return null;
			return new BitNameProviderFromString(variables[index].customNames);
		}
	}

	[System.Serializable]
	public class VariableInfo
	{
		public string name;
		public ProgressVariableType type;
		public string customNames;
		
		/// <summary>
		/// Optionale ID des externen Besitzers dieser customNames (z.B. Dialog Graph GUID).
		/// Wenn gesetzt, sind die customNames read-only im Editor.
		/// Verhindert, dass mehrere Dialoge dieselbe Variable mit unterschiedlichen Namen nutzen.
		/// </summary>
		public string ownerId;

		/// <summary>
		/// Gibt die Bit-Namen als Array zurück (für Bitset-Variablen).
		/// </summary>
		public string[] GetBitNamesArray()
		{
			if (string.IsNullOrEmpty(customNames)) return new string[0];
			return customNames.Split(',');
		}

		/// <summary>
		/// Setzt die Bit-Namen aus einem Array (für Bitset-Variablen).
		/// </summary>
		public void SetBitNamesArray(string[] bitNames)
		{
			if (bitNames == null || bitNames.Length == 0)
			{
				customNames = string.Empty;
				return;
			}
			customNames = string.Join(",", bitNames);
		}
		
		/// <summary>
		/// Prüft ob die customNames von einem externen Besitzer kontrolliert werden.
		/// </summary>
		public bool HasOwner => !string.IsNullOrEmpty(ownerId);
	}

	public enum ProgressVariableType
	{
		Number,		// Numerischer Wert
		Flag,		// 0/1 Werte
		Decision,	// 0=Offen, 1=Ja, -1=Nein
		Quest,		// 0=Nicht gestartet, 1-100=Aktiv, 101=Abgegeben, -1=Fehlgeschlagen, -2=Abgebrochen
		Bitset,		// Bitweise Operationen
		Custom		// Für spezielle Fälle
	}

	/// <summary>
	/// Implementierung von IBitNameProvider die Namen aus einem kommagetrennten String liest.
	/// </summary>
	public class BitNameProviderFromString : IBitNameProvider
	{
		private string[] bitNames;

		public BitNameProviderFromString(string commaseparated)
		{
			if (string.IsNullOrEmpty(commaseparated))
			{
				bitNames = new string[0];
			}
			else
			{
				bitNames = commaseparated.Split(',');
				// Trimmen für saubere Namen
				for (int i = 0; i < bitNames.Length; i++)
				{
					bitNames[i] = bitNames[i].Trim();
				}
			}
		}

		public int GetBitNameCount()
		{
			return bitNames.Length;
		}

		public string GetBitName(int index)
		{
			if (index < 0 || index >= bitNames.Length) return string.Empty;
			return bitNames[index];
		}

		public int GetBitIndex(string bitName)
		{
			for (int i = 0; i < bitNames.Length; i++)
			{
				if (bitNames[i] == bitName) return i;
			}
			return -1;
		}
	}
}