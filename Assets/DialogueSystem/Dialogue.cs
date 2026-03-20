using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;

public class Dialogue : MonoBehaviour
{
    //TODO: Make it so after talking once, talking to the npc again repeats their last line? (Use queues but last line doesnt exit q?)
    

    
    [SerializeField] float textSpeed;
    [SerializeField] GameObject player;
    [SerializeField] GameObject interactmsg;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject namePlate;

    private TextAsset script;
    private string[] lines;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI nameText;
    private int index;

    private bool textPlaying = false;
    public bool dialogueFinished = false;
    void Start()
    {

        textMesh = GetComponent<TextMeshProUGUI>();
        nameText = namePlate.GetComponent<TextMeshProUGUI>();

        textMesh.SetText(string.Empty);
        nameText.SetText(string.Empty);
        playerDialogue = player.GetComponent<PlayerDialogue>();
        //StartDialogue();
        
    }
    void readScript()
    {
        script = playerDialogue.npcScript;
        lines = Regex.Split(Regex.Replace(script.text, @"^\s*$\n", string.Empty, RegexOptions.Multiline), "\n"); //Format input script
    }
    private void Update()
    {
        interactmsg.SetActive(playerDialogue.canTalk && !textPlaying);
        dialogueBox.SetActive(textPlaying);
        if (Input.GetKeyDown(KeyCode.E) && textPlaying) //&& playerDialogue.canTalk
        {
            //if (!textPlaying)
            //{
            //    playerDialogue.disableMovement(); //Prevent them from moving so they cant walk away
            //    StartDialogue();
            //}
            //else
            //{
            //    if (textMesh.text == lines[index])
            //    {
            //        NextLine();
            //    }
            //    else
            //    {
            //        StopAllCoroutines();
            //        textMesh.text = lines[index];
            //    }
            //}
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

        if (nameText.text.CompareTo(">\r") == 0) //To breakdown dialogue
        {
            StopAllCoroutines();
            exitDialogue();
            index++;
        }
    }

    public void StartDialogue()
    {
        if(lines == null)
        {
            readScript();
            index = 0;
        }

        StartCoroutine(TypeLine());
        textPlaying = true;
    }

    IEnumerator TypeLine()
    {
        if(index % 2 == 0)
        {
            //string name = lines[index];

            nameText.text = lines[index]; //Get name of speaker
            if (lines[index].CompareTo("ASTRONAUT\r") == 0)
            {
                nameText.color = Color.ghostWhite;
            }
            else if(lines[index].CompareTo("CRAB\r") == 0)
            {
                nameText.color = Color.green;
            }
            else //Bosses
            {
                nameText.color = Color.red;
            }

            index++;

        }
        foreach(char c in lines[index].ToCharArray()){ //Type the text by char
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
            exitDialogue();
        }
    }

    void exitDialogue()
    {
        textPlaying = false;
        textMesh.SetText(string.Empty);
        nameText.SetText(string.Empty);
        playerDialogue.enableMovement(); //Allow movement again
        dialogueFinished = true;
    }
}
