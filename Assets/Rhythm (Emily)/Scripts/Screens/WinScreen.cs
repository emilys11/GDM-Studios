using UnityEngine;

public class WinScreen : MonoBehaviour
{
    void OnEnable()
    {
        RhythmEvents.OnWin += AppearOnWin;
    }

    void OnDisable()
    {
        RhythmEvents.OnWin -= AppearOnWin;
    }

    public void AppearOnWin()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }

}
