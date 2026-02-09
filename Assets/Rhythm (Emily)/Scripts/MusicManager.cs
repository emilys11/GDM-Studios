using UnityEngine;
using System;
public class MusicManager : MonoBehaviour
{
   public float bpm = 120f;
    public static event Action<double> OnBeat;

    private double nextBeatDspTime;
    private double secondsPerBeat;

    void Start()
    {
        secondsPerBeat = 60.0 / bpm;
        nextBeatDspTime = AudioSettings.dspTime + secondsPerBeat;
    }

    void Update()
    {
        double dspTime = AudioSettings.dspTime;

        if (dspTime >= nextBeatDspTime)
        {
            OnBeat?.Invoke(nextBeatDspTime);
            nextBeatDspTime += secondsPerBeat;
        }
    }
}
