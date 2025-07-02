using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public SpawnPatternSequence[] patternSequences;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool waitingForEnemiesToDie = false;
    public string nextSceneName;
    Money money;
    private void Start()
    {
         money = GameObject.Find("MoneyManager").GetComponent<Money>();
        StartCoroutine(RunPatterns());
    }
    private void Update()
    {
        if (waitingForEnemiesToDie)
        {
            
            spawnedEnemies.RemoveAll(e => e == null);

            if (spawnedEnemies.Count == 0)
            {
                waitingForEnemiesToDie = false;
                
                money.money += Money.score;
                Money.score = 0;
                if(StartMenuManager.tutorial == 1)
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                
            }
        }
    }

    private IEnumerator RunPatterns()
    {
        foreach (var sequence in patternSequences)
        {
            for (int rep = 0; rep < sequence.repetitions; rep++)
            {
                foreach (var entry in sequence.pattern.spawnSequence)
                {
                    int zi = entry.zoneIndex;
                    // Validar índice
                    if (zi >= 0 && zi < sequence.pattern.spawnZones.Length)
                    {
                        var zone = sequence.pattern.spawnZones[zi];
                        if (zone != null && entry.enemyPrefab != null)
                        {
                            GameObject spawned = zone.SpawnEnemy(entry.enemyPrefab);
                            spawnedEnemies.Add(spawned);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"SpawnManager: zoneIndex {zi} fuera de rango para patrón {sequence.pattern.name}");
                    }
                    yield return new WaitForSeconds(entry.delayAfterPrevious);
                }
                yield return new WaitForSeconds(sequence.delayBetweenRepetitions);
            }
        }

        // Terminamos de spawnear todas las olas
        waitingForEnemiesToDie = true;
    }
}