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
    void Start()
    {
        //int i = 0;
        //while (i < crabLines.Length && (crabLines[i].Equals("Astronaut\n") || crabLines[i].Equals("Crab\n")))
        //{
        //    lines[i] = crabLines[i];
        //    lines[i+1] = crabLines[i+1];
        //    i += 2;
        //}


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
        if (Input.GetKeyDown(KeyCode.E) && playerDialogue.canTalk) //&& playerDialogue.canTalk
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
        readScript();

        index = 0;
        StartCoroutine(TypeLine());
        textPlaying = true;
    }

    IEnumerator TypeLine()
    {
        if(index % 2 == 0)
        {
            //string name = lines[index];

            nameText.text = lines[index];
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

            //Get name of speaker

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
            textPlaying = false;
            textMesh.SetText(string.Empty);
            playerDialogue.enableMovement(); //Allow movement again
        }
    }
}
