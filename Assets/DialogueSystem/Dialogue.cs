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

    [SerializeField] private Animator animator;

    [SerializeField] private LevelLoader firstLevelLoader;
    [SerializeField] private LevelLoader secondLevelLoader;

    [SerializeField] private TextAsset testScript;

    private TextAsset script;
    private string[] lines;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI nameText;
    private int index;

    public bool displaySpeaker = true;

    private bool textPlaying = false;
    public bool dialogueFinished = false;
    private bool resumedDialogue = false;

    public bool continueDialogue = false;//for crab
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        nameText = namePlate.GetComponent<TextMeshProUGUI>();

        speakerPanel.SetActive(false);

        textMesh.SetText(string.Empty);
        nameText.SetText(string.Empty);
        playerDialogue = player.GetComponent<PlayerDialogue>();

        Debug.Log("textMesh = " + textMesh);
        Debug.Log("nameText = " + nameText);
        Debug.Log("playerDialogue = " + playerDialogue);
        Debug.Log("interactmsg = " + interactmsg);
        Debug.Log("dialogueBox = " + dialogueBox);

        if (textMesh != null) textMesh.SetText(string.Empty);
        if (nameText != null) nameText.SetText(string.Empty);
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
        
        if (lines != null && index < lines.Length && lines[index] != null && lines[index].Trim() == ">")
        {
            Debug.Log("Scene change marker detected!");
            StopAllCoroutines();
            index++;
            StartCoroutine(exitDialogue());
        }
    }

    public void StartDialogue()
    {
        readScript();

        if (GameManager.isNextPlanet)
        {
            index = DialogueState.savedIndex;
            GameManager.isNextPlanet = false;
            resumedDialogue = true;
            Debug.Log("StartDialogue: resuming at index " + index);
        }
        else
        {
            if (continueDialogue)
            {
                index++;
            }
            else
            {
                index = 0;
                resumedDialogue = false;
                Debug.Log("StartDialogue: starting fresh");
            }
            
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
        if (index >= lines.Length)
            yield break;

        string currentLine = lines[index].Trim();

        // Transition marker
        if (currentLine == ">")
        {
            Debug.Log("Scene change marker detected in TypeLine at index " + index);
            StopAllCoroutines();
            index++;
            StartCoroutine(exitDialogue());
            yield break;
        }

        // Speaker name line
        if (index % 2 == 0)
        {
            nameText.text = currentLine;

            if (currentLine == "ASTRONAUT")
            {
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerImage;
                }
                nameText.color = Color.ghostWhite;
            }
            else if (currentLine == "SIR CRABIUS THE III")
            {
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerDialogue.speakerSprite;
                }
                nameText.color = Color.forestGreen;
            }
            else
            {
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerDialogue.speakerSprite;
                }
                nameText.color = Color.softRed;
            }

            index++;
        }

        if (index >= lines.Length)
            yield break;

        // If next line is somehow the transition marker, catch it too
        if (lines[index].Trim() == ">")
        {
            Debug.Log("Scene change marker detected after name at index " + index);
            StopAllCoroutines();
            index++;
            StartCoroutine(exitDialogue());
            yield break;
        }

        textMesh.SetText(string.Empty);

        foreach (char c in lines[index])
        {
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
                StartCoroutine(exitDialogue());
            }
        }

        IEnumerator exitDialogue()
    {
        Debug.Log("exitDialogue called. resumedDialogue = " + resumedDialogue + ", index = " + index);

        DialogueState.savedIndex = index;

        textPlaying = false;
        textMesh.SetText(string.Empty);
        nameText.SetText(string.Empty);
        playerDialogue.enableMovement(); //Allow movement again
        if (displaySpeaker)
        {
            speakerPanel.SetActive(false);
        }
        dialogueFinished = true;

        if(playerDialogue.npcDialogue.gameObject.GetComponent<BoxCollider>() != null && !playerDialogue.npcDialogue.repositionComplete) //For AI Planet interaction, if repositionComplete then battle should start
        {
            playerDialogue.resetDialogue();
        }

        if (resumedDialogue)
        {
            Debug.Log("Using secondLevelLoader");

            if (secondLevelLoader == null)
            {
                yield break;
            }

            yield return StartCoroutine(secondLevelLoader.LoadLevel(secondLevelLoader.sceneToLoad));
            yield break;
        }

        Debug.Log("Using firstLevelLoader");

        if (firstLevelLoader == null)
        {
            yield break;
        }

        yield return StartCoroutine(firstLevelLoader.LoadLevel(firstLevelLoader.sceneToLoad));
    }
}
