using UnityEngine;
using System;
using UnityEngine.UI;
public class HealthNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private double hitWindow = 0.3f;

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

        if (rect.anchoredPosition.y < missLineY)
        {
            Miss();
        }
    }

    void Miss()
    {
        if (isResolved) return;
        isResolved = true;
        UnityEngine.Debug.Log("Missed from: " + gameObject.name);
        RhythmEvents.NoteMissed();
        lane.PlayMiss();
        Destroy(gameObject);
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
        double error = current - hitDspTime;

        if (Math.Abs(error) <= hitWindow)
        {
            Hit();
            return true;
        }

        return false;
    }

    void Hit()
    {
        isResolved = true;
        lane.PlayHit();
        RhythmEvents.HealthNoteHit();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (lane != null)
            lane.Unregister(this);
    }
}