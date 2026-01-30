using UnityEngine;
using UnityEngine.Events;

namespace TWS.Events
{
	public enum AggregatorOperation
	{
		Set,		// Setzt den Wert direkt
		Add,		// Addiert den Wert
		Subtract,	// Subtrahiert den Wert
		Multiply,   // Multipliziert den Wert
		Divide,     // Dividiert den Wert
		Modulo,     // Moduliert den Wert
	}

	[DefaultExecutionOrder(-110)]
	public class ProgressAggregator : MonoBehaviour
	{
		[ProgressVariable]
		public int targetVariable;
		public bool testRepeatedly = true;
		public int operationValue = 1;
		public AggregatorOperation operation = AggregatorOperation.Set;
		public ConditionSet conditions = new ConditionSet();

		private bool wasTrue = false;

		void Start()
		{
			GameProgress.Current.OnVariableChanged += OnVariableChanged;
			// Initial-Test
			int val = GameProgress.Current.GetVariable(targetVariable);
			OnVariableChanged(new EventData(targetVariable, val, val));
		}

		private void OnVariableChanged(EventData data)
		{
			bool isTrue = conditions.Test();

			// Wenn wir nicht wiederholt testen und der Zustand sich nicht geändert hat
			if (!testRepeatedly && isTrue == wasTrue) return;
			
			wasTrue = isTrue;
			
			if (isTrue)
			{
				int currentValue = GameProgress.Current.GetVariable(targetVariable);
				int newValue = currentValue;

				switch (operation)
				{
					case AggregatorOperation.Set:
						newValue = operationValue;
						break;
					case AggregatorOperation.Add:
						newValue = currentValue + operationValue;
						break;
					case AggregatorOperation.Subtract:
						newValue = currentValue - operationValue;
						break;
					case AggregatorOperation.Multiply:
						newValue = currentValue * operationValue;
						break;
					case AggregatorOperation.Divide:
						newValue = currentValue / operationValue;
						break;
					case AggregatorOperation.Modulo:
						newValue = currentValue % operationValue;
						break;
				}

				GameProgress.Current.SetVariable(targetVariable, newValue);
			}
		}
	}

	[System.Serializable]
	public class ConditionSet
	{
		public enum LinkType { AND, OR }

		public LinkType linkType = LinkType.AND;

		public ConditionSet[] subSets;
		public ConditionTest[] conditions;

		public bool Test()
		{
			if (linkType == LinkType.AND)
			{
				if (subSets != null && subSets.Length > 0)
				{
					foreach (var subSet in subSets)
					{
						if (!subSet.Test()) return false;
					}
				}
				else if (conditions != null)
				{
					foreach (var condition in conditions)
					{
						if (!condition.Test()) return false;
					}
				}
				return true;
			}
			else
			{
				if (subSets != null && subSets.Length > 0)
				{
					foreach (var subSet in subSets)
					{
						if (subSet.Test()) return true;
					}
				}
				else if (conditions != null)
				{
					foreach (var condition in conditions)
					{
						if (condition.Test()) return true;
					}
				}
				return false;
			}
		}
	}
}