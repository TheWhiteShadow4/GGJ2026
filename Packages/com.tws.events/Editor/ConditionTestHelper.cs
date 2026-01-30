using UnityEngine;
using UnityEditor;

namespace TWS.Events
{
    public static class ConditionTestHelper
    {
        private const float PADDING = 2f;

		private const string ASSET_PATH = "Assets/Settings/ProgressVariables.asset";

		private static ProgressVariables _variables;

        public static void DrawConditionTest(Rect rect, SerializedProperty element, bool showBackground = true)
        {
			if (_variables == null)
			{
				_variables = AssetDatabase.LoadAssetAtPath<ProgressVariables>(ASSET_PATH);
			}

            // Gesamthöhe mit Padding
            rect.height = EditorGUIUtility.singleLineHeight + PADDING * 2;

			var activeProp = element.FindPropertyRelative("active");
            var variableProp = element.FindPropertyRelative("variable");
            var valueProp = element.FindPropertyRelative("value");
            var typeProp = element.FindPropertyRelative("type");

			var type = ProgressVariableType.Number;
			if (_variables != null)
			{
				type = _variables.Get(variableProp.intValue).type;
			}

            // Hintergrund zeichnen wenn im Play Mode
            if (showBackground && Application.isPlaying && GameProgress.Current != null)
            {
                int currentValue = GameProgress.Current.GetVariable(variableProp.intValue);
                int targetValue = valueProp.intValue;
                bool isTrue = false;

                switch ((ConditionType)typeProp.enumValueIndex)
                {
                    case ConditionType.Gleich: isTrue = currentValue == targetValue; break;
                    case ConditionType.Ungleich: isTrue = currentValue != targetValue; break;
                    case ConditionType.Größer: isTrue = currentValue > targetValue; break;
                    case ConditionType.Kleiner: isTrue = currentValue < targetValue; break;
                    case ConditionType.GrößerGleich: isTrue = currentValue >= targetValue; break;
                    case ConditionType.KleinerGleich: isTrue = currentValue <= targetValue; break;
                }

                if (isTrue)
                {
                    EditorGUI.DrawRect(rect, new Color(0.0f, 0.8f, 0.0f, 0.5f));
                }
            }

            // Innere Rect für die Controls (mit Padding)
            rect.y += PADDING;
            rect.height = EditorGUIUtility.singleLineHeight;

            float spacing = 5;
            float typeWidth = 100;
            float valueWidth = 60;
            float variableWidth = rect.width - typeWidth - valueWidth - spacing * 2 - 20;

			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, 20, rect.height),
				activeProp, GUIContent.none);

			rect.x += 20;

			if (activeProp.boolValue)
			{
				EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, variableWidth, rect.height),
                variableProp, GUIContent.none);

				Rect typeRect = new Rect(rect.x + variableWidth + spacing, rect.y, typeWidth, rect.height);
				Rect valueRect = new Rect(rect.x + variableWidth + typeWidth + spacing * 2, rect.y, valueWidth, rect.height);
				switch (type)
				{
					case ProgressVariableType.Flag:
						typeProp.enumValueIndex = (int)ConditionType.Gleich;
						valueProp.intValue = EditorGUI.Toggle(valueRect, valueProp.intValue == 1) ? 1 : 0;
						break;
					case ProgressVariableType.Decision:
						EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
						valueProp.intValue = EditorGUI.Popup(valueRect, valueProp.intValue, new string[] { "Open", "Yes", "No" });
						break;
					case ProgressVariableType.Bitset:
						typeProp.enumValueIndex = (int)ConditionType.Bits;
						DrawBitsetCondition(valueRect, typeRect, variableProp, valueProp);
						break;
					default: // Number
						EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
						EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
						break;	
				}
			}
			else
			{
				EditorGUI.LabelField(rect, "No Condition");
			}
        }

        public static float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + PADDING * 2;
        }

		private static void DrawBitsetCondition(Rect valueRect, Rect typeRect, SerializedProperty variableProp, SerializedProperty valueProp)
		{
			// Hole die Bit-Namen für diese Variable
			IBitNameProvider bitNameProvider = _variables?.GetBitNameProvider(variableProp.intValue);
			int bitNameCount = bitNameProvider?.GetBitNameCount() ?? 0;

			if (bitNameProvider == null || bitNameCount == 0)
			{
				// Fallback: Zeige einzelnes Bit-Eingabefeld
				EditorGUI.LabelField(typeRect, "Bit:");
				valueProp.intValue = EditorGUI.IntField(valueRect, valueProp.intValue);
				return;
			}

			// Erweitere die Rect für das MaskField über beide Bereiche
			Rect maskFieldRect = typeRect;
			maskFieldRect.width = typeRect.width + valueRect.width + 5;

			// Erstelle Display-Namen für MaskField
			string[] displayNames = new string[bitNameCount];
			for (int i = 0; i < bitNameCount; i++)
			{
				string bitName = bitNameProvider.GetBitName(i);
				displayNames[i] = string.IsNullOrEmpty(bitName) ? $"Bit {i}" : bitName;
			}

			// Zeige MaskField (wie Unity's LayerMask)
			EditorGUI.BeginChangeCheck();
			int newMask = EditorGUI.MaskField(maskFieldRect, valueProp.intValue, displayNames);
			if (EditorGUI.EndChangeCheck())
			{
				valueProp.intValue = newMask;
			}
		}
    }
} 