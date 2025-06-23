using UnityEngine;
using System.Collections;

public class LaserEnemy : EnemyBase
{
    [Header("Laser Settings")]
    public LineRenderer laserRenderer;
    public Transform gunPoint;          // empty donde sale el láser
    public float laserOnDuration = 10f;
    public float laserOffDuration = 10f;
    public float movementSpeed = 2f;
    public float movementRadius = 3f;
    float moveTimer;

    private Transform playerTransform;
    private bool laserActive;
    private Coroutine laserRoutine;

    private enum MovementPattern { Horizontal, Vertical, Circular, Zigzag }
    private MovementPattern currentPattern;
    private Vector3 initialPosition;

    private GameObject playerGO;

    protected override void Start()
    {
        base.Start();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null) playerTransform = playerGO.transform;
        else Debug.LogError("LaserEnemy: No se encontró jugador");

        if (laserRenderer != null)
        {
            laserRenderer.enabled = false;
            laserRenderer.useWorldSpace = true;
        }
    }

    protected override void OnEnterComplete()
    {
        initialPosition = transform.position;
        currentPattern = (MovementPattern)Random.Range(0, System.Enum.GetValues(typeof(MovementPattern)).Length);
        Debug.Log("LaserEnemy: Patrón seleccionado -> " + currentPattern);
        if (laserRoutine == null)
            laserRoutine = StartCoroutine(LaserCycle());
    }

    private IEnumerator LaserCycle()
    {
        while (currentState == State.Active)
        {
            ActivateLaser(true);
            yield return new WaitForSeconds(laserOnDuration);

            ActivateLaser(false);
            yield return new WaitForSeconds(laserOffDuration);
        }
        ActivateLaser(false);
    }

    private void ActivateLaser(bool active)
    {
        laserActive = active;
        if (laserRenderer != null)
            laserRenderer.enabled = active;
    }

    protected override void Update()
    {
        base.Update();

        // actualizar playerTransform si es nulo
        if (playerTransform == null && GameObject.FindGameObjectWithTag("Player") != null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (currentState == State.Active)
        {
            MoveByPattern();
        }

        if (laserActive && laserRenderer != null)
        {
            Vector3 start = gunPoint != null ? gunPoint.position : transform.position;
            Vector3 end = new Vector3(start.x, start.y, -1f); // Extiende hasta Z=-1
            laserRenderer.SetPosition(0, start);
            laserRenderer.SetPosition(1, end);

            // Raycast para daño (opcional, si manejas daño aquí)
            Vector3 dir = (end - start).normalized;
            float dist = Vector3.Distance(start, end);
            if (Physics.Raycast(start, dir, out var hit, dist))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    DamagePlayer(hit.collider.gameObject);
                }
            }
        }
    }

    protected override void DamagePlayer(GameObject player)
    {
        if (player == null) return;

        PlayerLife life = player.GetComponent<PlayerLife>();
        Shield shield = player.GetComponent<Shield>();

        if (shield != null && shield.haveShield)
        {
            shield.GetDamage(50, true);
        }
        else if (life != null && life.canGetHit)
        {
            life.getHit = true;
            Debug.Log("Hit = True");
        }
        else
        {
            Debug.Log("SniperLaser: No se encontró PlayerLife o canGetHit es false");
        }
    }
    private void MoveByPattern()
    {
        moveTimer += Time.deltaTime;

        Vector3 offset = Vector3.zero;

        switch (currentPattern)
        {
            case MovementPattern.Horizontal:
                offset = Vector3.right * Mathf.Sin(moveTimer * movementSpeed) * movementRadius;
                break;

            case MovementPattern.Vertical:
                offset = Vector3.up * Mathf.Sin(moveTimer * movementSpeed) * movementRadius;
                break;

            case MovementPattern.Circular:
                offset = new Vector3(Mathf.Cos(moveTimer * movementSpeed), Mathf.Sin(moveTimer * movementSpeed), 0f) * movementRadius;
                break;

            case MovementPattern.Zigzag:
                float zig = Mathf.Sin(moveTimer * movementSpeed) > 0 ? 1f : -1f;
                offset = new Vector3(zig, Mathf.Sin(moveTimer * movementSpeed * 2f), 0f) * movementRadius * 0.5f;
                break;
        }

        transform.position = initialPosition + offset;
    }
    protected override void Die()
    {
        Money.score += 150 * Money.multiplier;
        if (StartMenuManager.tutorial == 0)
        {
            ActualDialogueTutorial.startChangeLines = true;
        }
        base.Die();
    }
}