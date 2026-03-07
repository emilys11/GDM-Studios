using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 1000;
    [SerializeField] private int currentHP;
    [SerializeField] private Slider hpBar;

    void OnEnable()
    {
        currentHP = maxHP;
        hpBar.value = 1f;

        RhythmEvents.OnNoteHit += TakeDamage;
        RhythmEvents.OnReady += ResetHealth;
    }

    void OnDisable()
    {
        currentHP = maxHP;
        hpBar.value = 1f;
        RhythmEvents.OnReady -= ResetHealth;
    }

    void ResetHealth()
    {
        currentHP = maxHP;
        hpBar.value = 1f;
    }

    void TakeDamage()
    {
        currentHP -= 50;
        hpBar.value = (float)currentHP / maxHP;

        if (currentHP <= 0)
        {
            Win();
        }
    }

    void Win()
    {
        Debug.Log("yay win");
        RhythmEvents.OnNoteHit -= TakeDamage;
        RhythmEvents.Win();
    }
}
