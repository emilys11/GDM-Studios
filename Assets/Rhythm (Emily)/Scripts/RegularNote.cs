using UnityEngine;

public class RegularNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private float hitWindow = 50f;
    [SerializeField] private float missY = -450f;

    private HitBarType hitBarType;
    public RectTransform hitbarTransform;

    private RectTransform rect;
    private bool isResolved;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        RhythmInput.OnHitInput += OnInput;
    }

    void OnDisable()
    {
        RhythmInput.OnHitInput -= OnInput;
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (!isResolved && rect.anchoredPosition.y < missY)
        {
            Miss();
        }
    }

    void OnInput(HitBarType inputType)
    {
        if (isResolved) return;
        if (inputType != hitBarType) return;

        float dist = Mathf.Abs(rect.anchoredPosition.y);

        if (dist <= hitWindow)
        {
            Hit();
        }
        else
        {
            Miss();
        }
    }

    public void Hit()
    {
        Debug.Log("Note hit on+ "+ hitBarType.ToString());
        isResolved = true;
        RhythmEvents.NoteHit();
        Destroy(gameObject);
    }

    void Miss()
    {
        isResolved = true;
        RhythmEvents.NoteMissed();
        Destroy(gameObject);
    }
    public void SetSpeed(float s) => speed = s;
    public void SetHitBarType(HitBarType type) => hitBarType = type;
}