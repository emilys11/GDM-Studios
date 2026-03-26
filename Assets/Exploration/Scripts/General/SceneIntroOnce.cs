using UnityEngine;
using System.Collections;

public class SceneIntroOnce : MonoBehaviour
{
    [SerializeField] private Animator introAnimator;
    [SerializeField] private float introDuration = 2f;
    [SerializeField] private GameObject gameplayRoot;

    private static bool hasPlayedIntro = false;

    void Start()
    {
        Debug.Log("=== SceneIntroOnce START ===");

        if (introAnimator == null)
        {
            Debug.LogError("SceneIntroOnce: introAnimator is NOT assigned.");
            return;
        }

        if (hasPlayedIntro)
        {
            Debug.Log("Intro already played → skipping");

            if (gameplayRoot != null)
                gameplayRoot.SetActive(true);

            // disable intro visuals entirely
            introAnimator.gameObject.SetActive(false);
            return;
        }

        hasPlayedIntro = true;

        if (gameplayRoot != null)
            gameplayRoot.SetActive(false);

        StartCoroutine(EnableGameplayAfterIntro());
    }

    IEnumerator EnableGameplayAfterIntro()
    {
        yield return new WaitForSeconds(introDuration);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(true);

        // hide intro after playing
        introAnimator.gameObject.SetActive(false);

        Debug.Log("=== SceneIntroOnce END ===");
    }
}