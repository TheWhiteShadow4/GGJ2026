namespace TWS.Events
{	
	[System.Serializable]
	public struct ConditionTest
	{
		public bool active;
		[ProgressVariable]
		public int variable;
		public int value;
		public ConditionType type;

		public bool Test()
		{
			if (!active) return true;
			if (variable == -1) return false;
			int ist = GameProgress.Current.GetVariable(variable);
			int soll = value;
			switch (type)
			{
				case ConditionType.Gleich: 			return ist == soll;
				case ConditionType.Ungleich: 		return ist != soll;
				case ConditionType.Größer: 			return ist > soll;
				case ConditionType.Kleiner: 		return ist < soll;
				case ConditionType.GrößerGleich: 	return ist >= soll;
				case ConditionType.KleinerGleich: 	return ist <= soll;
				case ConditionType.Bits: 			return (soll == 0 && ist == 0) || (ist & soll) == soll;
				default: return false;
			}
		}
	}

	public enum ConditionType
	{
		Gleich,
		Ungleich,
		Größer,
		Kleiner,
		GrößerGleich,
		KleinerGleich,
		Bits
	}
}