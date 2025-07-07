using UnityEngine;
using System.Collections;

public class BrawlerEnemy : EnemyBase
{
    [Header("Brawler Settings")]
    public float followSpeed = 2f;
    public float attackRange = 1f;
    public float attackDelay = 1f;
    public GameObject dangerZonePrefab;
    private GameObject playerGO;

    public float attackRadius = 3f;

    [SerializeField]private Transform playerTransform;
    private bool isCharging;

    [SerializeField] private Transform ataque;

    [SerializeField] private Animator animator;

    protected override void Start()
    {
        base.Start();
        health = 40f;
        base.currentHealth = health;
    }


    protected override void OnEnterComplete()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
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
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.transform;
        if (currentState == State.Active && playerTransform != null)
        {
            FollowAndMaybeAttack();
        }
    }

    private void FollowAndMaybeAttack()
    {
        // Seguir al jugador en X/Y, manteniendo Z fijo
        Vector3 target = new Vector3(playerTransform.position.x, playerTransform.position.y, ataque.position.z);
        ataque.position = Vector3.MoveTowards(ataque.position, target, followSpeed * Time.deltaTime);

        float dist = Vector2.Distance(
            new Vector2(ataque.position.x, ataque.position.y),
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

        animator.SetBool("Golpe", true);

        // Lista para controlar los dos ataques
        int numberOfAttacks = 2;
        int completedAttacks = 0;

        for (int i = 0; i < numberOfAttacks; i++)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(1f, attackRadius);

            Vector3 dzPos = new Vector3(
                playerTransform.position.x + offset.x,
                playerTransform.position.y + offset.y,
                0f
            );

            GameObject dzGO = Instantiate(dangerZonePrefab, dzPos, dangerZonePrefab.transform.rotation);
            DangerZone dz = dzGO.GetComponent<DangerZone>();
            RegisterAssociatedObject(dzGO);
            if (dz == null)
            {
                Debug.LogError("DangerZone prefab missing DangerZone script!");
                continue;
            }

            // Lanza cada zona y espera su callback
            dz.StartCharging(attackDelay, () =>
            {
                Debug.Log("BrawlerEnemy: Attack triggered!");

                BoxCollider box = dzGO.GetComponent<BoxCollider>();
                if (box != null)
                {
                    Vector3 center = box.transform.TransformPoint(box.center);
                    Vector3 halfExtents = Vector3.Scale(box.size, box.transform.lossyScale) / 2f;

                    Collider[] hits = Physics.OverlapBox(center, halfExtents, box.transform.rotation);
                    foreach (var h in hits)
                    {
                        if (h.CompareTag("Player"))
                        {
                            Debug.Log("BrawlerEnemy: Player HIT by DangerZone!");
                            DamagePlayer(h.gameObject);
                        }
                    }
                }

                Destroy(dzGO);
                completedAttacks++;
            });
        }

        // Espera hasta que ambas zonas terminen
        while (completedAttacks < numberOfAttacks)
            yield return null;

        isCharging = false;
        animator.SetBool("Golpe", false);
    }

    protected override void DamagePlayer(GameObject player)
    {
        PlayerLife life = player.GetComponent<PlayerLife>();

        Shield shield = player.GetComponent<Shield>();

        Bubble bubble = player.GetComponent<Bubble>();

        if (shield.haveShield)
        {
            shield.GetDamage(20, true);
            Debug.Log("Escudo Funciona Lets go");
        }
        else if (life != null && life.canGetHit && !shield.haveShield)
        {
            life.getHit = true;
            Debug.Log("Player got Hit");
        }
        else if (life != null && life.canGetHit && life.haveBubble)
        {
            bubble.getHitBubble = true;
        }
    }
    protected override void Die()
    {
        AudioManager.Instance.Play("Enemy Die");
        Money.score += 100 * Money.multiplier;
        if (StartMenuManager.tutorial == 0)
        {
            ActualDialogueTutorial.startChangeLines = true;
        }
        base.Die();
    }
}