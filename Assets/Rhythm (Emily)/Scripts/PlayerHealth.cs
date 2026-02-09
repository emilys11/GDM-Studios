using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int hearts = 3;

    void OnEnable()
    {
        RhythmEvents.OnNoteMiss += TakeDamage;
        RhythmEvents.OnBadInput += TakeDamage;
    }

    void TakeDamage()
    {
        hearts--;

        if (hearts <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        Time.timeScale = 0f;
    }
}
