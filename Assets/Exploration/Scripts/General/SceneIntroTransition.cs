using UnityEngine;
using System.Collections;

public class SceneIntroTransition : MonoBehaviour
{
    [SerializeField] private Animator introAnimator;
    [SerializeField] private float introDuration = 2f;
    [SerializeField] private GameObject gameplayRoot;

    void Start()
    {
        Debug.Log("=== SceneIntroTransition START ===");

        if (gameplayRoot != null)
        {
            gameplayRoot.SetActive(false);
            Debug.Log("Gameplay root disabled");
        }
        else
        {
            Debug.Log("No gameplayRoot assigned");
        }

        if (introAnimator == null)
        {
            Debug.LogError("SceneIntroTransition: introAnimator is NOT assigned.");
            return;
        }

        Debug.Log("SceneIntroTransition: animator = " + introAnimator.gameObject.name);

        StartCoroutine(EnableGameplayAfterIntro());
    }

    IEnumerator EnableGameplayAfterIntro()
    {
        yield return new WaitForSeconds(introDuration);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(true);

        Debug.Log("=== SceneIntroTransition END ===");
    }
}