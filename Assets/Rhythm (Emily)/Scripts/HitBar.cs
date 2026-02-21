using UnityEngine;

public class HitBar : MonoBehaviour
{
    [SerializeField] private HitBarType hitBarType;
    [SerializeField] private KeyCode key;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            RhythmInput.Fire(hitBarType);
        }
    }
}