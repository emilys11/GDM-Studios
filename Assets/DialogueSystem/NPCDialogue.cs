using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] GameObject[] repositionPoints;
    private int nextPos = 0;
    public TextAsset npcScript; //Dialogue to play for this npc
    public Sprite speakerImage;

    public void reposition() //For the crab
    {
        gameObject.transform.position = repositionPoints[nextPos].transform.position;
        if (nextPos < repositionPoints.Length)
        {
            nextPos++;
        }
    }
}
