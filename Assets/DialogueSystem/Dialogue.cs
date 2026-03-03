using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    //TODO: Make it so after talking once, talking to the npc again repeats their last line?, implement movement restriction while they are talking in PlayerDialogue

    [SerializeField] string[] lines;
    [SerializeField] float textSpeed;
    [SerializeField] GameObject player;
    [SerializeField] GameObject interactmsg;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
    private int index;

    private bool textPlaying = false;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.SetText(string.Empty);
        playerDialogue = player.GetComponent<PlayerDialogue>();
        
    }
    private void Update()
    {
        Debug.Log(index);
        interactmsg.SetActive(playerDialogue.canTalk && !textPlaying);
        if (Input.GetKeyDown(KeyCode.E) && playerDialogue.canTalk)
        {
            if (!textPlaying)
            {
                playerDialogue.disableMovement(); //Prevent them from moving so they cant walk away
                StartDialogue();
            }
            else
            {
                if (textMesh.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textMesh.text = lines[index];
                }
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        textPlaying = true;
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray()){
            textMesh.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {

        if (index < lines.Length - 1)
        {
            index++;
            textMesh.SetText(string.Empty);
            StartCoroutine(TypeLine());
        }
        else
        {
            textPlaying = false;
            textMesh.SetText(string.Empty);
            playerDialogue.enableMovement(); //Allow movement again
        }
    }
}
