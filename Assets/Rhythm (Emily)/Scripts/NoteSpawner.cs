using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;

    [SerializeField] private RectTransform[] laneParents;
    [SerializeField] private RectTransform[] hitBars;
    [SerializeField] private HitBarType[] laneTypes;

    [SerializeField] private float spawnY = 600f;
    [SerializeField] private float hitY = -350f;
    [SerializeField] private float noteSpeed = 400f;


    void OnEnable()
    {
        MusicManager.OnBeat += HandleBeat;
    }

    void OnDisable()
    {
        MusicManager.OnBeat -= HandleBeat;
    }

    void HandleBeat(double beatDspTime)
    {
        float travelTime = Mathf.Abs(spawnY - hitY) / noteSpeed;
        StartCoroutine(SpawnBeforeBeat(beatDspTime - travelTime));
    }

    IEnumerator SpawnBeforeBeat(double spawnDspTime)
    {
        while (AudioSettings.dspTime < spawnDspTime)
            yield return null;

        SpawnRandomNote();
    }

    void SpawnRandomNote()
    {
        if(MusicManager.audiosource.isPlaying)
        {
            int lane = Random.Range(0, laneParents.Length);

            GameObject noteObj = Instantiate(notePrefab, laneParents[lane]);

            RectTransform rect = noteObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, spawnY);

            RegularNote note = noteObj.GetComponent<RegularNote>();
            note.SetSpeed(noteSpeed);
            note.SetHitBarType(laneTypes[lane]);
            note.hitbarTransform = hitBars[lane];
        }
    }
}