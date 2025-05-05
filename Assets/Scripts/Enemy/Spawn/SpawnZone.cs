using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    public EnemySpawnData[] enemiesToSpawn;

    public Vector3 GetRandomPointInZone()
    {
        var box = GetComponent<BoxCollider>();
        Vector3 center = transform.position + box.center;
        Vector3 size = box.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(x, y, z);
    }

    public GameObject GetRandomEnemy()
    {
        float total = 0f;
        foreach (var e in enemiesToSpawn) total += e.spawnChance;

        float rand = Random.Range(0f, total);
        float sum = 0f;

        foreach (var e in enemiesToSpawn)
        {
            sum += e.spawnChance;
            if (rand <= sum)
                return e.enemyPrefab;
        }

        return null;
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPoint = GetRandomPointInZone();
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        Debug.Log($"Spawned {enemyPrefab.name} at {spawnPoint}");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var box = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(transform.position + box.center, box.size);
    }
}