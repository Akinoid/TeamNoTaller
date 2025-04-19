using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnZone[] spawnZones;

    void Start()
    {
        foreach (var zone in spawnZones)
        {
            zone.StartPatternSequence();
        }
    }
}
