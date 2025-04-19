using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;     
    [SerializeField] private Transform[] spawnPoints;    
    [SerializeField] private int baseAmount = 1;         

    private void Start()
    {
        SpawnEnemies(1);
    }

    public void SpawnEnemies(int round)
    {
        int totalToSpawn = baseAmount + (round - 1);

        for (int i = 0; i < totalToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}