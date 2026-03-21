using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{

    [System.Serializable]
    public class Sound
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<Sound> sounds = new List<Sound>();

    private Dictionary<string, Sound> soundDict;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    public int poolSize = 5;

    private List<AudioSource> sources = new List<AudioSource>();

    [Header("Spam Protection")]
    public float minInterval = 0.05f; 
    private Dictionary<string, float> lastPlayTime = new Dictionary<string, float>();

    void Awake()
    {

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.volume = 2f;
            src.playOnAwake = false;
            sources.Add(src);
        }

        soundDict = new Dictionary<string, Sound>();

        foreach (var s in sounds)
        {
            if (string.IsNullOrEmpty(s.id))
            {
                Debug.LogWarning("Sound with empty ID detected.");
                continue;
            }

            if (soundDict.ContainsKey(s.id))
            {
                Debug.LogWarning($"Duplicate sound ID: {s.id}");
                continue;
            }

            soundDict.Add(s.id, s);
        }

    }

    void OnEnable()
    {
        RhythmEvents.OnNoteHit += PlayNoteHit;
        RhythmEvents.OnHealthNoteHit += PlayNoteHit;
        RhythmEvents.OnNoteMiss += PlayNoteMiss;
        RhythmEvents.OnCombo += PlayCombo;
        RhythmEvents.OnWin += PlayWin;
        RhythmEvents.OnDeath += PlayDeath;
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteHit -= PlayNoteHit;
        RhythmEvents.OnHealthNoteHit -= PlayNoteHit;
        RhythmEvents.OnNoteMiss -= PlayNoteMiss;
        RhythmEvents.OnCombo -= PlayCombo;
        RhythmEvents.OnWin -= PlayWin;
        RhythmEvents.OnDeath -= PlayDeath;
    }

    AudioSource GetAvailableSource()
    {
        foreach (var src in sources)
        {
            if (!src.isPlaying) return src;
        }
        return sources[0];
    }

    public void Play(string id)
    {
        if (!soundDict.TryGetValue(id, out Sound s) || s.clip == null)
        {
            return;
        }

        if (!lastPlayTime.ContainsKey(id)) lastPlayTime[id] = -999f;

        if (Time.time - lastPlayTime[id] < minInterval) return;

        lastPlayTime[id] = Time.time;

        AudioSource src = GetAvailableSource();
        src.PlayOneShot(s.clip, s.volume);
    }

    void PlayNoteHit()
    {
        Play("hit");
    }

    void PlayNoteMiss()
    {
        Play("miss");
    }

    void PlayCombo()
    {
        Play("combo");
    }

    void PlayWin()
    {
        Play("win");
    }

    void PlayDeath()
    {
        Play("death");
    }
}