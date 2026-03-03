using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastMoveDir;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(Vector3 moveDir)
    {
        bool isMoving = moveDir.magnitude > 0.1f;

        animator.SetBool("IsMoving", isMoving);
        if (isMoving)
        {
            animator.SetFloat("MoveFront", moveDir.x);
            animator.SetFloat("MoveSide", moveDir.z);

            lastMoveDir = moveDir;

            animator.SetFloat("LastMoveFront", lastMoveDir.x);
            animator.SetFloat("LastMoveSide", lastMoveDir.z);
        }
    }
}