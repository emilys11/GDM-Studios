using UnityEngine;

public class SimonStartTrigger : MonoBehaviour
{
    public SimonSaysManager manager;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            manager.StartMiniGame();
        }
    }
}