using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    private Light _discoLight;
    [SerializeField] private float changeInterval = 0.5f;

    private void Start()
    {
        _discoLight = GetComponent<Light>();
        InvokeRepeating(nameof(ChangeLightColor), 0f, changeInterval);

    }

    private void ChangeLightColor()
    {
        var randomColor = new Color(Random.value, Random.value, Random.value);
        _discoLight.color = randomColor;
    }



}

