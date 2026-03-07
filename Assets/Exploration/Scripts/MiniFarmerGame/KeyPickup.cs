using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (StealthMiniGameManager.Instance != null)
        {
            StealthMiniGameManager.Instance.CompleteMiniGame();
        }

        gameObject.SetActive(false);
    }
}