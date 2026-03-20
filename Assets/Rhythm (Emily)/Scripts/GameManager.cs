using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] private string nextPlanetScene;
    void OnEnable()
    {
        RhythmEvents.OnDeath += EnableDeathScreen;
        RhythmEvents.OnWin += EnableWinScreen;
    }

    void OnDisable()
    {
        RhythmEvents.OnDeath -= EnableDeathScreen;
        RhythmEvents.OnWin -= EnableWinScreen;
    }

    public void Ready()
    {
        RhythmEvents.Ready();
    }

    public void NextPlanet()
    {
        SceneManager.LoadScene(nextPlanetScene);
    }

    void EnableDeathScreen()
    {
        EnableScreen(loseScreen);
        loseScreen.GetComponent<CanvasGroup>().alpha = 1;
    }

    void EnableWinScreen()
    {
        EnableScreen(winScreen);
    }

    public static void DisableScreen(GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void EnableScreen(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void ResetGame()
    {
        RhythmEvents.Ready();
    }
}
