using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicVolumeZone : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicVolumeParameter = "MusicVolume";

    [SerializeField] private float normalVolume = 0f;
    [SerializeField] private float loweredVolume = -15f;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartFade(loweredVolume);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartFade(normalVolume);
    }

    private void StartFade(float targetVolume)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMixerVolume(targetVolume));
    }

    private IEnumerator FadeMixerVolume(float targetVolume)
    {
        audioMixer.GetFloat(musicVolumeParameter, out float currentVolume);

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolume, targetVolume, time / fadeDuration);
            audioMixer.SetFloat(musicVolumeParameter, newVolume);
            yield return null;
        }

        audioMixer.SetFloat(musicVolumeParameter, targetVolume);
    }
}