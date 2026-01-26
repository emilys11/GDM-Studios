using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefab + Timing")]
    public Note notePrefab;
    public float bpm = 120f;

    [Header("Lanes")]
    public float[] laneX = new float[] { -3f, -1f, 1f, 3f };
    public float spawnY = 5.5f;

    private float beatInterval;
    private float timer;

    void Start()
    {
        beatInterval = 60f / bpm; // seconds per beat
    }

    void Update()
    {
        timer += Time.deltaTime;

        // For now: spawn a note every beat in a random lane
        if (timer >= beatInterval)
        {
            timer -= beatInterval;
            SpawnRandom();
        }
    }

    void SpawnRandom()
    {
        int lane = Random.Range(0, laneX.Length);
        Note note = Instantiate(notePrefab, new Vector3(laneX[lane], spawnY, 0f), Quaternion.identity);
        note.laneIndex = lane;
    }


}
