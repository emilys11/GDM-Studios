using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 1000;
    [SerializeField] private int currentHP;
    [SerializeField] private Slider hpBar;

    void Start()
    {
        currentHP = maxHP;
    }

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += TakeDamage;
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
    }
}
