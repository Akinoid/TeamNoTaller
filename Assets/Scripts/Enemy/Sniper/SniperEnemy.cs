using UnityEngine;
using System.Collections;

public class SniperEnemy : EnemyBase
{
    [Header("Sniper Settings")]
    public GameObject markerPrefab;
    public GameObject beamPrefab;
    public float markerOffsetZ = 0f;
    public float damage = 40f;
    public float beamDuration = 0.5f;
    public float beamRange = 100f;
    public float totalAimTime = 3f;
    public float markerFollowTime = 2f;

    private Transform playerTransform;
    private GameObject markerInstance;
    private Vector3 lastPlayerPosition;
    private bool hasStartedAiming = false;

    private GameObject pgo;
    private Vector3 spawnPoint;
    private bool isExiting = false;
    private bool hasStartedEntryShot = false;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform gunPoint;

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        health = 40f;
        currentHealth = health;

        pgo = GameObject.FindGameObjectWithTag("Player");
        if (pgo != null) playerTransform = pgo.transform;
        else Debug.LogError("SniperEnemy: No se encontró objeto con tag 'Player'");

        
    }

    protected override void Update()
    {
        base.Update();

        if (playerTransform == null)
        {
            pgo = GameObject.FindGameObjectWithTag("Player");
            if (pgo != null) playerTransform = pgo.transform;
        }
    }

    protected override void HandleEntering()
    {
        base.HandleEntering();

        // Iniciar disparo durante entrada
        if (!hasStartedEntryShot && currentState == State.Entering)
        {
            hasStartedEntryShot = true;
            StartCoroutine(AimAndFireRoutine());
        }
    }

    private IEnumerator AimAndFireRoutine(System.Action onComplete = null)
    {
        animator.SetBool("Golpe", true);

        if(markerInstance != null)
            Destroy(markerInstance); 

        markerInstance = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);

        RegisterAssociatedObject(markerInstance);
        float timer = 0f;

        while (timer < markerFollowTime)
        {
            if (playerTransform != null)
            {
                lastPlayerPosition = new Vector3(
                    playerTransform.position.x,
                    playerTransform.position.y,
                    markerOffsetZ
                );
                if (markerInstance != null)
                    markerInstance.transform.position = lastPlayerPosition;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        float holdTime = totalAimTime - markerFollowTime;
        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);

        // Destruir marcador antes de disparar
        if (markerInstance != null)
            Destroy(markerInstance);

        // Iniciar láser animado y daño diferido
        Vector3 targetPos = lastPlayerPosition;
        StartCoroutine(FireBeamRoutine(targetPos));

        animator.SetBool("Golpe", false);
        onComplete?.Invoke();
    }

    protected override void OnEnterComplete()
    {
        StartCoroutine(WaitThenExit());
    }

    private IEnumerator WaitThenExit()
    {
        currentState = State.Active;
        yield return new WaitForSeconds(6f);
        OnExitStart();
    }
    protected override void OnExitStart()
    {
        base.OnExitStart(); // Mantén el reinicio después de 3 segundos
        if (!isExiting)
            StartCoroutine(ExitRoutine());
    }

    private IEnumerator ExitRoutine()
    {
        isExiting = true;
        Vector3 retreatTarget = spawnPoint - new Vector3(0, 0, 10f);
        float speed = 10f;
        bool hasShot = false;

        while (Vector3.Distance(transform.position, retreatTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, retreatTarget, speed * Time.deltaTime);

            if (!hasShot && Vector3.Distance(transform.position, retreatTarget) > 5f)
            {
                // Disparo durante la salida
                StartCoroutine(AimAndFireRoutine());
                hasShot = true;
            }

            yield return null;
        }

        // Esperar atrás antes de volver a entrar
        yield return new WaitForSeconds(4f);

        isExiting = false;
        hasStartedAiming = false;
        hasStartedEntryShot = false;
        currentState = State.Entering;
    }


    private IEnumerator FireBeamRoutine(Vector3 targetPos)
    {
        // Punto de partida: gunPoint o la posición del sniper
        Vector3 start = (gunPoint != null) ? gunPoint.position : transform.position;
        Vector3 dir = (targetPos - start).normalized;
        float distance = Vector3.Distance(start, targetPos);

        // Instanciar beam prefab en escena
        GameObject beamGO = null;
        Beam beamScript = null;
        if (beamPrefab != null)
        {
            beamGO = Instantiate(beamPrefab, Vector3.zero, Quaternion.identity);
            beamScript = beamGO.GetComponent<Beam>();
            if (beamScript != null)
            {
                // Pasa puntos al beam para animar
                beamScript.Initialize(start, targetPos);
            }
            else
            {
                Debug.LogWarning("SniperEnemy: Beam prefab no tiene componente Beam.");
                Destroy(beamGO);
            }
        }

        // Esperar al láser viaje: si Beam.speed está definido, usa eso; si no, asume instantáneo
        float travelTime = 0f;
        if (beamScript != null && beamScript.speed > 0f)
        {
            travelTime = distance / beamScript.speed;
        }
        // Opcional: agregar un mínimo de delay para efectos visuales
        if (travelTime > 0f)
            yield return new WaitForSeconds(travelTime);
        else
            yield return null;

        // Después de que el láser “llega”, aplicamos daño con un raycast corto o chequeo de proximidad
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit, distance + 0.1f))
        {
            // Asegurarse de apuntar al root que tenga PlayerLife
            var pl = hit.collider.GetComponentInParent<PlayerLife>();
            if (pl != null)
            {
                GameObject playerGO = pl.gameObject;
                // Aplicar daño
                Shield shield = playerGO.GetComponent<Shield>();
                if (shield != null && shield.haveShield)
                    shield.GetDamage((int)damage, true);
                else if (pl.canGetHit)
                {
                    pl.getHit = true;
                    Debug.Log("SniperEnemy: Player HIT by laser at arrival");
                }
            }
        }

        // Opcional: puedes esperar un poco más antes de continuar
        yield break;
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
            Debug.Log("SniperEnemy: No se encontró PlayerLife o canGetHit es false");
        }
    }


    protected override void Die()
    {
        Money.score += 50 * Money.multiplier;
        if (StartMenuManager.tutorial == 0)
        {
            ActualDialogueTutorial.startChangeLines = true;
        }

        base.Die();
    }
}
