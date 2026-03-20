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
        new Color(1f, 0.3f, 0f),
        Color.red,
        new Color(0.7f, 0f, 1f),
        new Color(0f, 1f, 0.2f),
        Color.yellow,
        Color.white
    };

    private Renderer rend;
    private Material mat;

    private bool overrideActive = false;
    private Color overrideColor = Color.black;
    private float overrideEmission = 0f;

    private float defaultEmission;
    private float currentEmission;
    private Color currentColor = Color.white;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.EnableKeyword("_EMISSION");

        defaultEmission = emissionIntensity;
        currentEmission = emissionIntensity;

        if (danceColors.Length > 0)
            currentColor = danceColors[Random.Range(0, danceColors.Length)];

        ApplyCurrentVisuals();
        InvokeRepeating(nameof(ChangeColor), interval, interval);
    }

    void ChangeColor()
    {
        if (mat == null) return;

        if (!overrideActive && danceColors.Length > 0)
            currentColor = danceColors[Random.Range(0, danceColors.Length)];

        ApplyCurrentVisuals();
    }

    private void ApplyCurrentVisuals()
    {
        if (mat == null) return;

        if (overrideActive)
        {
            mat.SetColor("_BaseColor", overrideColor);
            mat.SetColor("_EmissionColor", overrideColor * overrideEmission);
        }
        else
        {
            mat.SetColor("_BaseColor", currentColor);
            mat.SetColor("_EmissionColor", currentColor * currentEmission);
        }
    }

    public void SetOverride(Color color, float emission)
    {
        overrideActive = true;
        overrideColor = color;
        overrideEmission = emission;
        ApplyCurrentVisuals();
    }

    public void ClearOverride()
    {
        overrideActive = false;
        ApplyCurrentVisuals();
    }

    public void SetEmissionMultiplier(float multiplier)
    {
        currentEmission = defaultEmission * multiplier;
        ApplyCurrentVisuals();
    }

    public void ResetEmission()
    {
        currentEmission = defaultEmission;
        ApplyCurrentVisuals();
    }
}