using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject Audio;

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (StealthMiniGameManager.Instance != null)
        {
            StealthMiniGameManager.Instance.CompleteMiniGame();
            Audio.SetActive(false);
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
        }
        
        gameObject.SetActive(false);
    }
}