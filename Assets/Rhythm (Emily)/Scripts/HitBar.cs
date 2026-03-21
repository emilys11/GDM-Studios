using System.Diagnostics;
using UnityEngine;

public class HitBar : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    private bool isReady = false;

    private NoteLane lane;
    private float inputBufferTime = 0.1f;
    private float lastInputTime;

    void Awake()
    {
        lane = GetComponent<NoteLane>();
    }

    void OnEnable()
    {
        RhythmEvents.OnReady += IsReady;
        RhythmEvents.OnDeath += IsNotReady;
        RhythmEvents.OnWin += IsReady;
    }

    void OnDisable()
    {
        RhythmEvents.OnReady -= IsReady;
        RhythmEvents.OnDeath -= IsNotReady;
        RhythmEvents.OnWin -= IsReady;
    }

    void IsReady()
    {
        isReady = true;
    }

    void IsNotReady()
    {
        isReady = false;
    }



    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            lastInputTime = Time.time;
            UnityEngine.Debug.Log("key pressed: "+key);
        }

        if (isReady && Time.time - lastInputTime <= inputBufferTime)
        {
            lane.HandleInput();
            lastInputTime = -999f;
        }
    }

    public bool IsKeyHeld()
    {
        return Input.GetKey(key);
    }
}