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

    [SerializeField] private Animator animator;

    [SerializeField] private LevelLoader firstLevelLoader;
    [SerializeField] private LevelLoader secondLevelLoader;

    private TextAsset script;
    private string[] lines;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI nameText;
    private int index;

    private bool textPlaying = false;
    public bool dialogueFinished = false;
    private bool resumedDialogue = false;
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
            index = 0;
            resumedDialogue = false;
            Debug.Log("StartDialogue: starting fresh");
        }

        textMesh.SetText(string.Empty);
        nameText.SetText(string.Empty);
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
        playerDialogue.enableMovement();
        dialogueFinished = true;

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
