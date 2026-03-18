using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string id;
        public AudioClip clip;
    }

    public List<Sound> sounds = new List<Sound>();

    private Dictionary<string, AudioClip> soundDict;
    private AudioSource audioSource;

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += PlayNoteHit;
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();

        soundDict = new Dictionary<string, AudioClip>();

        foreach (var s in sounds) soundDict[s.id] = s.clip;
    }

    public void Play(string id)
    {
        if (soundDict.ContainsKey(id))
        {
            audioSource.PlayOneShot(soundDict[id]);
        }
    }

    void PlayNoteHit()
    {
        Play("noteHit");
    }
}
