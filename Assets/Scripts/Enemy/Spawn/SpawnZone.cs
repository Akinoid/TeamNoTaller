using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    public SpawnPatternSequence[] patternSequences;

    private int currentPatternIndex = 0;
    private int currentRepetition = 0;

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

    public void StartPatternSequence()
    {
        if (patternSequences.Length > 0)
            StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        while (currentPatternIndex < patternSequences.Length)
        {
            var current = patternSequences[currentPatternIndex];
            for (int i = 0; i < current.repetitions; i++)
            {
                yield return StartCoroutine(RunSpawnPattern(current.pattern));
                yield return new WaitForSeconds(current.delayBetweenRepetitions);
            }

            currentPatternIndex++;
        }

        Debug.Log($"{gameObject.name}: Finished all pattern sequences.");
    }

    private IEnumerator RunSpawnPattern(SpawnPattern pattern)
    {
        foreach (var entry in pattern.spawnSequence)
        {
            if (entry.enemyPrefab != null)
            {
                Vector3 spawnPoint = GetRandomPointInZone();
                Instantiate(entry.enemyPrefab, spawnPoint, Quaternion.identity);
                Debug.Log($"Pattern spawn: {entry.enemyPrefab.name} at {spawnPoint}");
            }

            yield return new WaitForSeconds(entry.delayAfterPrevious);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var box = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(transform.position + box.center, box.size);
    }
}