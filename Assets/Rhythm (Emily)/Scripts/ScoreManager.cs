using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private ComboManager combo;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += AddScore;
    }

    void AddScore()
    {
        score += Mathf.RoundToInt(100 * combo.multiplier);
    }
}
