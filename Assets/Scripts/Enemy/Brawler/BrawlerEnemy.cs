using UnityEngine;
using System.Collections;

public class BrawlerEnemy : EnemyBase
{
    [Header("Brawler Settings")]
    public float followSpeed = 2f;
    public float attackRange = 1f;
    public float attackDelay = 1f;
    public GameObject dangerZonePrefab;

    private Transform playerTransform;
    private bool isCharging;

    protected override void OnEnterComplete()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTransform = playerGO.transform;
            Debug.Log("BrawlerEnemy: Player found");
        }
        else
        {
            Debug.LogError("BrawlerEnemy: Player tag missing!");
        }
    }

    protected override void Update()
    {
        base.Update(); // maneja los estados Entering/Active/Exiting
        if (currentState == State.Active && playerTransform != null)
        {
            FollowAndMaybeAttack();
        }
    }

    private void FollowAndMaybeAttack()
    {
        // Seguir al jugador en X/Y, manteniendo Z fijo
        Vector3 target = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, followSpeed * Time.deltaTime);

        float dist = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(playerTransform.position.x, playerTransform.position.y)
        );

        if (dist <= attackRange && !isCharging)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isCharging = true;
        Debug.Log("BrawlerEnemy: Start charging attack");

        
        Vector3 dzPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            0f
        );
        Debug.DrawLine(transform.position, dzPos, Color.red, 1f);

        GameObject dzGO = Instantiate(dangerZonePrefab, dzPos, Quaternion.identity);
        DangerZone dz = dzGO.GetComponent<DangerZone>();
        if (dz == null)
        {
            Debug.LogError("DangerZone prefab missing DangerZone script!");
            isCharging = false;
            yield break;
        }

        bool done = false;
        dz.StartCharging(attackDelay, () =>
        {
            Debug.Log("BrawlerEnemy: Attack triggered!");
            Collider[] hits = Physics.OverlapBox(dzGO.transform.position, dzGO.transform.localScale / 2);
            foreach (var h in hits)
                if (h.CompareTag("Player"))
                {
                    Debug.Log("BrawlerEnemy: Player HIT by DangerZone!");

                    DamageUtils.DamagePlayer(h.gameObject);
                }
                    

            Destroy(dzGO);
            done = true;
        });

        while (!done) yield return null;
        isCharging = false;
    }
}