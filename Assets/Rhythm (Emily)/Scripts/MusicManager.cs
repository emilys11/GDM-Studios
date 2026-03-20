using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public static float bpm = 120f;
    public float bpmGame = 120f;

    [SerializeField] private float startDelay = 2f; // 🎯 Adjustable delay before music starts

    public static event Action<double> OnBeat;
    public static event Action OnMusicFinished;
    public static AudioSource audiosource;

    public bool musicStarted = false;

    public static double SecondsPerBeat { get; private set; }
    public static double nextBeatDspTime;

    public static double songStartDspTime; // 🔥 useful for countdowns/UI

    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        bpm = bpmGame;
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
        songStartDspTime = AudioSettings.dspTime + startDelay;

        audiosource.PlayScheduled(songStartDspTime);

        nextBeatDspTime = songStartDspTime + SecondsPerBeat;
    }

    void MusicDeath()
    {
        musicStarted = false;
        audiosource.Stop();
        nextBeatDspTime = 0;
    }

    void Update()
    {
        if (!musicStarted) return;

        if (!audiosource.isPlaying && AudioSettings.dspTime > songStartDspTime)
        {
            musicStarted = false;
            OnMusicFinished?.Invoke();
            return;
        }

        double dspTime = AudioSettings.dspTime;

        if (dspTime >= nextBeatDspTime)
        {
            OnBeat?.Invoke(nextBeatDspTime);
            nextBeatDspTime += SecondsPerBeat;
        }
    }

    public static void IncreaseBPM(float amount)
    {
        bpm += amount;
        SecondsPerBeat = 60.0 / bpm;

        if (audiosource != null && audiosource.isPlaying)
        {
            double dspTime = AudioSettings.dspTime;

            double beatsSinceStart = Math.Floor((dspTime - songStartDspTime) / SecondsPerBeat);
            nextBeatDspTime = songStartDspTime + (beatsSinceStart + 1) * SecondsPerBeat;
        }
    }

    public static void ResetBPM(float value)
    {
        bpm = value;
        SecondsPerBeat = 60.0 / bpm;

        if (audiosource != null && audiosource.isPlaying)
        {
            double dspTime = AudioSettings.dspTime;

            double beatsSinceStart = Math.Floor((dspTime - songStartDspTime) / SecondsPerBeat);
            nextBeatDspTime = songStartDspTime + (beatsSinceStart + 1) * SecondsPerBeat;
        }
    }
}