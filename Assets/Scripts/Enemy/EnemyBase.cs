using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    public enum State { Entering, Active, Exiting }
    public enum EntryType { Straight, Spiral, TopDown }

    [Header("Entry / Exit Settings")]
    public float entrySpeed = 5f;
    public float entryZStart = 50f;
    public float entryZTarget = 25f;
    public float activeLifetime = 10f;
    public float exitSpeed = 5f;
    [SerializeField] protected EntryType entryType = EntryType.Spiral;

    [Header("Entry Area (X×Y)")]
    [Tooltip("Centro del área de convergencia (solo X/Y)")]
    public Vector2 entryAreaCenter = Vector2.zero;
    [Tooltip("Tamaño del área de convergencia en X e Y")]
    public Vector2 entryAreaSize = new Vector2(20f, 20f);

    protected Vector2 entryCenter;

    [Header("Health & Explosion")]
    public float health = 100f;
    public float currentHealth;
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;

    protected State currentState = State.Entering;
    public float activeTimer;
    protected Vector3 entryTargetPos;
    protected float entryTime = 0f;

    private List<GameObject> associatedObjects = new List<GameObject>();

    protected bool isDying = false;

    Vector3 spawnpoint;

    private PlayerActions playerActions;
    protected virtual void Start()
    {
        currentHealth = health;
        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();

        // 1) Coloca al spawn point en Z de entrada
        spawnpoint = transform.position;
        spawnpoint.z = entryZStart;
        transform.position = spawnpoint;

        // 2) Elige el centro DEL ÁREA de entrada (X/Y)
        float halfX = entryAreaSize.x * 0.5f;
        float halfY = entryAreaSize.y * 0.5f;
        entryCenter = new Vector2(
            UnityEngine.Random.Range(entryAreaCenter.x - halfX, entryAreaCenter.x + halfX),
            UnityEngine.Random.Range(entryAreaCenter.y - halfY, entryAreaCenter.y + halfY)
        );

        // 3) Ahora que ya conoces entryCenter, arma el destino 3D completo
        entryTargetPos = new Vector3(entryCenter.x, entryCenter.y, entryZTarget);

        // 4) Resto de inicialización
        activeTimer = activeLifetime;
        entryTime = 0f;
        entryType = (EntryType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EntryType)).Length);

        Debug.Log($"{name} START at {transform.position}, going to {entryTargetPos} via {entryType}");
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case State.Entering: HandleEntering(); break;
            case State.Active: HandleActive(); break;
            case State.Exiting: HandleExiting(); break;
        }
    }

    protected void RegisterAssociatedObject(GameObject obj)
    {
        if (obj != null)
            associatedObjects.Add(obj);
    } 

    protected void UnregisterAssociatedObject(GameObject obj)
    {
        if (obj != null)
            associatedObjects.Remove(obj);
    }
    protected GameObject InstantiateAssociated(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject go;
        if (parent != null)
            go = Instantiate(prefab, pos, rot, parent);
        else
            go = Instantiate(prefab, pos, rot);
        RegisterAssociatedObject(go);
        return go;
    }

    protected virtual void HandleEntering()
    {
        entryTime += Time.deltaTime;
        Vector3 newPos = transform.position;

        switch (entryType)
        {
            case EntryType.Straight:
                // Muévete directamente al Vector3 completo
                newPos = Vector3.MoveTowards(
                    transform.position,
                    entryTargetPos,
                    entrySpeed * Time.deltaTime
                );
                break;

            case EntryType.Spiral:
                {
                    float totalDist = Vector3.Distance(spawnpoint, entryTargetPos);
                    float duration = totalDist / entrySpeed;
                    float t = Mathf.Clamp01(entryTime / duration);

                    // Z lineal
                    float z = Mathf.Lerp(spawnpoint.z, entryZTarget, t);

                    // Radio dinámico alrededor de entryCenter.xy
                    float initialRadius = Vector2.Distance(
                        new Vector2(spawnpoint.x, spawnpoint.y),
                        entryCenter
                    );
                    float radius = Mathf.Lerp(initialRadius, 0f, t);

                    float angle = t * Mathf.PI * 3f;
                    float x = entryCenter.x + Mathf.Cos(angle) * radius;
                    float y = entryCenter.y + Mathf.Sin(angle) * radius;

                    newPos = new Vector3(x, y, z);
                    break;
                }

            case EntryType.TopDown:
                {
                    // Converge en XY hacia entryCenter
                    Vector3 midXY = Vector3.MoveTowards(
                        transform.position,
                        new Vector3(entryCenter.x, entryCenter.y, transform.position.z),
                        entrySpeed * Time.deltaTime
                    );
                    // Y oscila
                    float yOffset = Mathf.Sin(entryTime * Mathf.PI * 1f - Mathf.PI / 2f) * 2f;
                    // Z avanza
                    float z = Mathf.MoveTowards(transform.position.z, entryZTarget, entrySpeed * Time.deltaTime);

                    newPos = new Vector3(midXY.x, entryCenter.y + yOffset, z);
                    break;
                }
        }

        transform.position = newPos;

        // ■ CHEQUEO DE LLEGADA: compara contra entryTargetPos (X/Y/Z)
        if (Vector3.Distance(transform.position, entryTargetPos) < 0.2f)
        {
            currentState = State.Active;
            Debug.Log($"{name} ENTER COMPLETE – now Active");
            OnEnterComplete();
        }
    }

    protected virtual void HandleActive()
    {
        activeTimer -= Time.deltaTime;
        if (activeTimer <= 0f)
        {
            Debug.Log($"{name} ACTIVE TIME UP – starting Exit");
            currentState = State.Exiting;
            OnExitStart();
        }
    }

    protected virtual void HandleExiting()
    {
        transform.position += Vector3.forward * exitSpeed * Time.deltaTime;
    }

    protected virtual void OnExitStart()
    {
        Invoke(nameof(ReEnter), 3f);
    }

    private void ReEnter()
    {
        activeTimer = activeLifetime;
        Vector3 p = transform.position;
        p.z = entryZStart;
        transform.position = p;

        currentState = State.Entering;
        Debug.Log($"{name} RE-ENTER – back to Entering");
    }

    public void TakeDamage(float amount)
    {
        
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
            if (health <= 0f) Explode();
        }
        
    }

    private void Explode()
    {
        Debug.Log($"{name} EXPLODED at {transform.position}");
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var h in hits)
        {
            var other = h.GetComponent<EnemyBase>();
            if (other != null && other != this)
                other.TakeDamage(explosionDamage);
        }
        Destroy(gameObject);
    }
    protected virtual void DamagePlayer(GameObject player)
    {
        PlayerLife life = player.GetComponent<PlayerLife>();
        
        if (life != null && life.canGetHit)
        {
            life.getHit = true;
            Debug.Log("Player got Hit");
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && playerActions.gunType == PlayerActions.GunType.baseShoot)
        {
            Debug.Log("bullet hizo " + playerActions.baseShootDmg);
            TakeDamage(playerActions.baseShootDmg);
        }
        if (other.CompareTag("PlayerBullet") && playerActions.gunType == PlayerActions.GunType.blasterShoot)
        {
            Debug.Log("blaster hizo " + playerActions.blasterShootDmg);
            TakeDamage(playerActions.blasterShootDmg);
        }
        if (other.CompareTag("Missile"))
        {
            TakeDamage(currentHealth);
        }
        if (other.gameObject.CompareTag("Explosion"))
        {
            TakeDamage(currentHealth);
        }
                    
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            TakeDamage(currentHealth);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerActions.playerLife.haveBubble && Bubble.haveElectricBuff)
        {
            TakeDamage(playerActions.electricBubbleDmg);
        }
    }
    protected virtual void Die()
    {
        Money.combo.Add(+1);
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, entryTargetPos);

        if (entryType == EntryType.Spiral)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < 100; i++)
            {
                float t = i / 100f;
                float angle = t * Mathf.PI * 6f;
                float radius = 3f;
                float z = Mathf.Lerp(transform.position.z, entryTargetPos.z, t);
                Vector3 p = new Vector3(
                    transform.position.x + Mathf.Cos(angle) * radius,
                    transform.position.y + Mathf.Sin(angle) * 0.5f,
                    z
                );
                Gizmos.DrawSphere(p, 0.05f);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var obj in associatedObjects)
        {
            if (obj != null)
                DestroyImmediate(obj, true);
        }
        associatedObjects.Clear();
    }

    protected abstract void OnEnterComplete();
}
