using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zombieWaveSystem : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnpoints;
    public float timeBetweenwaves = 10f;
    [SerializeField] private float waveTimer = 0f;
    private int waveNumber = 1;
    public int zombiesPerwave = 4;

    [Header("UI")]
    public Text WaveNumber;
    public Text WaveTimer;

    void Update()
    {
        if (waveNumber == 8)
            return;

        waveTimer += Time.deltaTime;

        int intValue = Mathf.RoundToInt(waveTimer);

        WaveTimer.text = intValue.ToString();
        

        if (waveTimer >= timeBetweenwaves)
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
        WaveNumber.text = waveNumber.ToString();
    }
}
