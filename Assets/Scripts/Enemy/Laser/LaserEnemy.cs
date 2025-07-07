using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEnemy : EnemyBase
{
    [Header("Laser Settings")]
    public LineRenderer laserRenderer;
    public GameObject laserEffect;
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

    [SerializeField] private Animator animator;

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
            laserEffect.SetActive(false);
        }
    }

    protected override void OnEnterComplete()
    {
        AudioManager.Instance.Play("Laser Start");
        Debug.Log("pattern");
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
            // Antes de encender láser, asignamos un patrón nuevo distinto al anterior:
            currentPattern = ChooseRandomPatternExcept(currentPattern);
            Debug.Log("LaserEnemy: Nuevo patrón  " + currentPattern);

            animator.SetBool("Golpe", true);
            ActivateLaser(true);
            AudioManager.Instance.Play("Laser Attack");
            yield return new WaitForSeconds(laserOnDuration);

            animator.SetBool("Golpe", false);
            ActivateLaser(false);

            
            yield return new WaitForSeconds(laserOffDuration);
            currentPattern = ChooseRandomPatternExcept(currentPattern);
            Debug.Log("LaserEnemy: Patrón cambiado tras ciclo completo  " + currentPattern);
        }
        AudioManager.Instance.Stop("Laser Attack");
        ActivateLaser(false);
    }

    private void ActivateLaser(bool active)
    {
        laserActive = active;
        if (laserRenderer != null)
        {
            laserRenderer.enabled = active;
            laserEffect.SetActive(active);
        }
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
        Bubble bubble = player.GetComponent<Bubble>();

        if (shield != null && shield.haveShield)
        {
            shield.GetDamage(50, true);
        }
        else if (life != null && life.canGetHit && !shield.haveShield)
        {
            life.getHit = true;
            Debug.Log("Hit = True");
        }
        else if (life != null && life.canGetHit && life.haveBubble)
        {
            bubble.getHitBubble = true;
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
                // Oscilación suave en X alrededor de initialPosition.x
                offset = Vector3.right * Mathf.Sin(moveTimer * movementSpeed) * movementRadius;
                break;

            case MovementPattern.Vertical:
                offset = Vector3.up * Mathf.Sin(moveTimer * movementSpeed) * movementRadius;
                break;

            case MovementPattern.Circular:
                offset = new Vector3(
                    Mathf.Cos(moveTimer * movementSpeed),
                    Mathf.Sin(moveTimer * movementSpeed),
                    0f
                ) * movementRadius;
                break;

            case MovementPattern.Zigzag:
                // Ejemplo con seno suave en X y Y:
                float x = Mathf.Sin(moveTimer * movementSpeed) * (movementRadius * 0.5f);
                float y = Mathf.Sin(moveTimer * movementSpeed * 2f) * (movementRadius * 0.5f);
                offset = new Vector3(x, y, 0f);
                break;
        }

                transform.position = initialPosition + offset;
    }

    private MovementPattern ChooseRandomPatternExcept(MovementPattern except)
    {
        // Obtiene todos los valores del enum
        var values = System.Enum.GetValues(typeof(MovementPattern));
        // Convierte a lista de MovementPattern
        List<MovementPattern> list = new List<MovementPattern>();
        foreach (MovementPattern mp in values)
        {
            if (mp != except)
                list.Add(mp);
        }
        if (list.Count == 0)
            return except; // si hubiera solo un elemento (raro), retorna el mismo
                           // Selecciona uno al azar
        int idx = Random.Range(0, list.Count);
        return list[idx];
    }
    protected override void Die()
    {
        AudioManager.Instance.Play("Enemy Die");
        Money.score += 150 * Money.multiplier;
        if (StartMenuManager.tutorial == 0)
        {
            ActualDialogueTutorial.startChangeLines = true;
        }
        base.Die();
    }
}