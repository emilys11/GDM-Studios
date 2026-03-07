using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public float bpm = 120f;

    public static event Action<double> OnBeat;
    public static event Action OnMusicFinished;
    public static AudioSource audiosource;

    public bool musicStarted = false;   

    public static double SecondsPerBeat { get; private set; }

    private double nextBeatDspTime;

    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        RhythmEvents.OnReady += StartBeat;
        RhythmEvents.OnDeath += MusicDeath;
        RhythmEvents.OnWin += MusicDeath;
    }

    void OnDisable()
    {
        RhythmEvents.OnReady -= StartBeat;
        RhythmEvents.OnDeath -= MusicDeath;
        RhythmEvents.OnWin -= MusicDeath;
    }

    void StartBeat()
    {
        SecondsPerBeat = 60.0 / bpm;
        musicStarted = true;
        audiosource.Play();
        nextBeatDspTime = AudioSettings.dspTime + SecondsPerBeat;
    }

    void MusicDeath()
    {
        musicStarted = false;
        audiosource.Stop();
        nextBeatDspTime = 0;
    }

    void Update()
    {
        if (!audiosource.isPlaying && musicStarted)
        {
            musicStarted = false;
            OnMusicFinished?.Invoke();
            return;
        }

        double dspTime = AudioSettings.dspTime;

        if (dspTime >= nextBeatDspTime && musicStarted)
        {
            OnBeat?.Invoke(nextBeatDspTime);
            nextBeatDspTime += SecondsPerBeat;
        }
    }
}