using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	public float mainMenuMusicReduction = 0.3f;

	private AudioSource musicSource;
	private AudioLowPassFilter filter;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);

		musicSource = gameObject.GetComponent<AudioSource>();
		filter = gameObject.GetComponent<AudioLowPassFilter>();

		SceneManager.sceneLoaded += OnSceneLoaded;
		SetFilter(SceneManager.GetActiveScene().buildIndex);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		SetFilter(scene.buildIndex);
	}

	void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void SetFilter(int sceneIndex)
	{
		if (sceneIndex == 0)
		{
			filter.enabled = true;
			musicSource.volume = 1f - mainMenuMusicReduction;
		}
		else
		{
			filter.enabled = false;
			musicSource.volume = 1f;
		}
	}
}