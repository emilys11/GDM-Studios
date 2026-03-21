using UnityEngine;

public class MiniRhythm : MonoBehaviour
{
    [SerializeField] private GameObject miniRhythm;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private PlayerDialogue playerDialogue;

    private bool gameTriggered = false;

    private void OnTriggerEnter(Collider other)
    {

        if (gameTriggered) return;

        if (other.CompareTag(playerTag))
        {
            TriggerGame();
            gameTriggered = true;
        }
    }

    public void TriggerGame()
    {
        miniRhythm.SetActive(true);
    }

    public void WinMinigame()
    {
        //insert winning logic here
        KeyCollector.keysCollected++;
        playerDialogue.resetDialogue();
        Debug.Log("KEY HOE");

        //KEEP THIS
        miniRhythm.SetActive(false);
    }
}