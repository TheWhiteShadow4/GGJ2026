using UnityEngine;
using UnityEditor;

namespace TWS.Events
{
    [CustomPropertyDrawer(typeof(ProgressEvent))]
    public class ProgressEventDrawer : PropertyDrawer
    {
		private const string ASSET_PATH = "Assets/Settings/ProgressVariables.asset";

		private ProgressVariables _variables;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
			// Asset laden oder Create-Button anzeigen
			if (_variables == null)
			{
				_variables = AssetDatabase.LoadAssetAtPath<ProgressVariables>(ASSET_PATH);
			}

            EditorGUI.BeginProperty(position, label, property);

			var variableProp = property.FindPropertyRelative("variable");
			var valueProp = property.FindPropertyRelative("value");

            // Berechne die Breite für jedes Feld (minus den Abstand zwischen ihnen)
            float spacing = 5f;
            float fieldWidth = (position.width - spacing) / 2;

            // Erstelle Rechtecke für die beiden Felder
            var variableRect = new Rect(position.x, position.y, fieldWidth, position.height);
            var valueRect = new Rect(position.x + fieldWidth + spacing, position.y, fieldWidth, position.height);

            // Zeichne die Felder
            EditorGUI.PropertyField(variableRect, variableProp, GUIContent.none);

			if (_variables != null)
			{
				ProgressVariableType type = _variables.Get(variableProp.intValue).type;
				switch (type)
				{
					case ProgressVariableType.Flag:
						valueProp.intValue = EditorGUI.Toggle(valueRect, valueProp.intValue == 1) ? 1 : 0;
						break;
					case ProgressVariableType.Decision:
						valueProp.intValue = EditorGUI.Popup(valueRect, valueProp.intValue, new string[] { "Open", "Yes", "No" });
						break;
					case ProgressVariableType.Bitset:
						DrawBitsetField(valueRect, variableProp, valueProp);
						break;
					default: // Number
						EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
						break;
				}
			}
			else
			{
				EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
			}

            EditorGUI.EndProperty();
        }

		private void DrawBitsetField(Rect valueRect, SerializedProperty variableProp, SerializedProperty valueProp)
		{
			// Hole die Bit-Namen für diese Variable
			IBitNameProvider bitNameProvider = _variables?.GetBitNameProvider(variableProp.intValue);
			int bitNameCount = bitNameProvider?.GetBitNameCount() ?? 0;

			if (bitNameProvider == null || bitNameCount == 0)
			{
				// Fallback: Zeige einzelnes Int-Eingabefeld
				EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
				return;
			}

			// Erstelle Display-Namen für MaskField
			string[] displayNames = new string[bitNameCount];
			for (int i = 0; i < bitNameCount; i++)
			{
				string bitName = bitNameProvider.GetBitName(i);
				displayNames[i] = string.IsNullOrEmpty(bitName) ? $"Bit {i}" : bitName;
			}

			// Zeige MaskField (wie Unity's LayerMask)
			EditorGUI.BeginChangeCheck();
			int newMask = EditorGUI.MaskField(valueRect, valueProp.intValue, displayNames);
			if (EditorGUI.EndChangeCheck())
			{
				valueProp.intValue = newMask;
			}
		}
    }
} 