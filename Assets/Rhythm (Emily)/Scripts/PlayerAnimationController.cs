using UnityEngine;
using System.Collections;
public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;

    private bool isPlaying;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += Attack;
        RhythmEvents.OnNoteMiss += Hurt;
        RhythmEvents.OnBadInput += Hurt;
    }

    void Attack()
    {
        if (isPlaying) return;
        animator.SetTrigger("Attack");
        StartCoroutine(WaitForAnimation());
    }

    void Hurt()
    {
        if (isPlaying) return;
        animator.SetTrigger("Hurt");
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        isPlaying = true;

        yield return null;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        while (state.normalizedTime < 1f)
        {
            state = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        isPlaying = false;
    }
}