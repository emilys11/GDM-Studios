using System.Net.NetworkInformation;
using UnityEngine;

public class HoldNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private float hitWindow = 50f;
    [SerializeField] private float missY = -450f;

    [SerializeField] private float holdDuration = 1.0f;

    [SerializeField] private int scoreOnComplete = 250;

    private HitBarType hitBarType;
    public RectTransform hitbarTransform;

    private RectTransform rect;
    private bool isResolved;

    private bool holding;
    private float heldTime;

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

        if (!holding && rect.anchoredPosition.y < missY)
        {
            Miss();
            return;
        }

        if (holding)
        {
            if (!IsLaneKeyHeld())
            {
                // released early
                Miss();
                return;
            }

            heldTime += Time.deltaTime;
            if (heldTime >= holdDuration)
            {
                Complete();
            }
        }


    }

    void OnInput(HitBarType inputType)
    {
        if (isResolved) return;
        if (holding) return;
        if (inputType != hitBarType) return;

        float dist = Mathf.Abs(rect.anchoredPosition.y);

        if (dist <= hitWindow)
        {
            holding = true;
            heldTime = 0f;
        }
        else
        { 
            RhythmEvents.BadInput();
        }
    }

    public void Hit()
    {
        Complete();
    }

    private bool IsLaneKeyHeld()
    {
        switch (hitBarType)
        {
            case HitBarType.first:  return Input.GetKey(KeyCode.D);
            case HitBarType.second: return Input.GetKey(KeyCode.F);
            case HitBarType.third:  return Input.GetKey(KeyCode.J);
            case HitBarType.fourth: return Input.GetKey(KeyCode.K);
            default: return false;
        }
    }

     private void Complete()
    {
        if (isResolved) return;
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