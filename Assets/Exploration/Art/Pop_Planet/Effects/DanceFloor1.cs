using UnityEngine;

public class DanceFloor1 : MonoBehaviour
{
    [Header("Sync Settings")]
    public float speed = 1f;   // Set this to the same value as SmoothLightDimmer

    [Header("Glow Settings")]
    public float emissionIntensity = 3f;

    [Header("Dancefloor Colors")]
    public Color colorA = new Color(1f, 0.3f, 0f); // Orange
    public Color colorB = Color.red;               // Red

    private Renderer rend;
    private Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float wave = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        Color currentColor = Color.Lerp(colorA, colorB, wave);

        mat.SetColor("_BaseColor", currentColor);
        mat.SetColor("_EmissionColor", currentColor * emissionIntensity);
    }
}