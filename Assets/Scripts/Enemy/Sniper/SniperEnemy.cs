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

    protected override void Start()
    {
        base.Start();

        var pgo = GameObject.FindGameObjectWithTag("Player");
        if (pgo != null) playerTransform = pgo.transform;
        else Debug.LogError("SniperEnemy: No se encontró objeto con tag 'Player'");

        if (markerPrefab != null)
        {
            markerInstance = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
            markerInstance.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (currentState == State.Entering && playerTransform != null && !hasStartedAiming)
        {
            hasStartedAiming = true;
            StartCoroutine(AimAndFireRoutine());
        }
    }

    private IEnumerator AimAndFireRoutine()
    {
        if (markerInstance != null)
        {
            markerInstance.SetActive(true);
        }

        float timer = 0f;

        // Fase 1: marcador sigue al jugador
        while (timer < markerFollowTime)
        {
            if (playerTransform != null)
            {
                lastPlayerPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, markerOffsetZ);
                markerInstance.transform.position = lastPlayerPosition;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Fase 2: marcador se queda fijo
        float holdTime = totalAimTime - markerFollowTime;
        yield return new WaitForSeconds(holdTime);

        // Fase 3: disparar rayo
        if (markerInstance != null)
        {
            markerInstance.SetActive(false);
        }

        FireBeamAt(lastPlayerPosition);
    }

    private void FireBeamAt(Vector3 targetPos)
    {
        Vector3 start = transform.position;
        Vector3 dir = (targetPos - start).normalized;

        // Raycast para daño
        if (Physics.Raycast(start, dir, out var hit, beamRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("SniperEnemy: Player HIT by beam!");
                // Aquí puedes aplicar daño si tienes un sistema de salud
            }
        }

        // Visual del rayo
        if (beamPrefab != null)
        {
            var beam = Instantiate(beamPrefab, start, Quaternion.LookRotation(dir));
            Destroy(beam, beamDuration);
        }

        Debug.Log("SniperEnemy: Fired beam towards " + targetPos);
    }

    protected override void OnEnterComplete()
    {
        // Nada aquí, ya que ahora el disparo lo controla la corrutina
    }
}
