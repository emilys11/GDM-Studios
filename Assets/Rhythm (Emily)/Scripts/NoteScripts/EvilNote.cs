using UnityEngine;
using System;
using UnityEngine.UI;

public class EvilNote : MonoBehaviour, INote
{
    [SerializeField] private float speed = 400f;
    [SerializeField] private double hitWindow = 0.12;

    private RectTransform rect;
    private NoteLane lane;
    private double hitDspTime;
    private bool isResolved;

    [SerializeField] private Image image;
    [SerializeField] private float missLineY = -400f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.sprite = NoteSkinManager.CurrentSkin.evilNote;
    }

    void Update()
    {
        if (isResolved) return;

        rect.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (lane != null && lane.IsFirstNote(this) && rect.anchoredPosition.y < missLineY)
        {
            isResolved = true;
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float s) => speed = s;

    public void SetLane(NoteLane l)
    {
        lane = l;
        lane.Register(this);
    }

    public void SetHitTime(double dspTime) => hitDspTime = dspTime;

    public bool TryResolve()
    {
        if (isResolved) return false;
        if (lane != null && !lane.IsFirstNote(this)) return false;

        double error = Math.Abs(AudioSettings.dspTime - hitDspTime);

        if (error <= hitWindow)
        {
            isResolved = true;
            RhythmEvents.BadInput();
            lane.PlayMiss();
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    void OnDestroy()
    {
        lane?.Unregister(this);
    }
}