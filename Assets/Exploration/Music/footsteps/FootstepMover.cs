using UnityEngine;

public class FootstepMover : MonoBehaviour
{
    public FootstepPlayer footstepPlayer;
    public Rigidbody rb;

    public float stepInterval = 0.5f;
    public float minimumMoveSpeed = 0.1f;

    private float stepTimer;

    void Update()
    {
        if (footstepPlayer == null || rb == null) return;

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f;

        if (horizontalVelocity.magnitude > minimumMoveSpeed)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                footstepPlayer.PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}