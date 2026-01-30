using UnityEngine;
using UnityEngine.Events;

namespace TWS.Events
{
	[DefaultExecutionOrder(-200)]
	public class ProgressCondition : MonoBehaviour
	{
		#if UNITY_EDITOR
		public string description;
		#endif
		public GameObject activateObject;
		public MonoBehaviour activateComponent;
		public bool callEventOnLoad = false;
		public bool testWhenAlreadyActivated = false;
		public ConditionTest[] conditions = new ConditionTest[1] { new ConditionTest() { active = true } };
		

		public UnityEvent onTriggered;

		void Start()
		{
			GameProgress.Current.OnVariableChanged += OnVariableChanged;
			bool test = TestConditions();

			if (activateObject!= null) activateObject.SetActive(test);
			if (activateComponent != null) activateComponent.enabled = test;
			if (callEventOnLoad && test)
			{
				onTriggered.Invoke();
			}
		}

		void OnDestroy()
		{
			GameProgress.Current.OnVariableChanged -= OnVariableChanged;
		}

		private void OnVariableChanged(EventData data)
		{
			bool active = activateObject != null && activateObject.activeInHierarchy;
			if (active && !testWhenAlreadyActivated) return;
			active = activateComponent != null && activateComponent.enabled;
			if (active && !testWhenAlreadyActivated) return;

			bool test = TestConditions();
			if (activateObject) activateObject.SetActive(test);
			if (activateComponent != null) activateComponent.enabled = test;
			if (test)
			{
				onTriggered.Invoke();
			}
		}
		
		private bool TestConditions()
		{
			foreach (var condition in conditions)
			{
				if (!condition.Test()) return false;
			}
			return true;
		}
	}
}