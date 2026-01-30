using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TWS.Events
{
	public class ProgressVariablesTreeView : TreeView<int>
	{
		private readonly SerializedObject serializedObject;
		private readonly SerializedProperty variablesProp;

		private readonly Dictionary<string, TreeViewItem<int>> pathToFolder = new Dictionary<string, TreeViewItem<int>>();
		private int nextId = 1;
		private const float TYPE_COLUMN_WIDTH = 160f;
		
		// Tracking für aufgeklappte customNames
		private HashSet<int> expandedCustomNames = new HashSet<int>();

		public System.Action RequestRepaint;

		public ProgressVariablesTreeView(TreeViewState<int> state, SerializedObject so, SerializedProperty variablesProp)
			: base(state)
		{
			this.serializedObject = so;
			this.variablesProp = variablesProp;
			showAlternatingRowBackgrounds = true;
			showBorder = true;
			Reload();
		}

		protected override TreeViewItem<int> BuildRoot()
		{
			pathToFolder.Clear();
			nextId = 1;

			var root = new TreeViewItem<int> { id = 0, depth = -1, displayName = "Root" };

			for (int i = 0; i < variablesProp.arraySize; i++)
			{
				var element = variablesProp.GetArrayElementAtIndex(i);
				var nameProp = element.FindPropertyRelative("name");
				string fullPath = string.IsNullOrEmpty(nameProp.stringValue) ? $"Variable {i}" : nameProp.stringValue;
				string[] segments = fullPath.Split('/');

				TreeViewItem<int> parent = root;
				string currentPath = string.Empty;
				for (int s = 0; s < segments.Length - 1; s++)
				{
					string seg = string.IsNullOrWhiteSpace(segments[s]) ? "(Empty)" : segments[s].Trim();
					currentPath = string.IsNullOrEmpty(currentPath) ? seg : $"{currentPath}/{seg}";
					if (!pathToFolder.TryGetValue(currentPath, out var folder))
					{
						folder = new TreeViewItem<int> { id = nextId++, depth = parent.depth + 1, displayName = seg };
						if (parent.children == null) parent.children = new List<TreeViewItem<int>>();
						parent.AddChild(folder);
						pathToFolder[currentPath] = folder;
					}
					parent = folder;
				}

				string leafName = segments.Length > 0 ? segments[segments.Length - 1] : fullPath;
				var item = new VariableItem
				{
					id = nextId++,
					depth = parent.depth + 1,
					displayName = string.IsNullOrWhiteSpace(leafName) ? "(Unnamed)" : leafName,
					Index = i,
					FullPath = fullPath
				};
				if (parent.children == null) parent.children = new List<TreeViewItem<int>>();
				parent.AddChild(item);
			}

			// Ensure root has children list
			if (root.children == null) root.children = new List<TreeViewItem<int>>();

			SetupDepthsFromParentsAndChildren(root);
			return root;
		}

		protected override bool DoesItemMatchSearch(TreeViewItem<int> item, string search)
		{
			if (string.IsNullOrEmpty(search)) return true;
			string s = search.ToLowerInvariant();
			if (item is VariableItem vi)
			{
				return vi.FullPath != null && vi.FullPath.ToLowerInvariant().Contains(s);
			}
			return item.displayName != null && item.displayName.ToLowerInvariant().Contains(s);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			var rowRect = args.rowRect;
			if (args.item is VariableItem vi)
			{
				float typeWidth = TYPE_COLUMN_WIDTH;
				float indexWidth = 50f;
				float foldoutWidth = 16f;
				float indent = GetContentIndent(args.item);
				
				var element = variablesProp.GetArrayElementAtIndex(vi.Index);
				var nameProp = element.FindPropertyRelative("name");
				var typeProp = element.FindPropertyRelative("type");
				var customNamesProp = element.FindPropertyRelative("customNames");
				var ownerIdProp = element.FindPropertyRelative("ownerId");

				bool hasCustomNames = !string.IsNullOrEmpty(customNamesProp.stringValue);
				bool isExpanded = expandedCustomNames.Contains(vi.Index);

				// Erste Zeile: Variable selbst
				var foldoutRect = new Rect(rowRect.x + indent, rowRect.y, foldoutWidth, EditorGUIUtility.singleLineHeight);
				var indexRect = new Rect(rowRect.x + indent + foldoutWidth, rowRect.y, indexWidth - foldoutWidth, EditorGUIUtility.singleLineHeight);
				var nameRect = new Rect(rowRect.x + indent + indexWidth, rowRect.y, rowRect.width - indent - indexWidth - typeWidth - 8f, EditorGUIUtility.singleLineHeight);
				var typeRect = new Rect(rowRect.x + rowRect.width - typeWidth, rowRect.y, typeWidth, EditorGUIUtility.singleLineHeight);

				// Foldout für customNames
				if (hasCustomNames)
				{
					EditorGUI.BeginChangeCheck();
					bool newExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, GUIContent.none, true);
					if (EditorGUI.EndChangeCheck())
					{
						if (newExpanded)
							expandedCustomNames.Add(vi.Index);
						else
							expandedCustomNames.Remove(vi.Index);
						
						// TreeView muss Höhen neu berechnen
						RefreshCustomRowHeights();
						RequestRepaint?.Invoke();
					}
				}

				EditorGUI.LabelField(indexRect, $"{vi.Index}:");

				EditorGUI.BeginChangeCheck();
				string newName = EditorGUI.DelayedTextField(nameRect, nameProp.stringValue);
				if (EditorGUI.EndChangeCheck())
				{
					nameProp.stringValue = newName;
					serializedObject.ApplyModifiedProperties();
					Reload();
					RequestRepaint?.Invoke();
				}

				EditorGUI.BeginChangeCheck();
				var oldType = (ProgressVariableType)typeProp.enumValueIndex;
				EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
				if (EditorGUI.EndChangeCheck())
				{
					var newType = (ProgressVariableType)typeProp.enumValueIndex;
					// Wenn zu Bitset gewechselt wird, initialisiere mit leeren Namen
					if (oldType != ProgressVariableType.Bitset && newType == ProgressVariableType.Bitset)
					{
						if (string.IsNullOrEmpty(customNamesProp.stringValue))
						{
							customNamesProp.stringValue = "Bit 0,Bit 1,Bit 2,Bit 3";
						}
					}
					serializedObject.ApplyModifiedProperties();
					Reload();
					RequestRepaint?.Invoke();
				}

				// Zweite Zeile: CustomNames (wenn ausgeklappt)
				if (isExpanded && hasCustomNames)
				{
					const float lineSpacing = 4f;
					float customNamesY = rowRect.y + EditorGUIUtility.singleLineHeight + lineSpacing;
					float customNamesIndent = indent + 20f;
					var customNamesRect = new Rect(rowRect.x + customNamesIndent, customNamesY, 
						rowRect.width - customNamesIndent - 8f, EditorGUIUtility.singleLineHeight);

					bool hasOwner = !string.IsNullOrEmpty(ownerIdProp.stringValue);
					
					if (hasOwner)
					{
						// Read-only mit Owner-Info
						EditorGUI.BeginDisabledGroup(true);
						EditorGUI.TextField(customNamesRect, customNamesProp.stringValue);
						EditorGUI.EndDisabledGroup();
						
						// Owner-Label darunter
						var ownerLabelRect = new Rect(rowRect.x + customNamesIndent, customNamesY + EditorGUIUtility.singleLineHeight + lineSpacing, 
							rowRect.width - customNamesIndent - 8f, EditorGUIUtility.singleLineHeight);
						EditorGUI.LabelField(ownerLabelRect, $"🔒 Owned by: {ownerIdProp.stringValue}", EditorStyles.miniLabel);
					}
					else
					{
						// Editierbar
						EditorGUI.BeginChangeCheck();
						string newCustomNames = EditorGUI.TextField(customNamesRect, customNamesProp.stringValue);
						if (EditorGUI.EndChangeCheck())
						{
							customNamesProp.stringValue = newCustomNames;
							serializedObject.ApplyModifiedProperties();
							RequestRepaint?.Invoke();
						}
					}
				}

				// Separator zeichnen, wenn letztes Kind im Ordner
				if (args.item.parent != null && args.item.parent.children != null && args.item.parent.children.Count > 0)
				{
					var last = args.item.parent.children[args.item.parent.children.Count - 1];
					if (last == args.item)
					{
						var sepRect = new Rect(rowRect.x + indent, rowRect.yMax + 1f, rowRect.width - indent, 1f);
						EditorGUI.DrawRect(sepRect, new Color(0f, 0f, 0f, 0.2f));
					}
				}
			}
			else
			{
				base.RowGUI(args);
			}
		}

		protected override float GetCustomRowHeight(int row, TreeViewItem<int> item)
		{
			if (item is VariableItem vi)
			{
				var element = variablesProp.GetArrayElementAtIndex(vi.Index);
				var customNamesProp = element.FindPropertyRelative("customNames");
				var ownerIdProp = element.FindPropertyRelative("ownerId");
				
				bool hasCustomNames = !string.IsNullOrEmpty(customNamesProp.stringValue);
				bool isExpanded = expandedCustomNames.Contains(vi.Index);
				bool hasOwner = !string.IsNullOrEmpty(ownerIdProp.stringValue);

				if (isExpanded && hasCustomNames)
				{
					const float lineSpacing = 4f;
					// Basis-Zeile + Spacing + customNames Zeile
					float height = EditorGUIUtility.singleLineHeight + lineSpacing + EditorGUIUtility.singleLineHeight;
					if (hasOwner)
					{
						// + Spacing + Owner-Label Zeile
						height += lineSpacing + EditorGUIUtility.singleLineHeight;
					}
					return height;
				}
			}
			return EditorGUIUtility.singleLineHeight;
		}

		public void AddVariable()
		{
			int newIndex = variablesProp.arraySize;
			variablesProp.arraySize++;
			var element = variablesProp.GetArrayElementAtIndex(newIndex);
			element.FindPropertyRelative("name").stringValue = $"New Variable {newIndex}";
			element.FindPropertyRelative("type").enumValueIndex = (int)ProgressVariableType.Number;
			serializedObject.ApplyModifiedProperties();
			Reload();
		}

		public void RemoveSelected()
		{
			var selection = GetSelection();
			if (selection == null || selection.Count == 0) return;

			// collect indices to delete
			List<int> indices = new List<int>();
			foreach (int id in selection)
			{
				if (FindItem(id, rootItem) is VariableItem vi)
				{
					indices.Add(vi.Index);
				}
			}
			indices.Sort();
			indices.Reverse();
			foreach (int idx in indices)
			{
				variablesProp.DeleteArrayElementAtIndex(idx);
			}
			serializedObject.ApplyModifiedProperties();
			Reload();
		}


		public int GetVisibleRowCount()
		{
			var rows = GetRows();
			return rows != null ? rows.Count : 0;
		}

		private class VariableItem : TreeViewItem<int>
		{
			public int Index;
			public string FullPath;
		}
	}
} 