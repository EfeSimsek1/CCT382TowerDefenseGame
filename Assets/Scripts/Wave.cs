using System.Collections.Generic;
using UnityEngine;

public class Wave
{
}

[System.Serializable]
public struct WaveInfo
{
    public EnemyNumberPair[] enemyAmounts;
    public float enemiesSpawnedPerSecond;

    public WaveInfo(EnemyNumberPair[] pairs, float _enemiesPerSecond)
    {
        enemyAmounts = pairs;
        enemiesSpawnedPerSecond = _enemiesPerSecond;
    }
}

[System.Serializable]
public struct EnemyNumberPair
{
    public GameObject enemy;
    public int number;

    public EnemyNumberPair(GameObject enemy, int number)
    {
        this.enemy = enemy;
        this.number = number;
    }
}
