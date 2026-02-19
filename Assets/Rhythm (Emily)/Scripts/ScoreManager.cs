using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private ComboManager combo;

    [SerializeField] private int penalty = 100;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += AddScore;
        RhythmEvents.OnBadInput += SubtractScore;
    }

    void AddScore()
    {
        score += Mathf.RoundToInt(100 * combo.multiplier);
    }

    void SubtractScore()
    {
        score -= penalty;
        score = Mathf.Max(score, 0);
    }
}
