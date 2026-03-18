using UnityEngine;

public class HitBar : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    private bool isReady = false;

    private NoteLane lane;

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
        if (Input.GetKeyDown(key) && isReady)
        {
            lane.HandleInput();
        }
    }

    public bool IsKeyHeld()
    {
        return Input.GetKey(key);
    }
}