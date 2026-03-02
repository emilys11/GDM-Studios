using System;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    //TODO : Remove this component from player when merging

    private SphereCollider sphereCollider;
    private bool npcFound = false;
    private bool isTalking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        if (isTalking)
        {
            //Disable movement?
        }
    }
    public bool getNPCFound()
    {
        return npcFound;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npcFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npcFound=false;
        }
    }
}
