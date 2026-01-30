using UnityEditor;
using UnityEngine;

namespace TWS.Events
{
    [CustomEditor(typeof(ProgressAggregator))]
    public class ProgressAggregatorEditor : Editor
    {
		private const int MAX_DEPTH = 2; // 0, 1, 2 = 3 Ebenen
        private const float INDENT_WIDTH = 16f; // Breite der Einrückung
        private const float LINE_WIDTH = 4f; // Breite der vertikalen Linien

        private SerializedProperty targetVariableProp;
        private SerializedProperty rootConditionSetProp;
        
        private void OnEnable()
        {
            targetVariableProp = serializedObject.FindProperty("targetVariable");
            rootConditionSetProp = serializedObject.FindProperty("conditions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Ziel-Variable
            EditorGUILayout.PropertyField(targetVariableProp);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("testRepeatedly"));
            
            var operationProp = serializedObject.FindProperty("operation");
			var operationValueProp = serializedObject.FindProperty("operationValue");
            EditorGUILayout.PropertyField(operationProp);
            EditorGUILayout.PropertyField(operationValueProp, new GUIContent("Wert"));
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bedingungen", EditorStyles.boldLabel);
            
            // Root ConditionSet
            DrawConditionSet(rootConditionSetProp, 0);

            // Aktuelle Werte im Play Mode
            if (Application.isPlaying && GameProgress.Current != null)
            {
                EditorGUILayout.Space();
                DrawRuntimeInfo();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private float GetIndentPosition(int depth)
        {
            return depth * INDENT_WIDTH;
        }

        private void DrawSubSetList(SerializedProperty subSetsProp, int depth)
        {
            float indent = GetIndentPosition(depth);
            float buttonIndent = GetIndentPosition(depth + 1);
            
            EditorGUILayout.BeginVertical();
            
            // Header
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent); // Tiefere Einrückung für den Plus-Button
            
            // Plus-Button nur anzeigen, wenn wir nicht an der maximalen Tiefe sind
            if (depth < MAX_DEPTH)
            {
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    subSetsProp.arraySize++;
                }
            }
            else
            {
                // Optional: Disabled-Hinweis
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Button("Max", GUILayout.Width(35));
                EditorGUI.EndDisabledGroup();
                
                if (Event.current.type == EventType.Repaint)
                {
                    var lastRect = GUILayoutUtility.GetLastRect();
                    EditorGUI.LabelField(lastRect, new GUIContent("", "Maximale Verschachtelungstiefe erreicht"));
                }
            }

            EditorGUILayout.LabelField("Untergruppen", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            // Untergruppen
            for (int i = 0; i < subSetsProp.arraySize; i++)
            {
                // Vertikale Gruppe mit Linie
                Rect groupRect = EditorGUILayout.BeginVertical();
                if (Event.current.type == EventType.Repaint)
                {
                    // Vertikale Linie links von der Gruppe
                    Rect lineRect = new Rect(
                        groupRect.x + indent,
                        groupRect.y,
                        LINE_WIDTH,
                        groupRect.height
                    );
                    EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 1));
                }
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(buttonIndent); // Tiefere Einrückung für den Minus-Button
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    subSetsProp.DeleteArrayElementAtIndex(i);
                    continue;
                }
                EditorGUILayout.EndHorizontal();

