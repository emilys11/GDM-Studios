using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private GameObject ui;

    [SerializeField] private AudioSource src;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            ui.SetActive(true);
            Time.timeScale = 0f;
            src.Stop();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        src.loop = true;
        src.Play();
        ui.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
