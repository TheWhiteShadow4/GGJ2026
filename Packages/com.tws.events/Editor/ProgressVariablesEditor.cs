using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace TWS.Events
{
	[CustomEditor(typeof(ProgressVariables))]
	public class ProgressVariablesEditor : Editor
	{
		private SerializedProperty variablesProp;

		private TreeViewState<int> treeViewState;
		private SearchField searchField;
		private ProgressVariablesTreeView treeView;

		private string searchText = string.Empty;

		private void OnEnable()
		{
			variablesProp = serializedObject.FindProperty("variables");

			if (treeViewState == null) treeViewState = new TreeViewState<int>();
			if (searchField == null) searchField = new SearchField();
			treeView = new ProgressVariablesTreeView(treeViewState, serializedObject, variablesProp);
			treeView.RequestRepaint = Repaint;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawToolbar();

			int rowCount;
			try { rowCount = treeView?.GetVisibleRowCount() ?? 0; } catch { rowCount = 0; }
			float rowHeight = EditorGUIUtility.singleLineHeight + 2f;
			float desiredHeight = Mathf.Clamp(rowCount * rowHeight + 6f, 200f, 700f);
			var rect = GUILayoutUtility.GetRect(0, 100000, desiredHeight, desiredHeight);
			treeView.OnGUI(rect);

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawToolbar()
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				GUILayout.Label("Variablen", EditorStyles.boldLabel, GUILayout.Width(80));
				if (GUILayout.Button("Fenster", EditorStyles.toolbarButton, GUILayout.Width(60)))
				{
					ProgressVariablesWindow.ShowWindow((ProgressVariables)target);
				}
				GUILayout.FlexibleSpace();
				searchText = searchField.OnToolbarGUI(searchText);
				if (treeView.searchString != searchText)
				{
					treeView.searchString = searchText;
				}
				if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(24)))
				{
					treeView.AddVariable();
				}
				if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(24)))
				{
					treeView.RemoveSelected();
				}
			}
		}
	}
}