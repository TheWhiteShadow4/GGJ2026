using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TWS.Events
{
	public class ProgressVariablesWindow : EditorWindow
	{
		private ProgressVariables asset;
		private SerializedObject serializedObject;
		private SerializedProperty variablesProp;

		private TreeViewState<int> treeViewState;
		private SearchField searchField;
		private ProgressVariablesTreeView treeView;
		private string searchText = string.Empty;

		[MenuItem("TWS/Progress Variables")] 
		public static void Open()
		{
			var window = GetWindow<ProgressVariablesWindow>("Progress Variables");
			window.Show();
		}

		public static void ShowWindow(ProgressVariables targetAsset)
		{
			var window = GetWindow<ProgressVariablesWindow>("Progress Variables");
			window.asset = targetAsset;
			window.InitFromAsset();
			window.Show();
		}

		private void OnEnable()
		{
			if (searchField == null) searchField = new SearchField();
			if (treeViewState == null) treeViewState = new TreeViewState<int>();
			TryFindAssetIfMissing();
			InitFromAsset();
		}

		private void OnSelectionChange()
		{
			if (Selection.activeObject is ProgressVariables pv)
			{
				asset = pv;
				InitFromAsset();
				Repaint();
			}
		}

		private void TryFindAssetIfMissing()
		{
			if (asset != null) return;
			string[] guids = AssetDatabase.FindAssets("t:ProgressVariables");
			if (guids != null && guids.Length > 0)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[0]);
				asset = AssetDatabase.LoadAssetAtPath<ProgressVariables>(path);
			}
		}

		private void InitFromAsset()
		{
			if (asset == null)
			{
				serializedObject = null;
				variablesProp = null;
				treeView = null;
				return;
			}
			serializedObject = new SerializedObject(asset);
			variablesProp = serializedObject.FindProperty("variables");
			treeView = new ProgressVariablesTreeView(treeViewState, serializedObject, variablesProp);
			treeView.RequestRepaint = Repaint;
			treeView.searchString = searchText;
		}

		private void OnGUI()
		{
			DrawHeader();

			if (asset == null)
			{
				EditorGUILayout.HelpBox("Kein ProgressVariables Asset gefunden.", MessageType.Warning);
				using (new EditorGUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Asset erstellen"))
					{
						CreateProgressVariablesAsset();
						InitFromAsset();
					}
				}
				return;
			}

			serializedObject.Update();

			var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
			treeView?.OnGUI(rect);

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawHeader()
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				asset = (ProgressVariables)EditorGUILayout.ObjectField(asset, typeof(ProgressVariables), false, GUILayout.Width(280));
				GUILayout.Space(8);
				GUILayout.Label("Suche:", GUILayout.Width(48));
				searchText = searchField.OnToolbarGUI(searchText);
				if (treeView != null && treeView.searchString != searchText)
				{
					treeView.searchString = searchText;
				}
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(24)))
				{
					treeView?.AddVariable();
				}
				if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(24)))
				{
					treeView?.RemoveSelected();
				}
			}
		}

		private void CreateProgressVariablesAsset()
		{
			var newAsset = ScriptableObject.CreateInstance<ProgressVariables>();
			newAsset.variables = new VariableInfo[] { new VariableInfo() { name = "Variable 0", type = ProgressVariableType.Number, customNames = null } };

			if (!AssetDatabase.IsValidFolder("Assets/Settings"))
			{
				AssetDatabase.CreateFolder("Assets", "Settings");
			}
			string uniquePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Settings/ProgressVariables.asset");
			AssetDatabase.CreateAsset(newAsset, uniquePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			asset = newAsset;
			Selection.activeObject = asset;
		}
	}
} 