                var subSet = subSetsProp.GetArrayElementAtIndex(i);
                DrawConditionSet(subSet, depth + 1);
                
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawConditionSet(SerializedProperty setProperty, int depth)
        {
            float indent = GetIndentPosition(depth);
            
            var linkTypeProp = setProperty.FindPropertyRelative("linkType");
            var subSetsProp = setProperty.FindPropertyRelative("subSets");
            var conditionsProp = setProperty.FindPropertyRelative("conditions");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            
            // Link-Typ (AND/OR)
            EditorGUILayout.PropertyField(linkTypeProp, GUIContent.none, GUILayout.Width(60));

            bool isLeaf = subSetsProp.arraySize == 0;
            
            // Wenn wir an der maximalen Tiefe sind, erzwingen wir Leaf-Modus
            if (depth >= MAX_DEPTH)
            {
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Toggle(true, "Bedingungen", EditorStyles.miniButtonLeft);
                GUILayout.Toggle(false, "Untergruppen", EditorStyles.miniButtonRight);
                EditorGUI.EndDisabledGroup();
                
                // Sicherstellen, dass wir im Leaf-Modus sind
                if (!isLeaf)
                {
                    subSetsProp.ClearArray();
                }
            }
            else
            {
                // Normale Typ-Umschaltung
                if (GUILayout.Toggle(isLeaf, "Bedingungen", EditorStyles.miniButtonLeft) && !isLeaf)
                {
                    // Zu Leaf umwandeln
                    subSetsProp.ClearArray();
                }
                if (GUILayout.Toggle(!isLeaf, "Untergruppen", EditorStyles.miniButtonRight) && isLeaf)
                {
                    // Zu Branch umwandeln
                    conditionsProp.ClearArray();
                    subSetsProp.arraySize = 1; // Eine leere Untergruppe erstellen
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            if (isLeaf)
            {
                // Bedingungen anzeigen (Leaf)
                string title = linkTypeProp.enumValueIndex == 0 ? "UND" : "ODER";
                DrawConditionList(conditionsProp, title, depth);
            }
            else
            {
                // Untergruppen anzeigen (Branch)
                DrawSubSetList(subSetsProp, depth);
            }
        }

        private void DrawConditionList(SerializedProperty conditionsProp, string title, int depth)
        {
            float indent = GetIndentPosition(depth);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            // Header
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                conditionsProp.arraySize++;
            }
            EditorGUILayout.EndHorizontal();

            // Prüfen ob eine Bedingung die Zielvariable verwendet
            bool hasTargetVariableCondition = false;
            for (int i = 0; i < conditionsProp.arraySize; i++)
            {
                var element = conditionsProp.GetArrayElementAtIndex(i);
                var variableProp = element.FindPropertyRelative("variable");
                if (variableProp.intValue == targetVariableProp.intValue)
                {
                    hasTargetVariableCondition = true;
                    break;
                }
            }

            // Warnung anzeigen wenn nötig
            if (hasTargetVariableCondition)
            {
                EditorGUILayout.Space(4);
                EditorGUILayout.HelpBox(
                    "Diese Gruppe enthält eine Bedingung, die die Zielvariable selbst prüft. " +
                    "Dies könnte zu einer Endlosschleife führen, da die Variable sich selbst beeinflusst.", 
                    MessageType.Warning);
                EditorGUILayout.Space(4);
            }

            // Bedingungen
            for (int i = 0; i < conditionsProp.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var element = conditionsProp.GetArrayElementAtIndex(i);
                
                // Rect für die Condition berechnen
                var rect = EditorGUILayout.GetControlRect(false, ConditionTestHelper.GetHeight());
                ConditionTestHelper.DrawConditionTest(rect, element, false);
                
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    conditionsProp.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRuntimeInfo()
        {
            EditorGUILayout.LabelField("Laufzeit-Status:", EditorStyles.boldLabel);
            
            var aggregator = (ProgressAggregator)target;
            int targetValue = GameProgress.Current.GetVariable(aggregator.targetVariable);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ziel-Variable:");
            EditorGUILayout.LabelField(targetValue.ToString());
            EditorGUILayout.EndHorizontal();

            // Rekursiv den Status aller Bedingungen anzeigen
            DrawConditionSetStatus(aggregator.conditions, 0);
        }

        private void DrawConditionSetStatus(ConditionSet set, int depth)
        {
            float indent = GetIndentPosition(depth);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            
            string linkText = set.linkType == ConditionSet.LinkType.AND ? "UND" : "ODER";
            EditorGUILayout.LabelField($"Gruppe ({linkText}):");
            EditorGUILayout.EndHorizontal();

            if (set.subSets != null && set.subSets.Length > 0)
            {
                foreach (var subSet in set.subSets)
                {
                    DrawConditionSetStatus(subSet, depth + 1);
                }
            }
            else if (set.conditions != null)
            {
                foreach (var condition in set.conditions)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(indent + INDENT_WIDTH);
                    
                    int currentValue = GameProgress.Current.GetVariable(condition.variable);
                    bool isMet = condition.Test();
                    
                    EditorGUILayout.LabelField($"Variable {condition.variable}:");
                    EditorGUILayout.LabelField($"{currentValue} {condition.type} {condition.value}");
                    EditorGUILayout.LabelField(isMet ? "✓" : "✗", GUILayout.Width(20));
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}