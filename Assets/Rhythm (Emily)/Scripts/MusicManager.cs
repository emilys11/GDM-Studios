using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public float bpm = 120f;

    public static event Action<double> OnBeat;
    public static AudioSource audiosource;

    public static double SecondsPerBeat { get; private set; }

    private double nextBeatDspTime;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();

        SecondsPerBeat = 60.0 / bpm;

        audiosource.Play();
        nextBeatDspTime = AudioSettings.dspTime + SecondsPerBeat;
    }

    void Update()
    {
        if (!audiosource.isPlaying)
            return;

        double dspTime = AudioSettings.dspTime;

        if (dspTime >= nextBeatDspTime)
        {
            OnBeat?.Invoke(nextBeatDspTime);
            nextBeatDspTime += SecondsPerBeat;
        }
    }
}