using UnityEngine;
using TWS.Events;

public class GameManager : MonoBehaviour
{
	public static readonly string SAVE_GAME_KEY = "SaveGame";
	public static GameManager Instance { get; private set; }

	public bool wasLoaded = false;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		GameProgress.Current = new GameProgress(20);
	}

	public void SaveGame()
	{
		GameProgress.Current.SetPlayerPosition(PlayerController.Current.transform);
		PlayerPrefs.SetString(SAVE_GAME_KEY, JsonUtility.ToJson(GameProgress.Current));
	}

	public void LoadGame()
	{
		string saveGame = PlayerPrefs.GetString(SAVE_GAME_KEY);
		if (string.IsNullOrEmpty(saveGame))
		{
			return;
		}
		GameProgress.Current = JsonUtility.FromJson<GameProgress>(saveGame);
		wasLoaded = true;
	}
}
