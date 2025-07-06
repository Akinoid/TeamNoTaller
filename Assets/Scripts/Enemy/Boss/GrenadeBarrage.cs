using System.Collections;
using UnityEngine;

public class GrenadeBarrage : MonoBehaviour, IBossAttack
{
    public GameObject warningAreaPrefab;
    public GameObject explosionPrefab;

    public float warningDuration = 2f;
    public float explosionDelay = 0.2f;
    public int attackRepeats = 3;
    public float timeBetweenAttacks = 2f;

    public float areaRadius = 3f; 
    public IEnumerator Execute(Boss boss)
    {
        Debug.Log("Grenade Barrage iniciado");

        Animator animator = boss.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool("Explosion", true);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("GrenadeBarrage: Player not found");
            yield break;
        }

        for (int i = 0; i < attackRepeats; i++)
        {
           
            Vector2 offset = Random.insideUnitCircle * areaRadius;
            Vector3 areaPos = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, 0f);

            
            GameObject warning = Instantiate(warningAreaPrefab, areaPos, warningAreaPrefab.transform.rotation);
            WarningArea area = warning.GetComponent<WarningArea>();
            if (area != null)
            {
                yield return area.StartWarning(warningDuration, explosionDelay);
            }
            else
            {
                Debug.LogWarning("GrenadeBarrage: WarningArea missing script");
                yield return new WaitForSeconds(warningDuration + explosionDelay);
            }

            
            Instantiate(explosionPrefab, areaPos, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        Debug.Log("Grenade Barrage terminado");
        animator.SetBool("Explosion", false);
    }
}
