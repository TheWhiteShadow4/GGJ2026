using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TWS.Events
{
	[System.Serializable]
	public class GameProgress
	{
		// aktuelle Singleton Instanz (Spielstand)
		public static GameProgress Current;

		public string saveName;
		public int currentScene;
		public float[] playerPosition = new float[3];
		public float[] playerRotation = new float[3];
		public float playTime;

		public List<int> variables = new List<int>();

		public System.Action<EventData> OnVariableChanged;

		public GameProgress(int initialSize)
		{
			variables = new List<int>(initialSize);
		}

		public void SetVariable(int index, int value)
		{
			if (index >= variables.Count)
			{
				variables.AddRange(new int[index - variables.Count + 100]);
			}
			var old = variables[index];
			variables[index] = value;
			if (old != value)
			{
				OnVariableChanged?.Invoke(new EventData(index, old, value));
			}
		}

		public int GetVariable(int index)
		{
			if (index >= variables.Count) return 0;
			return variables[index];
		}

		public void SetPlayerPosition(Transform transform)
		{
			SetPlayerPositionInScene(SceneManager.GetActiveScene().buildIndex, transform.position, transform.rotation.eulerAngles);
		}

		public void SetPlayerPositionInScene(int sceneIndex, Vector3 position, Vector3 rotation)
		{
			currentScene = sceneIndex;
			playerPosition = new float[] { position.x, position.y, position.z };
			playerRotation = new float[] { rotation.x, rotation.y, rotation.z };
		}

		public void RestorePlayerPosition(Transform transform)
		{
			transform.position = new Vector3(playerPosition[0], playerPosition[1], playerPosition[2]);
			transform.rotation = Quaternion.Euler(playerRotation[0], playerRotation[1], playerRotation[2]);
		}
	}

	public struct EventData
	{
		public int index;
		public int oldValue;
		public int newValue;

		public EventData(int index, int oldValue, int newValue)
		{
			this.index = index;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}
	}
}