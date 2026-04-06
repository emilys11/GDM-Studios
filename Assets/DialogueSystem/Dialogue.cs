using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] float textSpeed;
    [SerializeField] GameObject player;
    [SerializeField] GameObject interactmsg;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject namePlate;
    [SerializeField] GameObject speakerPanel;
    [SerializeField] GameObject keyPrompt;

    [SerializeField] Sprite playerImage;

    [SerializeField] private Animator animator;
    [SerializeField] private TMP_FontAsset specialFont;

    [SerializeField] private LevelLoader firstLevelLoader;
    [SerializeField] private LevelLoader secondLevelLoader;

    [SerializeField] private TextAsset testScript;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource speakerSourceA;
    [SerializeField] private AudioSource speakerSourceB;
    [SerializeField] private AudioSource sfxSource;

    [Header("Speaker A Settings")]
    [SerializeField] private string[] speakerANames;
    [SerializeField] private AudioClip[] speakerAClips;

    [Header("Speaker B Settings")]
    [SerializeField] private string[] speakerBNames;
    [SerializeField] private AudioClip[] speakerBClips;

    [Header("Special One-Off SFX")]
    [SerializeField] private AudioClip specialLineClip;
    [SerializeField] private string specialLineTrigger = "Mouth pops (recording of Taym mouth popping)";

    [Header("Blip Settings")]
    [SerializeField] private int blipFrequency = 2;

    private TextAsset script;
    private string[] lines;

    private PlayerDialogue playerDialogue;
    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI nameText;
    private int index;
    private TMP_FontAsset defFont;
    

    public bool displaySpeaker = true;

    private bool textPlaying = false;
    public bool dialogueFinished = false;
    private bool resumedDialogue = false;

    public bool continueDialogue = false;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        nameText = namePlate.GetComponent<TextMeshProUGUI>();

        speakerPanel.SetActive(false);

        defFont = textMesh.font;
        textMesh.SetText(string.Empty);
        nameText.SetText(string.Empty);
        keyPrompt.SetActive(false);
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
        lines = Regex.Split(
            Regex.Replace(script.text, @"^\s*$\n", string.Empty, RegexOptions.Multiline),
            "\n"
        );
    }

    private void Update()
    {
        interactmsg.SetActive(playerDialogue.canTalk && !textPlaying);
        dialogueBox.SetActive(textPlaying);

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && textPlaying)
        {
            if (textMesh.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();

                if (speakerSourceA != null)
                    speakerSourceA.Stop();

                if (speakerSourceB != null)
                    speakerSourceB.Stop();

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

            //gameObject.GetComponent<Animator>().enabled = true;
            playerDialogue.gameObject.GetComponent<PlayerAnimator>().UpdateAnimation(playerDialogue.gameObject.transform.forward * 5);
            playerDialogue.gameObject.GetComponent<PlayerAnimator>().UpdateAnimation(Vector3.zero);
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
        keyPrompt.SetActive(true);
        StartCoroutine(TypeLine());
        textPlaying = true;
    }

    IEnumerator TypeLine()
    {
        if (index >= lines.Length)
            yield break;

        string currentLine = lines[index].Trim();
        if(currentLine == ">>")
        {
            StopAllCoroutines();
            index++;
            StartCoroutine(exitDialogue());
            yield break;
        }

        if (currentLine == ">")
        {
            Debug.Log("Scene change marker detected in TypeLine at index " + index);
            StopAllCoroutines();
            index++;
            index++;
            StartCoroutine(exitDialogue());
            yield break;
        }

        // Speaker name line
        if (index % 2 == 0)
        {
            nameText.text = currentLine;
            textMesh.font = defFont;

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

                if (lines[index + 1].StartsWith("<"))
                {
                    textMesh.font = specialFont;
                    lines[index + 1] = lines[index + 1].Remove(0, 1);
                }
            }
            else if(currentLine == "ELSALA MIKSON" || currentLine == "JELLY BELLY")
            {
                if (displaySpeaker)
                {
                    speakerPanel.GetComponent<Image>().sprite = playerDialogue.speakerSprite;
                }
                nameText.color = Color.aquamarine;
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

        if (lines[index].Trim() == ">")
        {
            Debug.Log("Scene change marker detected after name at index " + index);
            StopAllCoroutines();
            index++;
            StartCoroutine(exitDialogue());
            yield break;
        }

        textMesh.SetText(string.Empty);

        string speakerName = nameText.text;
        string dialogueLine = lines[index];
        bool isSpecialLine = dialogueLine.Contains(specialLineTrigger);

        if (isSpecialLine && sfxSource != null && specialLineClip != null)
        {
            sfxSource.PlayOneShot(specialLineClip);
        }

        int visibleCharIndex = 0;

        foreach (char c in dialogueLine)
        {
            textMesh.text += c;

            if (!isSpecialLine)
            {
                PlaySpeakerBlip(speakerName, c, visibleCharIndex);
            }

            visibleCharIndex++;
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
        keyPrompt.SetActive(false);
        playerDialogue.enableMovement();

        if (displaySpeaker)
        {
            speakerPanel.SetActive(false);
        }

        dialogueFinished = true;

        if (playerDialogue.npcDialogue.gameObject.GetComponent<BoxCollider>() != null &&
            !playerDialogue.npcDialogue.repositionComplete)
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

    private bool IsSpeakerInList(string speaker, string[] speakerList)
    {
        if (string.IsNullOrWhiteSpace(speaker) || speakerList == null)
            return false;

        string trimmedSpeaker = speaker.Trim().ToLower();

        for (int i = 0; i < speakerList.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(speakerList[i]) &&
                trimmedSpeaker == speakerList[i].Trim().ToLower())
            {
                return true;
            }
        }

        return false;
    }

    private AudioSource GetSpeakerSource(string speaker)
    {
        if (IsSpeakerInList(speaker, speakerBNames))
            return speakerSourceB;

        return speakerSourceA;
    }

    private AudioClip GetMappedClip(string speaker, string[] speakerNames, AudioClip[] speakerClips)
    {
        if (string.IsNullOrWhiteSpace(speaker) || speakerNames == null || speakerClips == null)
            return null;

        string trimmedSpeaker = speaker.Trim().ToLower();
        int count = Mathf.Min(speakerNames.Length, speakerClips.Length);

        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(speakerNames[i]) &&
                trimmedSpeaker == speakerNames[i].Trim().ToLower())
            {
                return speakerClips[i];
            }
        }

        return null;
    }

    private AudioClip GetSpeakerClip(string speaker)
    {
        AudioClip clip = GetMappedClip(speaker, speakerBNames, speakerBClips);
        if (clip != null)
            return clip;

        return GetMappedClip(speaker, speakerANames, speakerAClips);
    }

    private void PlaySpeakerBlip(string speaker, char letter, int visibleCharIndex)
    {
        if (char.IsWhiteSpace(letter))
            return;

        if (visibleCharIndex % blipFrequency != 0)
            return;

        AudioSource source = GetSpeakerSource(speaker);
        AudioClip clip = GetSpeakerClip(speaker);

        if (source == null || clip == null)
            return;

        source.pitch = 1f;
        source.PlayOneShot(clip);
    }
}