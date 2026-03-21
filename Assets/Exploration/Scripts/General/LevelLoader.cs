using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;
    public string sceneToLoad;

    [SerializeField] private string triggerName = "Start";

    public IEnumerator LoadLevel(string sceneName)
    {
        Debug.Log("LevelLoader on " + gameObject.name + " loading scene: " + sceneName);
        Debug.Log("Triggering animator trigger: " + triggerName);

        if (transition != null)
        {
            transition.ResetTrigger(triggerName);
            transition.SetTrigger(triggerName);
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}