using UnityEngine;

public class Gate : MonoBehaviour
{
    public int amountRequired = 2;
    public bool opened = false;

    private bool playerInRange = false;
    private KeyCollector currentPlayer;

    private void Update()
    {
        if (playerInRange && !opened && Input.GetKeyDown(KeyCode.E))
        {
            if (currentPlayer.keysCollected >= amountRequired)
            {
                OpenGate();
            }
            else
            {
                Debug.Log("Collect all keys first!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        KeyCollector kc = other.GetComponent<KeyCollector>();
        if (kc != null)
        {
            playerInRange = true;
            currentPlayer = kc;
            Debug.Log("Press E to open gate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<KeyCollector>() != null)
        {
            playerInRange = false;
            currentPlayer = null;
        }
    }

    private void OpenGate()
    {
        opened = true;

        foreach (var c in GetComponents<Collider>())
            c.enabled = false;

        var r = GetComponent<Renderer>();
        if (r != null) r.enabled = false;

        Debug.Log("Gate opened!");
    }
}