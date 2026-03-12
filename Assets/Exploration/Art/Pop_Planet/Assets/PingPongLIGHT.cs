using UnityEngine;

public class SmoothLightDimmer : MonoBehaviour
{
    public Light myLight;
    public float speed = 1f;
    public float minIntensity = 0f;
    public float maxIntensity = 3f;

    void Update()
    {
        float wave = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, wave);
    }
}