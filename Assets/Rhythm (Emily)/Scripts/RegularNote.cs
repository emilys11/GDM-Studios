using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class RegularNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private float hitWindow = 50f;
    [SerializeField] private float missY = -450f;

    public RectTransform hitbarTransform;

    private RectTransform rect;
    private bool isHit = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (rect.anchoredPosition.y < missY && !isHit)
        {
            Miss();
        }
    }

    public bool TryHit(float y)
    {
        if (isHit) return false;

        float dist = Mathf.Abs(
            rect.anchoredPosition.y - hitbarTransform.anchoredPosition.y
        );

        if (dist <= hitWindow)
        {
            isHit = true;
            Debug.Log("HIT");
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    void Miss()
    {
        isHit = true;
        RhythmEvents.NoteMissed();
        Destroy(gameObject);
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}