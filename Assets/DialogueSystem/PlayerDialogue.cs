using System;
using UnityEngine;
public class PlayerDialogue : MonoBehaviour
{
    //TODO : Remove this component from player when merging

    private SphereCollider sphereCollider;
    public bool canTalk = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void disableMovement()
    {
        //Disable movement
    }

    public void enableMovement()
    {
        //Enable movement
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            canTalk = true;
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
