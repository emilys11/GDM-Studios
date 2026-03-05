using UnityEngine;

public class HitBar : MonoBehaviour
{
    [SerializeField] private KeyCode key;

    private NoteLane lane;

    void Awake()
    {
        lane = GetComponent<NoteLane>();
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            lane.HandleInput();
        }
    }

    public bool IsKeyHeld()
    {
        return Input.GetKey(key);
    }
}