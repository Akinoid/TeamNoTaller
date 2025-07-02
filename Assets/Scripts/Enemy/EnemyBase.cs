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
        // Spawn fuera de cámara en Z
        spawnpoint = transform.position;
        spawnpoint.z = entryZStart;
        transform.position = spawnpoint;

        // Destino de entrada
        entryTargetPos = new Vector3(transform.position.x, transform.position.y, entryZTarget);

        activeTimer = activeLifetime;
        Debug.Log($"{name} START – state=Entering at {transform.position} → {entryTargetPos}");

        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();

        entryType = (EntryType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(EntryType)).Length);

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
                newPos = Vector3.MoveTowards(transform.position, entryTargetPos, entrySpeed * Time.deltaTime);
                break;

            case EntryType.Spiral:
                float duration = Vector3.Distance(spawnpoint, entryTargetPos) / entrySpeed;
                float t = Mathf.Clamp01(entryTime / duration); // Normaliza entre 0 y 1

                
                float z = Mathf.Lerp(spawnpoint.z, entryTargetPos.z, t);

               
                float radius = Mathf.Lerp(6f, 0f, t); 
                float angle = t * Mathf.PI * 4f; 

                
                float x = spawnpoint.x + Mathf.Cos(angle) * radius;
                float y = spawnpoint.y + Mathf.Sin(angle) * radius * 0.5f;

                newPos = new Vector3(x, y, z);
                break;

            case EntryType.TopDown:
                float frequency = 0.5f; 
                float amplitude = 2f;   

                float yOffset = Mathf.Sin(entryTime * Mathf.PI * 2f * frequency - Mathf.PI / 2f) * amplitude;

                Vector3 forwardMove = Vector3.MoveTowards(transform.position, entryTargetPos, entrySpeed * Time.deltaTime);
                newPos = new Vector3(forwardMove.x, spawnpoint.y + yOffset, forwardMove.z);
                break;
        }

        transform.position = newPos;

        // condición de llegada (ajustada según el tipo)
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
    private void OnTriggerEnter(Collider other)
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
        if (other.CompareTag("Explosion"))
        {
            TakeDamage(playerActions.missileExplosionDmg);
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
