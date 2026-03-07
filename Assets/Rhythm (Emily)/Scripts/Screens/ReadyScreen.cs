using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        RhythmEvents.OnReady += FadingOut;
    }

    void OnDisable()
    {
        RhythmEvents.OnReady -= FadingOut;
    }

    public void FadingOut()
    {
        GetComponent<Fade>().FadeOut();
    }
}
