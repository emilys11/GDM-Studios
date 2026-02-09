using UnityEngine;
using System.Collections.Generic;
/**
ATTACH THIS TO EACH OF THE 4 HIT BARS.
ROLE: DETECTS IF NOTE HAS BEEN HIT OR NOT
**/
public class HitBar : MonoBehaviour
{
    [SerializeField] private HitBarType hitBarType;
    [SerializeField] private KeyCode key;
    float y => GetComponent<RectTransform>().anchoredPosition.y;

    private List<INote> notes = new List<INote>();

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("Key pressed: " + key);
            TryHitNote();
        }
    }

    public void RegisterNote(INote note)
    {
        notes.Add(note);
    }

    void TryHitNote()
    {
        if (notes.Count == 0)
        {
            RhythmEvents.BadInput();
            return;
        }

        INote closest = notes[0];

        if (closest.TryHit(y))
        {
            RhythmEvents.NoteHit();
            notes.Remove(closest);
        }
        else
        {
            RhythmEvents.BadInput();
        }
    }
}
