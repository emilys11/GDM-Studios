using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    private PlayerDialogue  playerDialogue;
    public int keysCollected = 0;

    private void Start()
    {
        playerDialogue = GetComponent<PlayerDialogue>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keysCollected++;
            playerDialogue.resetDialogue();
            Destroy(other.gameObject);
            Debug.Log("Keys: " + keysCollected);
        }
    }
}