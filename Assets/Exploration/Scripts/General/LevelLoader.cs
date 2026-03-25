using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;
    public string sceneToLoad;

    [SerializeField] private string triggerName = "Start";
    [SerializeField] private string targetSpawnID = "DefaultSpawn";

    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);

        if (isLoading) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered level loader trigger");
            Debug.Log("Saving next spawn ID: " + targetSpawnID);

            isLoading = true;
            SpawnData.nextSpawnID = targetSpawnID;

            StartCoroutine(LoadLevel(sceneToLoad));
        }
    }

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