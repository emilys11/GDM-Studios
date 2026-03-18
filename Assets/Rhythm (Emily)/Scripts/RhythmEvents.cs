using UnityEngine;
using System;
public class RhythmEvents : MonoBehaviour
{
    [Header("Notes related")]
    public static Action OnNoteHit;
    public static Action OnNoteMiss;
    public static Action OnBadInput;

    [Header("Game Loop related")]
    public static Action OnReady;
    public static Action OnDeath;
    public static Action OnWin;

    public static void NoteHit() => OnNoteHit?.Invoke();
    public static void NoteMissed() => OnNoteMiss?.Invoke();
    public static void BadInput() => OnBadInput?.Invoke();
    public static void Ready() => OnReady?.Invoke();
    public static void Death() => OnDeath?.Invoke();
    public static void Win() => OnWin?.Invoke();
}
