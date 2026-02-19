using UnityEngine.SceneManagement;
using UnityEngine;

public class Open_Scene : MonoBehaviour
{

    public string Scene_Name;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Scene_Name);
        }



    }
}
