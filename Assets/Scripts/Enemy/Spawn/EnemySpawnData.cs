using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    [Range(0f, 1f)]
    public float spawnChance = 1f;
}

