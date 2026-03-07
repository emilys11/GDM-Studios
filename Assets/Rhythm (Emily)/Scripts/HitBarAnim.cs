using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitBarAnim : MonoBehaviour
{
    private Image feedbackImage;

    public Sprite hitSprite;
    public Sprite missSprite;
    public Sprite emptySprite;

    [SerializeField] private float animDuration = 0.08f;

    void Awake()
    {
        feedbackImage = GetComponent<Image>();
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
}
