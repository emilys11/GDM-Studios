using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

public class Dialogue : MonoBehaviour
{
    //TODO: Make it so after talking once, talking to the npc again repeats their last line? (Use queues but last line doesnt exit q?)
    

    
    [SerializeField] float textSpeed;
    [SerializeField] GameObject player;
    [SerializeField] GameObject interactmsg;
    [SerializeField] GameObject textBox;

    private TextAsset script;
    private string[] lines;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
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
        textMesh.SetText(string.Empty);
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
        textBox.SetActive(textPlaying);
        if (Input.GetKeyDown(KeyCode.E) && playerDialogue.canTalk) //&& playerDialogue.canTalk
        {
            if (!textPlaying)
            {
                playerDialogue.disableMovement(); //Prevent them from moving so they cant walk away
                StartDialogue();
            }
            else
            {
                if (textMesh.text == lines[index - 1] + "\n\n" + lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textMesh.text = lines[index-1] + "\n\n" + lines[index];
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
            textMesh.text = lines[index] + "\n\n"; //Get name of speaker
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
