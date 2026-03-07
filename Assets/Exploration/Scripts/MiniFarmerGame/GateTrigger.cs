using UnityEngine;
using System.Collections;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private bool shouldCloseAfterOpen = false;
    [SerializeField] private float closeDelay = 1.5f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (StealthMiniGameManager.Instance != null)
        {
            StealthMiniGameManager.Instance.StartMiniGame();

            if (shouldCloseAfterOpen)
            {
                StartCoroutine(CloseGateAfterDelay());
            }
        }
    }

    private IEnumerator CloseGateAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        if (StealthMiniGameManager.Instance != null)
        {
            StealthMiniGameManager.Instance.CloseGateOnly();
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}