using System;
using UnityEngine;
public class PlayerDialogue : MonoBehaviour
{
    //TODO : Remove this component from player when merging
    [SerializeField] GameObject dialogueManager;

    private Dialogue dialogue;
    private SphereCollider sphereCollider;
    public bool canTalk = false;
    public TextAsset npcScript;
    private NPCDialogue npcDialogue;
    public Sprite speakerSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = dialogueManager.GetComponent<Dialogue>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void disableMovement()
    {
        
        gameObject.GetComponent<PlayerController>().rb.linearVelocity = Vector3.zero;
        gameObject.GetComponent<PlayerController>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        //Disable movement
    }
  
    public void enableMovement()
    {
        gameObject.GetComponent<PlayerController>().enabled = true;
        gameObject.GetComponent<Animator>().enabled = true;
        //Enable movement
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            //canTalk = true;
            if (!dialogue.dialogueFinished)
            {
                npcDialogue = other.gameObject.GetComponent<NPCDialogue>();
                npcScript = other.gameObject.GetComponent<NPCDialogue>().npcScript; //get specific script for npc
                speakerSprite = other.gameObject.GetComponent<NPCDialogue>().speakerImage;
                disableMovement();
                dialogue.StartDialogue();
            }

        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("NPC"))
    //    {
    //        dialogue.dialogueFinished = false;
    //        canTalk = false;
    //    }
    //}

    public void resetDialogue()
    {
        dialogue.dialogueFinished = false;
        dialogue.continueDialogue = true;
        npcDialogue.reposition();
    }
}
