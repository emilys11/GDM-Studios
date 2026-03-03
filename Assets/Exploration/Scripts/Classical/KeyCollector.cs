using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public int keysCollected = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keysCollected++;
            Destroy(other.gameObject);
            Debug.Log("Keys: " + keysCollected);
        }
    }
}