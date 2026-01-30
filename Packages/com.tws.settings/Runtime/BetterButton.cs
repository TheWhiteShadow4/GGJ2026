using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace TWS.Settings
{
	[RequireComponent(typeof(Selectable))]
	public class BetterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
	{
		public StateTransition highlighted = StateTransition.Default;
		public StateTransition selected = StateTransition.Default;
		public StateTransition pressed = StateTransition.Default;
		public StateTransition disabled = StateTransition.Default;

		private StateTransition normal;
		private Selectable selectable;
		private TextMeshProUGUI text;

		void Awake()
		{
			selectable = GetComponent<Selectable>();
			text = GetComponentInChildren<TextMeshProUGUI>();
			normal.textColor = text.color;
			normal.scale = transform.localScale;
		}

		private void ApplyState(StateTransition state)
		{
			text.color = state.textColor;
			transform.localScale = state.scale;
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			ApplyState(highlighted);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (EventSystem.current && EventSystem.current.currentSelectedGameObject == selectable.gameObject)
			{
				ApplyState(selected);
			}
			else
			{
				ApplyState(normal);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			ApplyState(pressed);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			ApplyState(highlighted);
		}

		public void OnSelect(BaseEventData eventData)
		{
			ApplyState(selected);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			ApplyState(normal);
		}
	}

	[System.Serializable]
	public struct StateTransition
	{
		public Color textColor;
		public Vector2 scale;

		public static StateTransition Default => new StateTransition { textColor = Color.white, scale = Vector2.one };
	}
}