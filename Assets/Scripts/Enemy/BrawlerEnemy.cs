using UnityEngine;

public class BrawlerEnemy : EnemyBase
{
    public float attackDelay = 1f;
    public float attackDamage = 20f;
    public GameObject dangerZonePrefab;

    private bool isCharging = false;
    private GameObject dangerZoneInstance;

    protected override void OnEnterComplete()
    {
        InvokeRepeating("PerformAttack", 2f, 5f); // Ataca cada 5 segundos
    }

    void PerformAttack()
    {
        if (currentState != State.Attacking || isCharging) return;

        isCharging = true;

       
        Vector3 dangerPosition = transform.position + transform.forward * 3f;
        dangerZoneInstance = Instantiate(dangerZonePrefab, dangerPosition, Quaternion.identity);

        DangerZone dangerZone = dangerZoneInstance.GetComponent<DangerZone>();
        dangerZone.StartCharging(attackDelay, () =>
        {
           
            Collider[] hits = Physics.OverlapBox(dangerZoneInstance.transform.position, dangerZoneInstance.transform.localScale / 2);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {

                    Debug.Log("damage");
                }
            }

            Destroy(dangerZoneInstance);
            isCharging = false;
        });
    }

}
