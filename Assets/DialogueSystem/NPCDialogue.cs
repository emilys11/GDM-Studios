using System;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] GameObject[] repositionPoints;
    private int nextPos = 0;
    public TextAsset npcScript; //Dialogue to play for this npc
    public Sprite speakerImage;

    public bool repositionComplete = false;

    public void reposition() //For the crab
    {
        
        if (nextPos < repositionPoints.Length)
        {
            gameObject.transform.position = repositionPoints[nextPos].transform.position;
            nextPos++;
        }

        if (nextPos == repositionPoints.Length)
        {
            repositionComplete = true;
        }
    }
}
