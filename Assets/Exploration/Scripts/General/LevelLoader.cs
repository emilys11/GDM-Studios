using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public string sceneToLoad; // 👈 add this

    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isLoading) return;

        if (other.CompareTag("Player"))
        {
            isLoading = true;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(LoadLevel(sceneToLoad));
        }
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}