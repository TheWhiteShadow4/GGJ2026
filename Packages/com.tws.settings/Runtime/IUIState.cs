
namespace TWS.Settings
{
	public interface IUIState
	{
		public bool Disabled { get; set; }

		// Ausgewählt
		public bool Selected { get; set; }

		// Aktiviert oder gedrückt
		public bool Active { get; set; }
	}
}