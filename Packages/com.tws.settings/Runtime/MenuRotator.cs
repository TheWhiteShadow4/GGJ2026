using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using System;

namespace TWS.Settings
{
	/// <summary>
	/// Basisklasse für Settings mit Drehknopf (z.B. Audio)
	/// </summary>
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(Image))]
	public class MenuRotator : MonoBehaviour, IntValueUI, IDragHandler
	{
		public float minAngle = -120f;
		public float maxAngle = 120f;
		public int steps = 100;
		public int defaultValue = 0;

		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private string formatString = "{0}";

		protected Image image;

		public UnityEvent<int> OnValueChanged => onValueChanged;
		public Func<int, string> Formatter { get; set; }

		private UnityEvent<int> onValueChanged = new UnityEvent<int>();

		private int _value;
		private RectTransform rectTransform;

		public int Value => _value;

		public void SetValue(int value)
		{
			_value = value;
			image.transform.rotation = Quaternion.Euler(0f, 0f, 360f - (minAngle + (maxAngle - minAngle) * value / steps));
			onValueChanged.Invoke(value);
			if (valueText != null)
			{
				if (Formatter != null)
					this.valueText.text = Formatter(value);
				else
					this.valueText.text = string.Format(formatString, value.ToString());
			}
		}
		
		void Awake()
		{
			image = GetComponent<Image>();
			rectTransform = GetComponent<RectTransform>();
			SetValue(defaultValue);
		}

		public void OnDrag(PointerEventData eventData)
		{
			// Konvertiere Mausposition zu lokalen Koordinaten relativ zum RectTransform
			Vector2 localMousePos = eventData.position - (Vector2)rectTransform.position;

			float angle = 90f - Mathf.Atan2(localMousePos.y, localMousePos.x) * Mathf.Rad2Deg;
			
			// Normalisiere Winkel auf -180° bis 180° Bereich (wichtig für korrekte Clamp-Berechnung)
			if (angle > 180f) angle -= 360f;
			if (angle < -180f) angle += 360f;
			
			ApplyValueByAngle(angle);
		}

		private void ApplyValueByAngle(float angle)
		{
			angle = Mathf.Clamp(angle, minAngle, maxAngle);
			int v = Mathf.RoundToInt((angle - minAngle) / (maxAngle - minAngle) * steps);
			SetValue(v);
		}
	}
} 