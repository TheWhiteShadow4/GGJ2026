using UnityEngine;

public class VictoryArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Congratulations - you successfully released climachange to the world!");
        }
    }
}