using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    void OnEnable()
    {
        RhythmEvents.OnReady += FadingOut;
        RhythmEvents.OnDeath += AppearOnDeath;
    }

    void OnDisable()
    {
        RhythmEvents.OnReady -= FadingOut;
        RhythmEvents.OnDeath -= AppearOnDeath;
    }

    public void AppearOnDeath()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void FadingOut()
    {
        GetComponent<Fade>().FadeOut();
    }
}
