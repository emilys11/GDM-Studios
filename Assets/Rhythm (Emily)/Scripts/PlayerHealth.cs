using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int hearts = 3;
    [SerializeField] private GameObject[] heartsObjects;

    void OnEnable()
    {
        foreach(GameObject heart in heartsObjects)
        {
            heart.SetActive(true);
        }

        hearts = 3;

        RhythmEvents.OnNoteMiss += TakeDamage;
        RhythmEvents.OnBadInput += TakeDamage;
    }

    void TakeDamage()
    {
        hearts--;
        //avoid out of bounds
        if(hearts >= 0)
        {
            heartsObjects[hearts].SetActive(false);
        }


        if (hearts <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
    }
}
