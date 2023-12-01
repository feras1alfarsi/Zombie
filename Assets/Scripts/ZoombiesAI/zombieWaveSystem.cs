using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieWaveSystem : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnpoints;
    public float timeBetweenwaves = 10f;
    [SerializeField] private float waveTimer = 0f;
    private int waveNumber;
    public int zombiesPerwave = 4;

    void Update()
    {
        if (waveNumber == 8)
            return;

        waveTimer += Time.deltaTime;

        int intValue = Mathf.RoundToInt(waveTimer);

        if(waveTimer >= timeBetweenwaves)
        {
            StartNewWave();
        }
    }

    void StartNewWave()
    {
        waveTimer = 0f;

        zombiesPerwave += 2;

        float minDistance = 4f;

        for(int i = 0; i < zombiesPerwave; i++)
        {
            
            int randomSpawnIndex = Random.Range(0, spawnpoints.Length);
            Transform spawnpoint = spawnpoints[randomSpawnIndex];

            GameObject randomZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

            Vector3 spawnPosition = spawnpoint.position + Random.insideUnitSphere * minDistance;

            spawnPosition.y = spawnpoint.position.y;

            Instantiate(randomZombiePrefab, spawnPosition, spawnpoint.rotation);
            
        }

        waveNumber++;
    }
}
