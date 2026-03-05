using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int hitsPerMultiplier = 5;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;

    private int consecutiveHits;
    public int multiplier = 1;

    public int CurrentMultiplier => multiplier;

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

    void Start()
    {
        UpdateUI();
    }

    void AddCombo()
    {
        consecutiveHits++;
        multiplier = 1 + (consecutiveHits / hitsPerMultiplier);

        UpdateUI();
    }

    void ResetCombo()
    {
        consecutiveHits = 0;
        multiplier = 1;

        UpdateUI();
    }

    void UpdateUI()
    {
        text.text = multiplier + "X";
    }
}