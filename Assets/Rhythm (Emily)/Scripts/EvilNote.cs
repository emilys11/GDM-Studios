using UnityEngine;

public class EvilNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private float hitWindow = 50f;
    [SerializeField] private float missY = -450f;

    [SerializeField] private int penaltyOnHit = 150;

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
        if (isResolved) return;

        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (rect.anchoredPosition.y < missY)
        {
            isResolved = true;
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
    }

    public void Hit()
    {
        isResolved = true;
        RhythmEvents.NoteMissed();
        Destroy(gameObject);

    }

    void Miss()
    {
  
        Debug.Log("Evil Note hit on+ "+ hitBarType.ToString());
        isResolved = true;
        RhythmEvents.BadInput();
        Destroy(gameObject);
    }
    public void SetSpeed(float s) => speed = s;
    public void SetHitBarType(HitBarType type) => hitBarType = type;
}