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
    public NPCDialogue npcDialogue;
    public Sprite speakerSprite;

    private bool talkedToJelly = false;
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
        gameObject.GetComponent<PlayerAnimator>().UpdateAnimation(Vector3.zero);
        //Disable movement
    }
  
    public void enableMovement()
    {
        gameObject.GetComponent<PlayerController>().enabled = true;
        //Enable movement
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            //canTalk = true;
            if (other.gameObject.GetComponent<CapsuleCollider>() != null && !other.GetComponent<NPCDialogue>().alreadyTalked)
            {
                dialogue.dialogueFinished = false;
                talkedToJelly = true;
            }

            if (!dialogue.dialogueFinished)
            {
                npcDialogue = other.gameObject.GetComponent<NPCDialogue>();
                npcScript = other.gameObject.GetComponent<NPCDialogue>().npcScript; //get specific script for npc
                speakerSprite = other.gameObject.GetComponent<NPCDialogue>().speakerImage;
                disableMovement();
                dialogue.StartDialogue();
                npcDialogue.alreadyTalked = true;
            }

            //if (other.gameObject.GetComponent<CapsuleCollider>() != null)
            //{
            //    dialogue.dialogueFinished = false;
            //    talkedToJelly = true;
            //}

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
