using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        MoveToSpawn();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        MoveToSpawn();
    }

    private void MoveToSpawn()
    {
        Debug.Log("PlayerSpawner running");
        Debug.Log("Looking for spawn ID: " + SpawnData.nextSpawnID);

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

        Debug.Log("Found " + spawnPoints.Length + " spawn points in scene.");

        foreach (SpawnPoint sp in spawnPoints)
        {
            Debug.Log("Spawn point found: " + sp.spawnID + " at " + sp.transform.position);

            if (sp.spawnID == SpawnData.nextSpawnID)
            {
                Debug.Log("Match found. Moving player to: " + sp.spawnID);

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                transform.position = sp.transform.position;
                transform.rotation = sp.transform.rotation;
                return;
            }
        }

        Debug.LogWarning("No matching spawn point found for ID: " + SpawnData.nextSpawnID);
    }
}