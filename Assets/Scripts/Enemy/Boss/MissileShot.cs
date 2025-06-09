using System.Collections;
using UnityEngine;

public class MissileShot : MonoBehaviour,IBossAttack
{
    public IEnumerator Execute(Boss boss)
    {
        Debug.Log("Ejecutando disparo de misiles");
        yield return new WaitForSeconds(4f);
    }
}
