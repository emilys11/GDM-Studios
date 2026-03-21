using UnityEngine;
using System;
using UnityEngine.UI;
public class RegularNote : MonoBehaviour, INote
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

        
        if (lane != null && lane.IsFirstNote(this) && rect.anchoredPosition.y < missLineY)
        {
            isResolved = true;
            RhythmEvents.NoteMissed(); 
            lane.PlayMiss();
            Destroy(gameObject);
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
        double error = current - hitDspTime;

        float y = MathF.Abs(rect.anchoredPosition.y);


        if (y <= hitLineY + hitYWindow) //not too early or late
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
        UnityEngine.Debug.Log("Missed from: " + gameObject.name);
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