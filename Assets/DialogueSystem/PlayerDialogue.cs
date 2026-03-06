using System;
using UnityEngine;
public class PlayerDialogue : MonoBehaviour
{
    //TODO : Remove this component from player when merging

    private SphereCollider sphereCollider;
    public bool canTalk = false;
    public TextAsset npcScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void disableMovement()
    {
        gameObject.GetComponent<PlayerController>().enabled = false;
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
            canTalk = true;
            npcScript = other.gameObject.GetComponent<NPCDialogue>().npcScript; //get specific script for npc
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            canTalk=false;
        }
    }
}
