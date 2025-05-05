using UnityEngine;
[System.Serializable]

    [CreateAssetMenu(fileName = "SpawnPattern", menuName = "Spawning/Spawn Pattern")]
public class SpawnPattern : MonoBehaviour
{ 
    public SpawnZone[] spawnZones;
    public EnemySpawnEntry[] spawnSequence;    
}
