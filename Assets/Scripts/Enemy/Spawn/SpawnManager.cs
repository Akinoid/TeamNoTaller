using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private System.Collections.IEnumerator RunPatterns()
    {
        foreach (var sequence in patternSequences)
        {
            
            for (int i = 0; i < sequence.repetitions; i++)
            {
                
                
                foreach (var zone in sequence.pattern.spawnZones)
                {
                    
                    if (zone != null)
                    {
                        foreach (var e in sequence.pattern.spawnSequence)
                        {
                           

                            GameObject enemy = e.enemyPrefab;
                            if (enemy != null)
                            {
                                GameObject spawned = zone.SpawnEnemy(enemy);
                                if (spawned != null)
                                {
                                    spawnedEnemies.Add(spawned);
                                }
                            }
                            else
                            {
                                Debug.Log("enemy = null");
                            }
                            
                        }                       
                        
                    }
                    else
                    {
                        Debug.Log("zone = null");
                    }
                }

                yield return new WaitForSeconds(sequence.delayBetweenRepetitions);
            }
        }
        waitingForEnemiesToDie = true;
    }
}