using UnityEngine;
using System;
using UnityEngine.UI;
public class RegularNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private double hitWindow = 0.12;

    private RectTransform rect;
    private NoteLane lane;
    private double hitDspTime;

    private bool isResolved;

    [SerializeField] private float missLineY = -450f;

    [SerializeField] private float hitLineY = 0f;
    [SerializeField] private float hitYWindow = 60f;

    [SerializeField] private Image image;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        image.sprite = NoteSkinManager.CurrentSkin.regularNote;
    }

    void Update()
    {
        if (isResolved) return;

        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        double current = AudioSettings.dspTime;

        if (!isResolved && rect.anchoredPosition.y < missLineY)
        {
            Miss();
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetLane(NoteLane l)
    {
        lane = l;
        lane.Register(this);
    }

    public void SetHitTime(double dspTime)
    {
        hitDspTime = dspTime;
    }

    public bool TryResolve()
    {
        if (isResolved) return false;

        double current = AudioSettings.dspTime;
        double error = Math.Abs(current - hitDspTime);

        float yError = Mathf.Abs(rect.anchoredPosition.y - hitLineY);

        if (error <= hitWindow && yError <= hitYWindow)
        {
            Hit();
            return true;
        }

        Miss();
        return false;
    }

    void Hit()
    {
        isResolved = true;
        lane.PlayHit();
        RhythmEvents.NoteHit();
        Destroy(gameObject);
    }

    void Miss()
    {
        if (isResolved) return;
        isResolved = true;
        RhythmEvents.NoteMissed();
        lane.PlayMiss();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (lane != null)
            lane.Unregister(this);
    }
}