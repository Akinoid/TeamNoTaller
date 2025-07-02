using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;
    public int zoneIndex;
    public float delayAfterPrevious = 1f;

}


