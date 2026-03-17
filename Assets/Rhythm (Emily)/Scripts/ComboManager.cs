using UnityEngine;
using TMPro;
using System.Collections;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private int hitsPerMultiplier = 5;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float scaleUpAmount = 1.5f;
    [SerializeField] private float animDuration = 0.2f;  

    private int consecutiveHits;
    public int multiplier = 1;

    public int CurrentMultiplier => multiplier;

    private Coroutine scaleCoroutine;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += AddCombo;
        RhythmEvents.OnNoteMiss += ResetCombo;
        RhythmEvents.OnBadInput += ResetCombo;
        RhythmEvents.OnReady += ResetCombo;
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteHit -= AddCombo;
        RhythmEvents.OnNoteMiss -= ResetCombo;
        RhythmEvents.OnBadInput -= ResetCombo;
        RhythmEvents.OnReady -= ResetCombo;
    }

    void Start()
    {
        UpdateUI();
    }

    void AddCombo()
    {
        consecutiveHits++;
        int oldMultiplier = multiplier;
        multiplier = 1 + (consecutiveHits / hitsPerMultiplier);

        UpdateUI();

        if (multiplier > oldMultiplier)
        {
            if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(AnimateScaleAndColor());
        }
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

    IEnumerator AnimateScaleAndColor()
    {
        Vector3 originalScale = text.rectTransform.localScale;
        Vector3 targetScale = originalScale * scaleUpAmount;
        Color originalColor = text.color;

        float elapsed = 0f;

        text.color = Color.yellow;

        while (elapsed < animDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (animDuration / 2f);
            text.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < animDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (animDuration / 2f);
            text.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        text.rectTransform.localScale = originalScale;
        text.color = originalColor;
    }
}