using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    public static Action OnWaveCompleted;
    
    [Header("Settings")]
    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBetweenWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBetweenSpawns;
    
    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    [Header("Poolers")]
    [SerializeField] private ObjectPooler enemyWave10Pooler;
    [SerializeField] private ObjectPooler enemyWave11To20Pooler;
    [SerializeField] private ObjectPooler enemyWave21To30Pooler;
    [SerializeField] private ObjectPooler enemyWave31To40Pooler;
    [SerializeField] private ObjectPooler enemyWave41To50Pooler;

    private float spawnTimer;
    private int enemiesSpawned;
    private int enemiesRemaining;
    
    private Waypoint waypoint;

    private void Start()
    {
        waypoint = GetComponent<Waypoint>();

        enemiesRemaining = enemyCount;
    }
    
    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            spawnTimer = GetSpawnDelay();

            if (enemiesSpawned < enemyCount)
            {
                enemiesSpawned++;
                
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = GetPooler().GetInstanceFromPool();
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.Waypoint = waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;
        
        newInstance.SetActive(true);
    }

    private float GetSpawnDelay()
    {
        float delay = 0f;

        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBetweenSpawns;
        }

        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }

    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);

        return randomTimer;
    }

    private ObjectPooler GetPooler()
    {
        int currentWave = LevelManager.Instance.CurrentWave;

        if (currentWave <= 10)
        {
            return enemyWave10Pooler;
        }
        
        if (currentWave > 10 && currentWave <= 20)
        {
            return enemyWave11To20Pooler;
        }
        
        if (currentWave > 20 && currentWave <= 30)
        {
            return enemyWave21To30Pooler;
        }
        
        if (currentWave > 30 && currentWave <= 40)
        {
            return enemyWave31To40Pooler;
        }
        
        if (currentWave > 40 && currentWave <= 50)
        {
            return enemyWave41To50Pooler;
        }

        return null;
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        enemiesRemaining = enemyCount;
        spawnTimer = 0f;
        enemiesSpawned = 0;
    }

    private void RecordEnemy(Enemy enemy)
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            OnWaveCompleted?.Invoke();
            
            StartCoroutine(NextWave());
        }
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}
