using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject evilNotePrefab;
    [SerializeField] private GameObject holdNotePrefab;

    [SerializeField] private GameObject healthNotePrefab;

    [SerializeField] private RectTransform[] laneParents;
    [SerializeField] private NoteLane[] lanes;

    [SerializeField] private float spawnY = 600f;
    [SerializeField] private float hitY = -350f;

    private float originalSpeed;
    [SerializeField] private float noteSpeed = 400f;
    [SerializeField] private float holdNoteCompressor = 3.4f;

    [Header("Speed Progression")]
    [SerializeField] private int speedIncreaseAfter = 20;
    [SerializeField] private float speedIncreaseAmount = 50f;
    [SerializeField] private float bpmIncrease = 0;
    [SerializeField] private float bpmOriginal = 0;

    private int notesSpawned = 0;

    [Range(0f, 1f)] [SerializeField] private float evilChance = 0.15f;
    [Range(0f, 1f)] [SerializeField] private float holdChance = 0.20f;
    [Range(0f, 1f)] [SerializeField] private float healthChance = 0.01f;
    void OnEnable()
    {
        originalSpeed = noteSpeed;
        bpmOriginal = MusicManager.bpm;
        MusicManager.OnBeat += HandleBeat;
        RhythmEvents.OnReady += ResetSpeed;
    }

    void OnDisable()
    {
        MusicManager.OnBeat -= HandleBeat;
        RhythmEvents.OnReady -= ResetSpeed;
    }

    void ResetSpeed()
    {
        noteSpeed = originalSpeed;
        notesSpawned = 0;
        MusicManager.ResetBPM(bpmOriginal);
    }

    void HandleBeat(double beatDspTime)
    {
        float travelTime = Mathf.Abs(spawnY - hitY) / noteSpeed;
        double spawnTime = beatDspTime - travelTime;

        StartCoroutine(SpawnBeforeBeat(spawnTime, beatDspTime));
    }

    IEnumerator SpawnBeforeBeat(double spawnDspTime, double hitDspTime)
    {
        while (AudioSettings.dspTime < spawnDspTime)
            yield return null;

        SpawnRandomNote(hitDspTime);
    }

    void SpawnRandomNote(double hitDspTime)
    {
        int laneIndex = Random.Range(0, laneParents.Length);
        GameObject prefab = ChoosePrefab();

        GameObject noteObj = Instantiate(prefab, laneParents[laneIndex]);

        RectTransform rect = noteObj.GetComponent<RectTransform>();
        INote note = noteObj.GetComponent<INote>();

        float y = spawnY;

        if (note is HoldNote hold)
        {
            hold.heightDeductor = holdNoteCompressor;

            Canvas.ForceUpdateCanvases();

            float height = rect.rect.height;
            y += height * 0.5f;
        }



        rect.anchoredPosition = new Vector2(0, y);


        if (note != null)
        {
            note.SetSpeed(noteSpeed);
            note.SetLane(lanes[laneIndex]);
            note.SetHitTime(hitDspTime);

            if(note is HoldNote)
            {
                noteObj.GetComponent<HoldNote>().heightDeductor = holdNoteCompressor;
            }
        }

        notesSpawned++;

        if(noteSpeed == originalSpeed)
        {
            CheckSpeedIncrease();
        }
        
    }

    void CheckSpeedIncrease()
    {
        if (notesSpawned >= speedIncreaseAfter)
        {
            MusicManager.IncreaseBPM(bpmIncrease);
            noteSpeed += speedIncreaseAmount;

        }
    }

    GameObject ChoosePrefab()
    {
        float r = Random.value;

        if (evilNotePrefab != null && r < evilChance)
            return evilNotePrefab;

        if (holdNotePrefab != null && r < evilChance + holdChance)
            return holdNotePrefab;

        if (healthNotePrefab != null && r < evilChance + holdChance + healthChance)
            return healthNotePrefab;

        return notePrefab;
    }
}