using UnityEngine;

public class Gate : MonoBehaviour
{
    public int amountRequired = 2;
    public bool opened = false;

    public GameObject interactText;
    public GameObject noKeysText;

    private bool playerInRange = false;
    private KeyCollector currentPlayer;

    private void Start()
    {
        if (interactText != null) interactText.SetActive(false);
        if (noKeysText != null) noKeysText.SetActive(false);
    }

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
                if (noKeysText != null)
                {
                    noKeysText.SetActive(true);
                    Invoke(nameof(HideNoKeysText), 2f); // hide after 2 seconds
                }

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

            if (interactText != null && !opened)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<KeyCollector>() != null)
        {
            playerInRange = false;
            currentPlayer = null;

            if (interactText != null) interactText.SetActive(false);
            if (noKeysText != null) noKeysText.SetActive(false);
        }
    }

    private void OpenGate()
    {
        opened = true;

        foreach (var c in GetComponents<Collider>())
            c.enabled = false;

        var r = GetComponent<Renderer>();
        if (r != null) r.enabled = false;

        if (interactText != null) interactText.SetActive(false);
        if (noKeysText != null) noKeysText.SetActive(false);

        Debug.Log("Gate opened!");
    }

    private void HideNoKeysText()
    {
        if (noKeysText != null)
            noKeysText.SetActive(false);
    }
}