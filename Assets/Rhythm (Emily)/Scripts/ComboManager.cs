using UnityEngine;
using TMPro;
public class ComboManager : MonoBehaviour
{
    public int combo;
    public float multiplier => 1 + combo * 0.1f;
    [SerializeField] private TextMeshProUGUI text;

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

    void AddCombo() 
    {
        combo++;
        text.text = (combo+1).ToString()+"X";
    } 
    void ResetCombo()
    {
        combo = 0;
        text.text = "1X";
    } 
}
