using UnityEngine;
using System.Collections;

public class NPCWatcher : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float closedEyesDuration = 3f;
    [SerializeField] private float openEyesDuration = 2f;

    [Header("Visuals")]
    [SerializeField] private GameObject eyesClosedVisual;
    [SerializeField] private GameObject eyesOpenVisual;

    [SerializeField] private NPCVision vision;

    [SerializeField] private NPCVisionConeMesh visionConeMesh;

    private bool isWatching = false;
    private Coroutine watchRoutine;
    private StealthMiniGameManager miniGameManager;

    public void SetMiniGameManager(StealthMiniGameManager manager)
    {
        miniGameManager = manager;
    }

    public void BeginWatchingCycle()
    {
        StopWatchingCycle();
        watchRoutine = StartCoroutine(WatchCycle());
    }

    public void StopWatchingCycle()
    {
        if (watchRoutine != null)
        {
            StopCoroutine(watchRoutine);
            watchRoutine = null;
        }

        isWatching = false;
        UpdateVisuals();

        if (vision != null)
        {
            vision.ShowCone(false);
        }

        if (visionConeMesh != null)
        {
            visionConeMesh.SetVisible(false);
        }
    }

    public bool IsWatching()
    {
        return isWatching;
    }

    private void Update()
    {
        if (!isWatching) return;

        if (vision != null && vision.CanSeePlayer())
        {
            StealthMiniGameManager.Instance.FailMiniGame();
        }
    }

    private IEnumerator WatchCycle()
    {
        while (true)
        {
            isWatching = false;
            UpdateVisuals();

            if (vision != null)
            {
                vision.ShowCone(false);
            }

            if (visionConeMesh != null)
            {
                visionConeMesh.SetVisible(false);
            }

            yield return new WaitForSeconds(closedEyesDuration);

            isWatching = true;
            UpdateVisuals();

            if (vision != null)
            {
                vision.ShowCone(true);
            }

            if (visionConeMesh != null)
            {
                visionConeMesh.SetVisible(true);
            }

            yield return new WaitForSeconds(openEyesDuration);
        }
    }

    private void UpdateVisuals()
    {
        if (eyesClosedVisual != null) eyesClosedVisual.SetActive(!isWatching);
        if (eyesOpenVisual != null) eyesOpenVisual.SetActive(isWatching);
    }
}