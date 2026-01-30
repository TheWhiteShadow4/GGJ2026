
namespace TWS.Events
{
	[System.Serializable]
	public struct ProgressEvent
	{
		[ProgressVariable]
		public int variable;
		public int value;
	}
}