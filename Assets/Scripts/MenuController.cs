using UnityEngine;
using UnityEngine.SceneManagement;
using TWS.Events;

public class MenuController : MonoBehaviour
{
	public int newGameSceneIndex = 1;
	public GameObject loadGameButton;

	void Start()
	{
		loadGameButton.SetActive(PlayerPrefs.HasKey(GameManager.SAVE_GAME_KEY));
	}

	public void StartNewGame()
	{
		GameProgress.Current = new GameProgress(20);
		SceneManager.LoadScene(newGameSceneIndex);
	}

	public void LoadGame()
	{
		GameManager.Instance.LoadGame();
		SceneManager.LoadScene(GameProgress.Current.currentScene);
	}

	public void ExitGame()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
	}
}
