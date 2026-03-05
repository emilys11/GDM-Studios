using UnityEngine;
using System;

public class EvilNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private double hitWindow = 0.12;

    private RectTransform rect;
    private NoteLane lane;
    private double hitDspTime;

    private bool isResolved;

    [SerializeField] private float missLineY = -400f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isResolved) return;

        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        double current = AudioSettings.dspTime;

       if (!isResolved && rect.anchoredPosition.y < missLineY)
        {
            isResolved = true;

            RhythmEvents.NoteHit();

            Destroy(gameObject);
        }
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

        if (error <= hitWindow)
        {
            isResolved = true;
            RhythmEvents.BadInput();
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    void OnDestroy()
    {
        if (lane != null)
            lane.Unregister(this);
    }
}