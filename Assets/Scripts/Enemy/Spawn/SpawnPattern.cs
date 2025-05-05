using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "SpawnPattern", menuName = "Spawning/Spawn Pattern")]
public class SpawnPattern : ScriptableObject
{
    public EnemySpawnEntry[] spawnSequence;
}
