using UnityEngine;
using System;
public class RhythmEvents : MonoBehaviour
{
    public static Action OnNoteHit;
    public static Action OnNoteMiss;
    public static Action OnBadInput;

    public static void NoteHit() => OnNoteHit?.Invoke();
    public static void NoteMissed() => OnNoteMiss?.Invoke();
    public static void BadInput() => OnBadInput?.Invoke();
}
