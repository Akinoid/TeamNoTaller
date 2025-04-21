using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;
    public float delayAfterPrevious = 1f;

}

[CreateAssetMenu(fileName = "SpawnPattern", menuName = "Spawning/Spawn Pattern")]
public class SpawnPattern : ScriptableObject
{
    public EnemySpawnEntry[] spawnSequence;
}