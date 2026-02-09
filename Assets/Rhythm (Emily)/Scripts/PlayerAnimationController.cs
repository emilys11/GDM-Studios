using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += Attack;
        RhythmEvents.OnNoteMiss += Hurt;
        RhythmEvents.OnBadInput += Hurt;
    }

    void Attack() => animator.SetTrigger("Attack");
    void Hurt() => animator.SetTrigger("Hurt");
}