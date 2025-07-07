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

    [SerializeField] private Transform visual;

    [SerializeField] private Animator animator;

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
            dangerSymbolInstance = Instantiate(dangerSymbolPrefab, dangerPos, dangerSymbolPrefab.transform.rotation);
            StartCoroutine(EntryChargeRoutine(dangerPos));
            RegisterAssociatedObject(dangerSymbolInstance);
        }
    }

    private IEnumerator EntryChargeRoutine(Vector3 dangerPos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -25);
        float timer = 0f;
        
        Renderer symbolRenderer = dangerSymbolInstance.GetComponentInChildren<Renderer>();
        while (timer < warningDuration)
        {
            /*if (symbolRenderer != null)
                symbolRenderer.enabled = !symbolRenderer.enabled;*/
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }
        

        Vector3 start = transform.position;
        transform.position = start - Vector3.forward * chargeDistanceBehind; // start behind
        transform.LookAt(dangerPos);

        while (Vector3.Distance(transform.position, new Vector3(dangerPos.x, dangerPos.y, positionInFrontOfPlayer)) > 0.1f)
        {
            animator.SetBool("Golpe", true);
            AudioManager.Instance.Play("Ambusher Attack");
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(dangerPos.x,dangerPos.y, positionInFrontOfPlayer), chargeSpeed * Time.deltaTime);
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
        
        //transform.position = playerTransform.position + playerTransform.forward * positionInFrontOfPlayer;
        visual.localRotation = Quaternion.Euler(0, 180, 0);
        currentState = State.Active;
        activeTimer = activeTimeBeforeExit;
        animator.SetBool("Golpe", false);
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

        animator.SetBool("Golpe", true);

        Vector3 target = new Vector3(transform.position.x, transform.position.y, -25);
        transform.LookAt(target);
        visual.localRotation = Quaternion.Euler(0, 0, 0);
        
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
        animator.SetBool("Golpe", false);
        currentState = State.Entering;
        isExiting = false;

    }
    
    protected override void DamagePlayer(GameObject player)
    {
        PlayerLife life = player.GetComponent<PlayerLife>();

        Shield shield = player.GetComponent<Shield>();

        Bubble bubble = player.GetComponent<Bubble>();

        if (shield.haveShield)
        {
            shield.GetDamage(30, true);
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
   

    protected override void OnEnterComplete() { /* Entry is handled with coroutine */ }
    protected override void Die()
    {
        Money.score += 200 * Money.multiplier;
        if(StartMenuManager.tutorial == 0)
        {
            ActualDialogueTutorial.startChangeLines = true;
        }
        base.Die();
    }
}
