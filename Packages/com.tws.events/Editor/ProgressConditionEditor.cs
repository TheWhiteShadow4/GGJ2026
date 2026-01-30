using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace TWS.Events
{
	[CustomEditor(typeof(ProgressCondition))]
	public class ProgressConditionEditor : Editor
	{
		private SerializedProperty descriptionProp;
		private SerializedProperty activateObjectProp;
		private SerializedProperty activateComponentProp;
		private SerializedProperty callEventOnLoadProp;
		private SerializedProperty testWhenAlreadyActivatedProp;
		private SerializedProperty conditionsProp;
		private SerializedProperty onTriggeredProp;
		private ReorderableList conditionsList;

		private void OnEnable()
		{
			descriptionProp = serializedObject.FindProperty("description");
			activateObjectProp = serializedObject.FindProperty("activateObject");
			activateComponentProp = serializedObject.FindProperty("activateComponent");
			callEventOnLoadProp = serializedObject.FindProperty("callEventOnLoad");
			testWhenAlreadyActivatedProp = serializedObject.FindProperty("testWhenAlreadyActivated");
			conditionsProp = serializedObject.FindProperty("conditions");
			onTriggeredProp = serializedObject.FindProperty("onTriggered");

			conditionsList = new ReorderableList(serializedObject, conditionsProp);
			conditionsList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, conditionsProp.displayName);
			conditionsList.elementHeightCallback = (index) => ConditionTestHelper.GetHeight();
			conditionsList.drawElementCallback = (rect, index, isActive, isFocused) => 
			{
				var element = conditionsProp.GetArrayElementAtIndex(index);
				ConditionTestHelper.DrawConditionTest(rect, element);
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(descriptionProp);
			
			conditionsList.DoLayoutList();

			EditorGUILayout.PropertyField(activateObjectProp);
			EditorGUILayout.PropertyField(activateComponentProp);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(testWhenAlreadyActivatedProp);
			EditorGUILayout.PropertyField(callEventOnLoadProp);
			EditorGUILayout.PropertyField(onTriggeredProp);

			serializedObject.ApplyModifiedProperties();

			// Repaint im Play Mode für die Farbaktualisierung
			if (Application.isPlaying)
			{
				Repaint();
			}
		}
	}
}