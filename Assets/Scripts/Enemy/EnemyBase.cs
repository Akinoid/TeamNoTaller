using UnityEngine;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    public enum State { Entering, Active, Exiting }

    [Header("Entry / Exit Settings")]
    public float entrySpeed = 5f;
    public float entryZStart = 50f;
    public float entryZTarget = 25f;
    public float activeLifetime = 1000000f;
    public float exitSpeed = 5f;

    [Header("Health & Explosion")]
    public float health = 100f;
    private float currentHealth;
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;

    protected State currentState = State.Entering;
    public float activeTimer;
    private Vector3 entryTargetPos;

    private PlayerActions playerActions;
    protected virtual void Start()
    {
        currentHealth = health;
        // Spawn fuera de cámara en Z
        Vector3 p = transform.position;
        p.z = entryZStart;
        transform.position = p;

        // Destino de entrada
        entryTargetPos = new Vector3(transform.position.x, transform.position.y, entryZTarget);

        activeTimer = activeLifetime;
        Debug.Log($"{name} START – state=Entering at {transform.position} → {entryTargetPos}");

        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();
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

    protected virtual void HandleEntering()
    {
        transform.position = Vector3.MoveTowards(transform.position, entryTargetPos, entrySpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, entryTargetPos) < 0.1f)
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

    private void HandleExiting()
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
        Money.combo.Add(1);
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
    public void DamagePlayer(GameObject player)
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
            TakeDamage(playerActions.baseShootDmg); 
        }
        if (other.CompareTag("PlayerBullet") && playerActions.gunType == PlayerActions.GunType.blasterShoot)
        {
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
        Destroy(gameObject);
    }

    protected abstract void OnEnterComplete();
}
