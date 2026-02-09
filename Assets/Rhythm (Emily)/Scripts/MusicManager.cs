using UnityEngine;
using System;
public class MusicManager : MonoBehaviour
{
    public float bpm = 120f;
    public static event Action<double> OnBeat;
    public static AudioSource audiosource;

    private double nextBeatDspTime;
    private double secondsPerBeat;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        secondsPerBeat = 60.0 / bpm;
        nextBeatDspTime = AudioSettings.dspTime + secondsPerBeat;
    }

    void Update()
    {

        if (!GetComponent<AudioSource>().isPlaying) OnBeat = null;
        
        double dspTime = AudioSettings.dspTime;

        if (dspTime >= nextBeatDspTime)
        {
            OnBeat?.Invoke(nextBeatDspTime);
            nextBeatDspTime += secondsPerBeat;
        }
    }
}
