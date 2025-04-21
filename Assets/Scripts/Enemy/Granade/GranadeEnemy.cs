using UnityEngine;
using System.Collections;
public class GranadeEnemy : EnemyBase
{
    [Header("Grenade Settings")]
    public GameObject warningAreaPrefab;
    public GameObject explosionPrefab;
    public float warningDuration = 2f;
    public float explosionDelay = 0.2f;
    public int attackRepeats = 5;
    public float timeBetweenAttacks = 2f;

    private Transform playerTransform;

    protected override void OnEnterComplete()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTransform = playerGO.transform;
            StartCoroutine(AttackRoutine());
        }
        else
        {
            Debug.LogError("GrenadeEnemy: Player not found!");
        }
    }

    private IEnumerator AttackRoutine()
    {
        for (int i = 0; i < attackRepeats; i++)
        {
            Vector3 areaPos = new Vector3(playerTransform.position.x, playerTransform.position.y, 0);

            GameObject warning = Instantiate(warningAreaPrefab, areaPos, Quaternion.identity);
            WarningArea area = warning.GetComponent<WarningArea>();
            if (area != null)
            {
                yield return area.StartWarning(warningDuration, explosionDelay);
            }

            Instantiate(explosionPrefab, areaPos, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

}
