using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenuArea : MonoBehaviour
{
    [SerializeField] VictoryArea _victory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_victory.VictoryIsYours)
            { // the player will return back to the main menu because of the VictoryArea -> so nothing to do here
                return;
            }
            Invoke(nameof(PlayerVictory), 3f);
        }
    }

    private void PlayerVictory()
    {
        SceneManager.LoadScene(0);
    }
}