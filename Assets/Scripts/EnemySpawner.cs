using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private WaveInfo[] waves;

    [Header("Attributes")]
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 0;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    [SerializeField] private Dictionary<GameObject, int> enemiesLeftToSpawn = new Dictionary<GameObject, int>();
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(OnEnemyDestroy);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (isSpawning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / waves[currentWave].enemiesSpawnedPerSecond) && enemiesLeftToSpawn.Count > 0)
            {
                SpawnEnemy();
                enemiesAlive++;
                timeSinceLastSpawn = 0;
            }

            if (enemiesAlive == 0 && enemiesLeftToSpawn.Count == 0)
            {
                EndWave();
            }
        }
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        timeSinceLastSpawn = 0;
        SetEnemiesPerWave();
    }

    void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        if(currentWave < waves.Length)
        {
            StartCoroutine(StartWave());
        }
    }

    private void SpawnEnemy()
    {
        List<GameObject> remainingEnemyTypes = new List<GameObject>(enemiesLeftToSpawn.Keys);

        int randomEnemyIndex = Random.Range(0, remainingEnemyTypes.Count);

        GameObject prefabToSpawn = remainingEnemyTypes[randomEnemyIndex];

        if (enemiesLeftToSpawn[prefabToSpawn] - 1 == 0)
        {
            enemiesLeftToSpawn.Remove(prefabToSpawn);
        }
        else
        {
            enemiesLeftToSpawn[prefabToSpawn] -= 1;
        }
        
        Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity);

        Debug.Log($"Spawned {prefabToSpawn.name}. {(enemiesLeftToSpawn.ContainsKey(prefabToSpawn) ? enemiesLeftToSpawn[prefabToSpawn] : 0)} remaining");
    }

    private void SetEnemiesPerWave()
    {
        foreach (EnemyNumberPair pair in waves[currentWave].enemyAmounts)
        {
            enemiesLeftToSpawn[pair.enemy] = pair.number;
        }
    }

    private void OnEnemyDestroy()
    {
        enemiesAlive--;
    }
}
