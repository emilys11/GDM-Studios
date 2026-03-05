using UnityEngine;


public class ExplorationUI : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject interactMsg;
    [SerializeField] GameObject dialogue;
    [SerializeField] GameObject speaker;

    private PlayerDialogue playerDialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDialogue = player.GetComponent<PlayerDialogue>();
        //interactMsg.SetActive(false);
        //dialogue.SetActive(false);
        //speaker.SetActive(false);
    }

    void FixedUpdate()
    {

    }
}
