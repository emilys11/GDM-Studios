using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private bool debugSpawns = true;


    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject evilNotePrefab;
    [SerializeField] private GameObject holdNotePrefab;

    [SerializeField] private RectTransform[] laneParents;
    [SerializeField] private RectTransform[] hitBars;
    [SerializeField] private HitBarType[] laneTypes;

    [SerializeField] private KeyCode[] laneHoldKeys;

    [SerializeField] private float spawnY = 600f;
    [SerializeField] private float hitY = -350f;
    [SerializeField] private float noteSpeed = 400f;

    [Range(0f, 1f)] [SerializeField] private float evilChance = 0.15f;
    [Range(0f, 1f)] [SerializeField] private float holdChance = 0.20f;

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

            GameObject prefab = ChoosePrefab();


            if (debugSpawns)
            {
                Debug.Log($"[Spawner] Beat spawn. lane={lane} type={laneTypes[lane]} " +
                        $"prefab={(prefab != null ? prefab.name : "NULL")} " +
                        $"evilChance={evilChance} holdChance={holdChance}");
            }

            if (prefab == null)
            {
                Debug.LogError("[Spawner] Prefab was NULL. Check inspector assignments.");
                return;
            }


            Debug.Log($"[Spawner] About to Instantiate prefab='{prefab.name}' " +
            $"hasRegular={prefab.GetComponent<RegularNote>() != null} " +
            $"hasEvil={prefab.GetComponent<EvilNote>() != null} " +
            $"hasHold={prefab.GetComponent<HoldNote>() != null}");


            GameObject noteObj = Instantiate(prefab, laneParents[lane]);

            RectTransform rect = noteObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, spawnY);

            ConfigureSpawnedNote(noteObj, lane);
        }
    }

    GameObject ChoosePrefab()
    {
        float r = Random.value;

        // If a prefab is missing, fallback gracefully
        bool hasRegular = notePrefab != null;
        bool hasEvil = evilNotePrefab != null;
        bool hasHold = holdNotePrefab != null;

        // If only one exists, always use it
        if (hasRegular && !hasEvil && !hasHold) return notePrefab;
        if (!hasRegular && hasEvil && !hasHold) return evilNotePrefab;
        if (!hasRegular && !hasEvil && hasHold) return holdNotePrefab;

        // Weighted choice (evil -> hold -> regular)
        if (hasEvil && r < evilChance) return evilNotePrefab;
        if (hasHold && r < evilChance + holdChance) return holdNotePrefab;

        // Default fallback
        if (hasRegular) return notePrefab;
        if (hasHold) return holdNotePrefab;
        if (hasEvil) return evilNotePrefab;

        return null;
    }

    void ConfigureSpawnedNote(GameObject noteObj, int lane)
    {
        
        if (debugSpawns)
        {
            Debug.Log($"[Spawner] Spawned '{noteObj.name}' in lane {lane}. Components: " +
                    $"Regular={noteObj.GetComponent<RegularNote>() != null}, " +
                    $"Evil={noteObj.GetComponent<EvilNote>() != null}, " +
                    $"Hold={noteObj.GetComponent<HoldNote>() != null}");
        }
        
        // Configure RegularNote
        var regular = noteObj.GetComponent<RegularNote>();
        if (regular != null)
        {
            regular.SetSpeed(noteSpeed);
            regular.SetHitBarType(laneTypes[lane]);
            regular.hitbarTransform = hitBars[lane];
            return;
        }

        // Configure EvilNote
        var evil = noteObj.GetComponent<EvilNote>();
        if (evil != null)
        {
            evil.SetSpeed(noteSpeed);
            evil.SetHitBarType(laneTypes[lane]);
            evil.hitbarTransform = hitBars[lane];
            return;
        }

        // Configure HoldNote
        var hold = noteObj.GetComponent<HoldNote>();
        if (hold != null)
        {
            hold.SetSpeed(noteSpeed);
            hold.SetHitBarType(laneTypes[lane]);
            hold.hitbarTransform = hitBars[lane];
            return;
        }

        Debug.LogWarning($"Spawned note prefab '{noteObj.name}' has no RegularNote/EvilNote/HoldNote component.");
    }

}