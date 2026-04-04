using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    [Range(0.8f, 1.2f)] public float minPitch = 0.95f;
    [Range(0.8f, 1.2f)] public float maxPitch = 1.05f;
    [Range(0f, 1f)] public float volume = 1f;

    private int lastIndex = -1;

    public void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, footstepClips.Length);
        }
        while (footstepClips.Length > 1 && index == lastIndex);

        lastIndex = index;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(footstepClips[index], volume);
    }
}