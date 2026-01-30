using UnityEngine;
using UnityEditor;

namespace TWS.Events
{
	[CustomPropertyDrawer(typeof(ConditionTest))]
	public class ConditionTestDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ConditionTestHelper.DrawConditionTest(position, property);
		}
	}
}