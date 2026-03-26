using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimonFloorSection : MonoBehaviour
{
    public int sectionIndex;
    public SimonDanceManager manager;

    [Header("Tiles in this section")]
    public ColorChanger[] tiles;

    [Header("Visuals")]
    public Color flashColor = Color.cyan;
    public float flashDuration = 0.5f;
    public float flashEmission = 8f;

    [Header("Audio")]
    [SerializeField] private AudioClip pressSound;
    [SerializeField] private float soundVolume = 1f;

    private AudioSource audioSource;
    private bool interactable = false;
    private bool isPressed = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void Reset()
    {
        tiles = GetComponentsInChildren<ColorChanger>();
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
    }

    public IEnumerator Flash()
    {
        PlayPressSound();
        SetSectionOverride(true);
        yield return new WaitForSeconds(flashDuration);
        SetSectionOverride(false);
    }

    private void SetSectionOverride(bool on)
    {
        foreach (ColorChanger tile in tiles)
        {
            if (tile == null) continue;

            if (on) tile.SetOverride(flashColor, flashEmission);
            else tile.ClearOverride();
        }
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
        manager.PlayerPressed(sectionIndex);
        yield return new WaitForSeconds(0.15f);
        isPressed = false;
    }

    private void PlayPressSound()
    {
        if (pressSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pressSound, soundVolume);
        }
    }
}