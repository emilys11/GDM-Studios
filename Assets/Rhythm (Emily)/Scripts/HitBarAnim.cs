using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitBarAnim : MonoBehaviour
{
    private Image feedbackImage;

    public Sprite hitSprite;
    public Sprite missSprite;
    public Sprite emptySprite;
    public Sprite holdSprite;

    [SerializeField] private float animDuration = 0.08f;

    [Header("Hold Pulsate Settings")]
    [SerializeField] private float pulseSpeed = 4f;     
    [SerializeField] private float pulseAmount = 0.2f;   

    private bool isHolding = false;

    void Awake()
    {
        feedbackImage = GetComponent<Image>();
    }

    void Update()
    {
        if (isHolding && holdSprite != null)
        {
            feedbackImage.sprite = holdSprite;
            float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            feedbackImage.rectTransform.localScale = Vector3.one * scale;
        }
        else
        {
            feedbackImage.rectTransform.localScale = Vector3.one;
        }
    }

    public void PlayFeedback(Sprite sprite)
    {
        feedbackImage.sprite = sprite;
        StopAllCoroutines();
        StartCoroutine(FeedbackAnim());
    }

    IEnumerator FeedbackAnim()
    {
        RectTransform rect = feedbackImage.rectTransform;
        float time = 0f;

        while (time < animDuration)
        {
            time += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 1.35f, time / animDuration);
            rect.localScale = Vector3.one * scale;
            yield return null;
        }

        time = 0f;

        while (time < animDuration)
        {
            time += Time.deltaTime;
            float scale = Mathf.Lerp(1.35f, 1f, time / animDuration);
            rect.localScale = Vector3.one * scale;
            yield return null;
        }

        rect.localScale = Vector3.one;
        feedbackImage.sprite = emptySprite;
    }

    public void StartHold()
    {
        isHolding = true;
    }

    public void EndHold()
    {
        isHolding = false;
        feedbackImage.rectTransform.localScale = Vector3.one;
        feedbackImage.sprite = emptySprite;
    }
}