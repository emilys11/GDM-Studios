using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += Hurt;
        RhythmEvents.OnNoteMiss += Attack;
        RhythmEvents.OnBadInput += Attack;
    }

    void Attack() => animator.SetTrigger("Attack");
    void Hurt() => animator.SetTrigger("Hurt");
}