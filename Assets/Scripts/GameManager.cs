using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI (optional)")]
    public TMP_Text scoreText;
    public TMP_Text comboText;

    [Header("Keys")]
    public KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    private int score = 0;
    private int combo = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
                TryHitLane(lane);
        }
    }

    private void TryHitLane(int lane)
    {
        // Find any note in this lane that is currently hittable
        foreach (var note in FindObjectsOfType<Note>())
        {
            if (note.laneIndex == lane && note.TryHit())
                return;
        }

        // pressed with no hittable note
        Miss();
    }

    public void Hit()
    {
        combo++;
        score += 100;
        RefreshUI();
    }

    public void Miss()
    {
        combo = 0;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (scoreText) scoreText.text = $"Score: {score}";
        if (comboText) comboText.text = $"Combo: {combo}";
    }
}
