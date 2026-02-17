using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public float interval = 1f; // seconds between color changes
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        InvokeRepeating(nameof(ChangeColor), 0f, interval);
    }

    void ChangeColor()
    {
        rend.material.SetColor("_BaseColor", Random.ColorHSV(
     0f, 1f,
     0.9f, 1f,
     0.9f, 1f
 ));


    }
}
