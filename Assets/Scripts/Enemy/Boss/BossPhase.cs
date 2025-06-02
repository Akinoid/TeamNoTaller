using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPhase
{
    public string name;
    public int minHealth;
    public List<MonoBehaviour> attackBehaviours; 

    public List<IBossAttack> GetAttacks()
    {
        List<IBossAttack> attacks = new();
        foreach (var mono in attackBehaviours)
        {
            if (mono is IBossAttack attack)
                attacks.Add(attack);
        }
        return attacks;
    }
}
