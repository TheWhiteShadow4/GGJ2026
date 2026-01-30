using UnityEngine;

namespace TWS.Events
{
	public class ProgressSwitch : MonoBehaviour
	{
		[Header("Debugging")]
		public bool editorOnly = true;

		[Space]
		public ProgressEvent[] variables;


		void OnEnable()
		{
			if (editorOnly && !Application.isEditor)
			{
				enabled = false;
				return;
			}

			foreach (var variable in variables)
			{
				GameProgress.Current.SetVariable(variable.variable, variable.value);
			}
		}
	}
}