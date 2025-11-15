using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveInfo[] waves;

    [Header("References")]
    [SerializeField] TextMeshProUGUI waveIndicator;

    [Header("Attributes")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int moneyBetweenWaves = 10;

    [Header("Events")]
    public static UnityEvent<GameObject> onEnemyDestroy = new UnityEvent<GameObject>();

    private int currentWave = 0;
    private float timeSinceLastSpawn;
    private int enemiesTotalAlive;
    private Dictionary<GameObject, int> enemiesAlive = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> enemiesLeftToSpawn = new Dictionary<GameObject, int>();
    private Dictionary<String, GameObject> nameToPrefab = new Dictionary<String, GameObject>();
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(enemy =>
        {
            // Can add effects here for when the player is damaged, like screen shake

            OnEnemyDestroy(enemy);
        });
    }

    private void Start()
    {
        StartCoroutine(StartWave());
        enemiesTotalAlive = 0;
    }

    void Update()
    {
        if (isSpawning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / waves[currentWave].enemiesSpawnedPerSecond) && enemiesLeftToSpawn.Count > 0)
            {
                SpawnEnemy();
                timeSinceLastSpawn = 0;
            }
            else if (enemiesTotalAlive <= 0 && enemiesLeftToSpawn.Count == 0)
            {
                EndWave();
            }
        }

        waveIndicator.text = $"Wave: {currentWave + 1}, Enemies Left: \n {printEnemiesRemaining()}";
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        timeSinceLastSpawn = 0;
        SetEnemiesPerWave();
        //Debug.Log($"Wave {currentWave + 1} started");
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
        GameManager.onGainMoney.Invoke(moneyBetweenWaves);

        //Debug.Log($"Wave {currentWave} ended");
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
            enemiesLeftToSpawn[prefabToSpawn]--;
        }
        
        Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity);

        enemiesAlive[prefabToSpawn]++;
        enemiesTotalAlive++;

        //Debug.Log($"Spawned {prefabToSpawn.name}. {(enemiesLeftToSpawn.ContainsKey(prefabToSpawn) ? enemiesLeftToSpawn[prefabToSpawn] : 0)} remaining");
    }

    private void SetEnemiesPerWave()
    {
        foreach (EnemyNumberPair pair in waves[currentWave].enemyAmounts)
        {
            enemiesLeftToSpawn[pair.enemy] = pair.number;
            enemiesAlive[pair.enemy] = 0;
            nameToPrefab[pair.enemy.name] = pair.enemy;
        }
    }

    private void OnEnemyDestroy(GameObject enemy)
    {
        enemiesAlive[nameToPrefab[enemy.name.Replace("(Clone)", "")]]--;
        enemiesTotalAlive--;
    }

    private String printEnemiesRemaining()
    {
        String s = "";
        foreach(KeyValuePair<GameObject, int> entry in enemiesAlive)
        {
            s += entry.Key.name + $": {entry.Value}\n";
        }
        return s;
    }
}
