using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(PlayerVictory), 3f);
            Debug.Log("Congratulations - you successfully released climachange to the world!");
        }
    }

    private void PlayerVictory() 
    {
        SceneManager.LoadScene(0);
    }
}
