using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Header("Timing")]
    public float interval = 0.5f;

    [Header("Glow Settings")]
    public float emissionIntensity = 3f;

    [Header("Dancefloor Colors")]
    public Color[] danceColors =
    {
        new Color(1f, 0.3f, 0f),   // Orange
        Color.red,                 // Red
        new Color(0.7f, 0f, 1f),   // Purple
        new Color(0f, 1f, 0.2f),   // Neon Green
        Color.yellow,              // Yellow
        Color.white                // White
    };

    private Renderer rend;
    private Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;

        // Enable emission
        mat.EnableKeyword("_EMISSION");

        InvokeRepeating(nameof(ChangeColor), 0f, interval);
    }

    void ChangeColor()
    {
        if (danceColors.Length == 0) return;

        Color newColor = danceColors[Random.Range(0, danceColors.Length)];

        // Base color
        mat.SetColor("_BaseColor", newColor);

        // Emission glow
        mat.SetColor("_EmissionColor", newColor * emissionIntensity);
    }
}
