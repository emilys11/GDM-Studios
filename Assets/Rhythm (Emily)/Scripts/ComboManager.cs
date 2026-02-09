using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int combo;
    public float multiplier => 1 + combo * 0.1f;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += AddCombo;
        RhythmEvents.OnNoteMiss += ResetCombo;
        RhythmEvents.OnBadInput += ResetCombo;
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteHit -= AddCombo;
        RhythmEvents.OnNoteMiss -= ResetCombo;
        RhythmEvents.OnBadInput -= ResetCombo;
    }

    void AddCombo() => combo++;
    void ResetCombo() => combo = 0;
}
