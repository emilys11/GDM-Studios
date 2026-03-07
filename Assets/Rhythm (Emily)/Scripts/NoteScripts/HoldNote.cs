using UnityEngine;
using System;
using UnityEngine.UI;
public class HoldNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private double hitWindow = 0.12;
    [SerializeField] private double holdBeats = 2.0;

    private RectTransform rect;
    private NoteLane lane;

    private double hitDspTime;
    private double holdDuration;
    private double holdStartTime;

    private bool isResolved;
    private bool holding;

    private float originalHeight;

    [SerializeField] private float heightDeductor = 0.9f;

    [SerializeField] private float missLineY = -450f;

    [SerializeField] private Image image;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalHeight = rect.sizeDelta.y;
        image = GetComponent<Image>();

        image.sprite = NoteSkinManager.CurrentSkin.holdNote;
    }

    void Update()
    {
        if (isResolved) return;

        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        double current = AudioSettings.dspTime;

        if (!holding && rect.anchoredPosition.y < missLineY)
        {
            Miss();
        }

        if (holding)
        {
            if (!lane.IsKeyHeld())
            {
                Miss();
                return;
            }

            double elapsed = current - holdStartTime;
            double progress = Math.Clamp(elapsed / holdDuration * heightDeductor, 0.0, 1.0);

            float newHeight = (float)(originalHeight * (1.0 - progress));
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

            if (progress >= 0.998f)
            {
                Complete();
            }
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
        holdDuration = holdBeats * MusicManager.SecondsPerBeat;
    }

    public bool TryResolve()
    {
        if (isResolved || holding) return false;

        double error = Math.Abs(AudioSettings.dspTime - hitDspTime);

        if (error <= hitWindow)
        {
            holding = true;
            holdStartTime = AudioSettings.dspTime;
            return true;
        }

        return false;
    }

    void Complete()
    {
        isResolved = true;
        RhythmEvents.NoteHit();
        Destroy(gameObject);
    }

    void Miss()
    {
        if (isResolved) return;
        isResolved = true;
        RhythmEvents.NoteMissed();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (lane != null) lane.Unregister(this);
    }
}