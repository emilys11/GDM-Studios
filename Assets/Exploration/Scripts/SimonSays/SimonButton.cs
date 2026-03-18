using System.Collections;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    public int buttonIndex;
    public SimonSaysManager manager;

    [Header("Visuals")]
    public Renderer targetRenderer;
    public float flashDuration = 0.4f;

    [Header("Emission")]
    public Color emissionColor = Color.red;
    public float emissionIntensity = 8f;

    [Header("Press Animation")]
    public float pressDepth = 0.08f;

    private Material mat;
    private bool interactable = false;
    private bool isPressed = false;
    private Vector3 originalLocalPosition;

    private void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            mat = targetRenderer.material;
            mat.EnableKeyword("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            SetEmissionOff();
        }
        else
        {
            Debug.LogError($"{name}: No targetRenderer assigned.");
        }

        originalLocalPosition = transform.localPosition;
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
    }

    public IEnumerator Flash()
    {
        SetEmissionOn();
        transform.localPosition = originalLocalPosition + Vector3.down * pressDepth;

        yield return new WaitForSeconds(flashDuration);

        SetEmissionOff();
        transform.localPosition = originalLocalPosition;
    }

    private void SetEmissionOn()
    {
        if (mat == null) return;

        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
    }

    private void SetEmissionOff()
    {
        if (mat == null) return;

        mat.SetColor("_EmissionColor", Color.black);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!interactable || isPressed) return;
        if (!other.CompareTag("Player")) return;

        isPressed = true;
        StartCoroutine(PressRoutine());
    }

    private IEnumerator PressRoutine()
    {
        yield return Flash();
        manager.PlayerPressed(buttonIndex);
        yield return new WaitForSeconds(0.15f);
        isPressed = false;
    }
}