using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TWS.Settings
{
	[ExecuteAlways]
	[RequireComponent(typeof(Image))]
	public class KarusselButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[HideInInspector] public Karussel karussel;
		[HideInInspector] public bool isLeft = false;

		public Color normalColor = Color.white;
		public Color highlightedColor = Color.lightGray;
		public Color disabledColor = Color.darkGray;

		private bool _disabled = false;
		private Image image;

		public bool disabled
		{
			get => _disabled;
			set
			{
				_disabled = value;
				var uiState = GetComponent<IUIState>();
				if (uiState != null)
				{
					uiState.Disabled = value;
				}
				image.color = value ? disabledColor : normalColor;
			}
		}

		void Awake()
		{
			image = GetComponent<Image>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (isLeft)
			{
				karussel.OnLeftArrowClick();
			}
			else
			{
				karussel.OnRightArrowClick();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (disabled) return;
			image.color = highlightedColor;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (disabled) return;
			image.color = normalColor;
		}
	}
}