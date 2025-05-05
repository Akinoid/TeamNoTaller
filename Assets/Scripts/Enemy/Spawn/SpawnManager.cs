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

                        GameObject enemy = zone.GetRandomEnemy();
                        if (enemy != null)
                        {
                            zone.SpawnEnemy(enemy);
                        }
                    }
                }

                yield return new WaitForSeconds(sequence.delayBetweenRepetitions);
            }
        }
    }
}