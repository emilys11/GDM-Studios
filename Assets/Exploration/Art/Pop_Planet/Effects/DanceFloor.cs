using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Header("Timing")]
    public float interval = 0.5f;

    [Header("Glow Settings")]
    public float emissionIntensity = 3f;

    [Header("Dancefloor Colors - used when Simon is NOT active")]
    public Color[] danceColors =
    {
        new Color32(255, 77, 0, 255),    // orange
        new Color32(255, 0, 0, 255),     // red
        new Color32(179, 0, 255, 255),   // purple
        new Color32(0, 255, 80, 255),    // green
        new Color32(255, 255, 0, 255),   // yellow
        Color.white
    };

    [Header("Simon Game Idle State")]
    public bool isCenterTile = false;
    public Color simonIdleColor = Color.grey;

    [Header("Section Color")]
    public int sectionIndex = 0;
    public Color[] sectionColors =
    {
        new Color32(250, 13, 13, 255),   // red    #FA0D0D
        new Color32(0, 255, 255, 255),   // blue   #00FFFF
        new Color32(0, 255, 70, 255),    // green  #00FF46
        new Color32(223, 255, 0, 255)    // yellow #DFFF00
    };

    private Renderer rend;
    private Material mat;

    private bool overrideActive = false;
    private Color overrideColor = Color.black;
    private float overrideEmission = 0f;

    private float defaultEmission;
    private float currentEmission;

    private bool simonGameActive = false;
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
        InvokeRepeating(nameof(ChangeColor), 0f, interval);
    }

    void ChangeColor()
    {
        if (mat == null) return;

        if (overrideActive)
        {
            ApplyCurrentVisuals();
            return;
        }

        if (simonGameActive)
        {
            if (isCenterTile)
            {
                if (sectionIndex >= 0 && sectionIndex < sectionColors.Length)
                {
                    currentColor = sectionColors[sectionIndex];
                    currentEmission = defaultEmission;
                }
                else
                {
                    currentColor = simonIdleColor;
                    currentEmission = defaultEmission * 0.4f;
                }
            }
            else
            {
                currentColor = simonIdleColor;
                currentEmission = defaultEmission * 0.4f;
            }
        }
        else
        {
            if (danceColors.Length > 0)
                currentColor = danceColors[Random.Range(0, danceColors.Length)];

            currentEmission = defaultEmission;
        }

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
        ChangeColor();
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

    public void SetSimonGameActive(bool active)
    {
        simonGameActive = active;
        ChangeColor();
    }
}