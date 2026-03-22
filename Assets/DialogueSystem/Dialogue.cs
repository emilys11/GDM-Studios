using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    //TODO: Make it so after talking once, talking to the npc again repeats their last line? (Use queues but last line doesnt exit q?)
    

    
    [SerializeField] float textSpeed;
    [SerializeField] GameObject player;
    [SerializeField] GameObject interactmsg;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject namePlate;
    [SerializeField] GameObject speakerPanel;

    [SerializeField] Sprite playerImage;

    private TextAsset script;
    private string[] lines;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI nameText;
    private int index;

    public bool displaySpeaker = true;

    private bool textPlaying = false;
    public bool dialogueFinished = false;
    void Start()
    {

        textMesh = GetComponent<TextMeshProUGUI>();
        nameText = namePlate.GetComponent<TextMeshProUGUI>();

        speakerPanel.SetActive(false);

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

        if (displaySpeaker)
        {
            speakerPanel.SetActive(true);
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
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerImage;
                }
                nameText.color = Color.ghostWhite;
            }
            else if(lines[index].CompareTo("SIR CRABIUS THE III\r") == 0)
            {
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerDialogue.speakerSprite;
                }
                nameText.color = Color.forestGreen;
            }
            else //Bosses
            {
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerDialogue.speakerSprite;
                }
                nameText.color = Color.softRed;
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
        if (displaySpeaker)
        {
            speakerPanel.SetActive(false);
        }
        dialogueFinished = true;
    }
}
