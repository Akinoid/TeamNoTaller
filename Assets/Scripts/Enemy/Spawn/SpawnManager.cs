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