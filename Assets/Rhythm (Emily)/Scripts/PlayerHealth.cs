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
        MusicManager.OnMusicFinished += Die;
        RhythmEvents.OnHealthNoteHit += AddHeart;
        RhythmEvents.OnReady += ResetHearts;
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteMiss -= TakeDamage;
        RhythmEvents.OnBadInput -= TakeDamage;
        MusicManager.OnMusicFinished -= Die;
        RhythmEvents.OnHealthNoteHit -= AddHeart;
        RhythmEvents.OnReady -= ResetHearts;
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
        UnityEngine.Debug.Log("Just died fr");
        RhythmEvents.Death();
    }

    void ResetHearts()
    {
        hearts = 3;
        
        foreach(GameObject heart in heartsObjects)
        {
            heart.SetActive(true);
        }
    }

    void AddHeart()
    {
        if (hearts >= 3) return;
        heartsObjects[hearts].SetActive(true);
        hearts++;
    }
}
