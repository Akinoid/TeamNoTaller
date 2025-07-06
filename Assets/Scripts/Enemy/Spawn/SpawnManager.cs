using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Oleadas Normales")]
    public SpawnPatternSequence[] normalSequences;
    public int normalWaveCount = 3;

    [Header("Oleadas Avanzadas")]
    public SpawnPatternSequence[] advancedSequences;
    public int advancedWaveCount = 2;

    [Header("Fase de Boss")]
    public GameObject bossPrefab;
    public SpawnZone bossSpawnZone;

    [Header("Siguiente Escena")]
    public string nextSceneName;

    public GameObject bosslife;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Money money;

    private void Start()
    {
        bosslife.SetActive(false);
        money = GameObject.Find("MoneyManager").GetComponent<Money>();
        StartCoroutine(RunPatterns());
    }

    private IEnumerator RunPatterns()
    {
        
        for (int i = 0; i < normalWaveCount; i++)
        {
            var seq = normalSequences[Random.Range(0, normalSequences.Length)];
            yield return StartCoroutine(RunSinglePattern(seq));
            
        }

       
        for (int i = 0; i < advancedWaveCount; i++)
        {
            var seq = advancedSequences[Random.Range(0, advancedSequences.Length)];
            yield return StartCoroutine(RunSinglePattern(seq));
        }

        
        yield return StartCoroutine(WaitForClear());

        bosslife.SetActive(true);
        if (bossPrefab != null && bossSpawnZone != null)
        {
            Debug.Log("SpawnManager: Spawn del Boss");
            var bossGO = bossSpawnZone.SpawnEnemy(bossPrefab);
            spawnedEnemies.Add(bossGO);
        }

        
        yield return StartCoroutine(WaitForClear());

        money.money += Money.score;
        Money.score = 0;
        if (StartMenuManager.tutorial == 1)
            SceneManager.LoadScene(nextSceneName);
    }


    private IEnumerator RunSinglePattern(SpawnPatternSequence sequence)
    {
        for (int rep = 0; rep < sequence.repetitions; rep++)
        {
            foreach (var entry in sequence.pattern.spawnSequence)
            {
                SpawnEntry(entry, sequence.pattern.spawnZones);
                yield return new WaitForSeconds(entry.delayAfterPrevious);
            }
            yield return new WaitForSeconds(sequence.delayBetweenRepetitions);
        }
    }

   
    private void SpawnEntry(EnemySpawnEntry entry, SpawnZone[] zones)
    {
        int zi = entry.zoneIndex;
        if (zi >= 0 && zi < zones.Length && entry.enemyPrefab != null)
        {
            var go = zones[zi].SpawnEnemy(entry.enemyPrefab);
            spawnedEnemies.Add(go);
        }
        else Debug.LogWarning($"SpawnManager: zoneIndex {zi} fuera de rango o prefab null");
    }

    
    private IEnumerator WaitForClear()
    {
        // Limpia la lista de posibles nulls
        spawnedEnemies.RemoveAll(e => e == null);

        // Espera mientras quede alguno
        while (spawnedEnemies.Count > 0)
        {
            yield return null;
            spawnedEnemies.RemoveAll(e => e == null);
        }
    }
}