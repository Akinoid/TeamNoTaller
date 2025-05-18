using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPatternSequence[] patternSequences;

    private void Start()
    {
        StartCoroutine(RunPatterns());
    }

    private System.Collections.IEnumerator RunPatterns()
    {
        foreach (var sequence in patternSequences)
        {
            Debug.Log("check 1");
            for (int i = 0; i < sequence.repetitions; i++)
            {
                Debug.Log("Check2");
                
                foreach (var zone in sequence.pattern.spawnZones)
                {
                    Debug.Log("check3");
                    if (zone != null)
                    {
                        foreach (var e in sequence.pattern.spawnSequence)
                        {
                            Debug.Log("check4");

                            GameObject enemy = e.enemyPrefab;
                            if (enemy != null)
                            {
                                Debug.Log("check5");
                                zone.SpawnEnemy(enemy);
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
    }
}