using UnityEngine;
using UnityEditor;

namespace TWS.Events
{
	[CustomPropertyDrawer(typeof(ProgressVariableAttribute))]
	public class VariableDrawer : PropertyDrawer
	{
		private const string ASSET_PATH = "Assets/Settings/ProgressVariables.asset";
		private const float VALUE_WIDTH = 60f;
		private static ProgressVariables _variables;

		private static System.Action _closeHandler;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.serializedObject.targetObject == null && _closeHandler != null)
			{
				_closeHandler();
				_closeHandler = null;
				return;
			}

			position.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.BeginProperty(position, label, property);

			// Dropdown Button
			int currentIndex = property.intValue;
			if (currentIndex < 0 || _variables == null || currentIndex >= _variables.Length) currentIndex = 0;
			ProgressVariableButton(position, currentIndex, label, (index) =>
			{
				property.intValue = index;
				property.serializedObject.ApplyModifiedProperties();
			});

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + 4f;
		}

		public static ProgressVariables GetProgressVariablesAsset()
		{
			var variables = AssetDatabase.LoadAssetAtPath<ProgressVariables>(ASSET_PATH);
			if (variables == null)
			{
				variables = CreateProgressVariablesAsset();
			}
			return variables;
		}

		public static void ProgressVariableButton(Rect position, int currentIndex, GUIContent label, System.Action<int> onSelect)
		{
			Rect labelRect = position;
			if (label != null && label.text.Length > 0)
			{
				labelRect.width = EditorGUIUtility.labelWidth;
			}
			else
			{
				labelRect.width = 0;
			}

			Rect dropdownRect = position;
			dropdownRect.x += labelRect.width;
			dropdownRect.width = position.width - labelRect.width - VALUE_WIDTH;

			Rect valueRect = position;
			valueRect.x = dropdownRect.x + dropdownRect.width + 4;
			valueRect.width = VALUE_WIDTH - 4;

			// Label links
			if (label != null) EditorGUI.LabelField(labelRect, label);
			
			// Play Mode Wert anzeigen
			if (Application.isPlaying && GameProgress.Current != null)
			{
				EditorGUI.LabelField(valueRect, $"{GameProgress.Current.GetVariable(currentIndex)}");
			}
			else
			{
				EditorGUI.LabelField(valueRect, "-");
			}

			// Asset laden oder Create-Button anzeigen
			if (_variables == null)
			{
				_variables = AssetDatabase.LoadAssetAtPath<ProgressVariables>(ASSET_PATH);
				if (_variables == null)
				{
					if (GUI.Button(position, "ProgressVariables Asset erstellen"))
					{
						_variables = CreateProgressVariablesAsset();
					}
					return;
				}
			}

			VariableInfo info = _variables.Get(currentIndex);
			string name = info.name;
			string currentName = $"{currentIndex}: {name}";
			bool isDropdown = EditorGUI.DropdownButton(dropdownRect, new GUIContent(currentName), FocusType.Keyboard);
			if (isDropdown)
			{
				if (_closeHandler != null)
				{
					_closeHandler();
					_closeHandler = null;
				}

				Rect windowRect = dropdownRect;
				windowRect.y += EditorGUIUtility.singleLineHeight;
				_closeHandler = VariablePopup.Show(windowRect, _variables, currentName, onSelect);
			}
		}

		private static ProgressVariables CreateProgressVariablesAsset()
		{
			var asset = ScriptableObject.CreateInstance<ProgressVariables>();
			asset.variables = new VariableInfo[] { new VariableInfo() { name = "Variable 0", type = ProgressVariableType.Number, customNames = null } };

			if (!AssetDatabase.IsValidFolder("Assets/Settings"))
			{
				AssetDatabase.CreateFolder("Assets", "Settings");
			}
			
			AssetDatabase.CreateAsset(asset, ASSET_PATH);
			AssetDatabase.SaveAssets();
			
			return asset;
		}
	}

	public class VariablePopup : EditorWindow
	{
		private static ProgressVariables _variables;
		private static string _currentName;
		private static System.Action<int> _onSelect;
		private string _searchText = "";
		private Vector2 _scrollPosition;
		private const float WINDOW_WIDTH = 240;
		private const float WINDOW_HEIGHT = 300;
		private const float SEARCH_HEIGHT = 20f;
		private const float ITEM_HEIGHT = 20f;
		private const float CLOSE_BUTTON_WIDTH = 20f;
		private VariableInfo[] _filteredNames;
		private bool _hasCalledCallback;
		private GUIContent _closeButtonContent;

		public static System.Action Show(Rect buttonRect, ProgressVariables variables, string currentName, System.Action<int> onSelect)
		{
			_variables = variables;
			_currentName = currentName;
			_onSelect = onSelect;

			// Position das Fenster unter dem Button
			var windowRect = GUIUtility.GUIToScreenRect(buttonRect);
			var window = CreateInstance<VariablePopup>();
			window.titleContent = new GUIContent("Variablen");
			window.position = new Rect(
				windowRect.x,
				windowRect.y,
				WINDOW_WIDTH,
				WINDOW_HEIGHT
			);
			window._filteredNames = variables.variables;
			window._hasCalledCallback = false;
			window._closeButtonContent = new GUIContent("×");
			window.ShowPopup();
			return () => window.Close();
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			
			// Suchfeld (etwas schmaler für den X-Button)
			GUI.SetNextControlName("SearchField");
			string newSearch = EditorGUILayout.TextField(_searchText, EditorStyles.toolbarSearchField, 
				GUILayout.Width(WINDOW_WIDTH - CLOSE_BUTTON_WIDTH - 4));
			
			// X-Button
			if (GUILayout.Button(_closeButtonContent, EditorStyles.toolbarButton, 
				GUILayout.Width(CLOSE_BUTTON_WIDTH)))
			{
				Close();
			}
			
			EditorGUILayout.EndHorizontal();

			if (newSearch != _searchText)
			{
				_searchText = newSearch;
				UpdateFilteredNames();
			}

			if (Event.current.type == EventType.Repaint && GUI.GetNameOfFocusedControl() == "")
			{
				// Fokussiere das Suchfeld beim Öffnen
				EditorGUI.FocusTextInControl("SearchField");
			}

			// Liste der Namen
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
			
			if (_filteredNames != null)
			{
				for (int i = 0; i < _filteredNames.Length; i++)
				{
					bool isSelected = _filteredNames[i].name == _currentName;
					
					// Hover Style
					var rect = EditorGUILayout.GetControlRect(GUILayout.Height(ITEM_HEIGHT));
					if (rect.Contains(Event.current.mousePosition))
					{
						EditorGUI.DrawRect(rect, new Color(0.7f, 0.7f, 0.7f, 0.3f));
					}
					
					// Ausgewähltes Item hervorheben
					if (isSelected)
					{
						EditorGUI.DrawRect(rect, new Color(0.3f, 0.5f, 0.9f, 0.3f));
					}

					// Klick-Handler
					if (GUI.Button(rect, _filteredNames[i].name, EditorStyles.label))
					{
						int index = _variables.IndexOf(_filteredNames[i].name);
						if (index != -1) _onSelect?.Invoke(index);
						_hasCalledCallback = true;
						Close();
					}
				}
			}

			EditorGUILayout.EndScrollView();

			// Schließen bei Escape
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
			{
				Close();
				Event.current.Use();
			}

			// Schließen bei Klick außerhalb
			if (Event.current.type == EventType.MouseDown && !position.Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
			{
				Close();
				Event.current.Use();
			}
		}

		private void UpdateFilteredNames()
		{
			if (string.IsNullOrEmpty(_searchText))
			{
				_filteredNames = _variables.variables;
				return;
			}

			_filteredNames = System.Array.FindAll(_variables.variables, 
				info => info.name.ToLower().Contains(_searchText.ToLower()));
		}
	}
}