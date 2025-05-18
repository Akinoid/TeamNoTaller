using UnityEngine;
using System.Collections;

public class AmbusherEnemy : EnemyBase
{
    [Header("Ambusher Settings")]
    public float chargeSpeed = 20f;
    public float warningDuration = 2f;
    public GameObject dangerSymbolPrefab;
    public float exitFlashDuration = 1f;
    public Color flashColor = Color.red;
    public float chargeDistanceBehind = 10f;
    public float positionInFrontOfPlayer = 5f;
    public float attackHitRadius = 1f;
    public float activeTimeBeforeExit = 10f;

    private Transform playerTransform;
    private GameObject dangerSymbolInstance;
    private Vector3 chargeTarget;
    private bool isExiting = false;

    private GameObject playerGO;

    protected override void Start()
    {
        base.Start();
        health = 30f;
        base.currentHealth = health;

        playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null) playerTransform = playerGO.transform;
        
    }
    protected override void Update()
    {
        base.Update();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.transform;
    }
    protected override void HandleEntering()
    {
        if (dangerSymbolInstance == null && playerTransform != null)
        {
            Vector3 dangerPos = playerTransform.position - playerTransform.forward * chargeDistanceBehind;
            dangerSymbolInstance = Instantiate(dangerSymbolPrefab, dangerPos, Quaternion.identity);
            StartCoroutine(EntryChargeRoutine(dangerPos));
        }
    }

    private IEnumerator EntryChargeRoutine(Vector3 dangerPos)
    {
        float timer = 0f;
        Renderer symbolRenderer = dangerSymbolInstance.GetComponentInChildren<Renderer>();
        while (timer < warningDuration)
        {
            if (symbolRenderer != null)
                symbolRenderer.enabled = !symbolRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }

        Vector3 start = transform.position;
        transform.position = start - Vector3.forward * chargeDistanceBehind; // start behind
        transform.LookAt(dangerPos);

        while (Vector3.Distance(transform.position, dangerPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dangerPos, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        // Check hit
        if (Vector3.Distance(playerTransform.position, dangerPos) < attackHitRadius)
        {
            Debug.Log("AmbusherEnemy: Player HIT during entry charge!");
            DamagePlayer(playerTransform.gameObject);
        }

        Destroy(dangerSymbolInstance);
        // Move in front of player
        transform.position = playerTransform.position + playerTransform.forward * positionInFrontOfPlayer;
        currentState = State.Active;
        activeTimer = activeTimeBeforeExit;
    }

    protected override void HandleActive()
    {
        base.HandleActive();

        if (!isExiting)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0f)
            {
                StartCoroutine(ExitChargeRoutine());
            }
        }
    }

    private IEnumerator ExitChargeRoutine()
    {
        isExiting = true;

        Renderer rend = GetComponentInChildren<Renderer>();
        float timer = 0f;
        while (timer < exitFlashDuration)
        {
            if (rend != null)
                rend.material.color = rend.material.color == flashColor ? Color.white : flashColor;
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }

        Vector3 target = transform.position - Vector3.forward * chargeDistanceBehind;
        transform.LookAt(target);

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        // Hit check
        Vector3 playerPos = playerTransform.position;
        Vector3 direction = (target - transform.position).normalized;
        Vector3 closestPoint = transform.position + direction * Vector3.Distance(transform.position, playerPos);
        if (Vector3.Distance(playerPos, closestPoint) < attackHitRadius)
        {
            Debug.Log("AmbusherEnemy: Player HIT during exit charge!");
            DamagePlayer(playerTransform.gameObject);
            
            
        }

        currentState = State.Entering;  // Optionally destroy or call base.HandleExiting()
    }
    
    protected override void DamagePlayer(GameObject player)
    {
        PlayerLife life = player.GetComponent<PlayerLife>();

        Shield shield = playerGO.GetComponent<Shield>();
        if (shield.haveShield)
        {
            shield.GetDamage(30,true);
        }
        else  if (life != null && life.canGetHit&& !shield.haveShield)
        {
            life.getHit = true;
            Debug.Log("Player got Hit");
        }
       
    }
   

    protected override void OnEnterComplete() { /* Entry is handled with coroutine */ }
    protected override void Die()
    {
        Money.score += 200 * Money.multiplier;
        base.Die();
    }
}
