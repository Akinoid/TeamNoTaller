using UnityEngine;

[System.Serializable]
public class SpawnPatternSequence
{
    public string label;
    public SpawnPattern pattern;
    public int repetitions = 1;
    public float delayBetweenRepetitions = 1f;
}
