using UnityEngine;
using UnityEngine.SceneManagement;

public class Open_Scene_Pop : MonoBehaviour
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

