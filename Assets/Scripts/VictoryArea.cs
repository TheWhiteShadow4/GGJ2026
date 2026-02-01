using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryArea : MonoBehaviour
{
    [SerializeField] AudioClip _victorySound;

    public bool VictoryIsYours { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VictoryIsYours = true;
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.PlaySound(_victorySound);
            Invoke(nameof(PlayerVictory), 3f);
            Debug.Log("Congratulations - you successfully released climachange to the world!");
            // TODO: Show or bind to Victory text here!
        }
    }

    private void PlayerVictory() 
    {
        SceneManager.LoadScene(0);
    }
}
