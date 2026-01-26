using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] lanes;       // size 4
    public float spawnInterval = 0.6f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            SpawnRandom();
        }
    }

    void SpawnRandom()
    {
        int lane = Random.Range(0, lanes.Length);
        Vector3 spawnPos = lanes[lane].position;
        Instantiate(notePrefab, spawnPos, Quaternion.identity);
    }
}
