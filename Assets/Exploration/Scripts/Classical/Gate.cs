using UnityEngine;

public class Gate : MonoBehaviour
{
    public int amountRequired = 2;
    public bool opened = false;

    public GameObject interactText;
    public GameObject noKeysText;

    // References to the door meshes
    public GameObject closedDoorMesh;  // Closed door mesh
    public GameObject openDoorMesh;    // Open door mesh

    private bool playerInRange = false;
    private KeyCollector currentPlayer;

    private void Start()
    {
        if (interactText != null) interactText.SetActive(false);
        if (noKeysText != null) noKeysText.SetActive(false);

        // Ensure only the closed door is active initially
        if (closedDoorMesh != null) closedDoorMesh.SetActive(true);
        if (openDoorMesh != null) openDoorMesh.SetActive(false);
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

        // Disable the closed door mesh and enable the open door mesh
        if (closedDoorMesh != null) closedDoorMesh.SetActive(false);
        if (openDoorMesh != null) openDoorMesh.SetActive(true);

        // Move all child objects (including the locks) with the gate
        foreach (Transform child in transform)
        {
            // You can update position, scale, etc., for any child, if needed
            child.gameObject.SetActive(true); // Make sure children are active if they are deactivated
        }

        // Disable colliders and renderer for the closed door
        foreach (var c in GetComponents<Collider>())
            c.enabled = false;

        var r = GetComponent<Renderer>();
        if (r != null) r.enabled = false;

        // Hide the interaction text
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