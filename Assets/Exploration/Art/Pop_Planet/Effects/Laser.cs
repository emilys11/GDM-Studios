using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DiscoLaserStrobeSweep : MonoBehaviour
{
    [Header("Strobe")]
    public float strobeHz = 12f;
    [Range(0f, 1f)] public float duty = 0.5f;

    [Header("Disco Color")]
    public bool rainbow = true;
    public float rainbowSpeed = 2f;

    [Header("Width Pulse")]
    public bool pulseWidth = true;
    public float widthPulseSpeed = 8f;
    public float widthMin = 0.02f;
    public float widthMax = 0.08f;

    [Header("Sweep")]
    public bool sweep = true;
    public float sweepAngle = 60f;   // total angle range
    public float sweepSpeed = 2f;    // how fast it moves

    private LineRenderer lr;
    private float strobeTimer;
    private Quaternion baseRotation;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = true;
        baseRotation = transform.rotation;
    }

    void Update()
    {
        //SWEEP MOTION
        if (sweep)
        {
            float angle = Mathf.Sin(Time.time * sweepSpeed) * (sweepAngle * 0.5f);
            transform.rotation = baseRotation * Quaternion.Euler(0f, angle, 0f);
        }

        //STROBE
        float period = 1f / Mathf.Max(0.01f, strobeHz);
        strobeTimer += Time.deltaTime;
        if (strobeTimer >= period) strobeTimer -= period;

        bool on = (strobeTimer / period) < duty;
        lr.enabled = on;

        if (!on) return;

        //RAINBOW COLOR
        if (rainbow)
        {
            float h = Mathf.Repeat(Time.time * rainbowSpeed, 1f);
            Color c = Color.HSVToRGB(h, 1f, 1f);
            lr.startColor = c;
            lr.endColor = c;
        }

        //WIDTH PULSE
        if (pulseWidth)
        {
            float t = (Mathf.Sin(Time.time * widthPulseSpeed) + 1f) * 0.5f;
            float w = Mathf.Lerp(widthMin, widthMax, t);
            lr.startWidth = w;
            lr.endWidth = w;
        }
    }
}