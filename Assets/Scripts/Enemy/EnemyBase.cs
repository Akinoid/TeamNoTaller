using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float health = 100f;
    public float entrySpeed = 10f;
    public float exitSpeed = 5f;
    public float lifeTime = 30f;
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    protected enum State { Entering, Attacking, Exiting, WaitingToReEnter }
    protected State currentState = State.Entering;

    protected float lifeTimer;
    protected Vector3 targetEntryPosition;

    void Start()
    {
        lifeTimer = lifeTime;
        SetEntryPosition();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Entering:
                MoveIntoScene();
                MoveIntoScene();
                break;
            case State.Exiting:
                ExitScene();
                break;
        }

        if (currentState == State.Attacking || currentState == State.Entering)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                StartExit();
            }
        }
    }

    protected void SetEntryPosition()
    {
        // Ajusta según la dirección que desees
        targetEntryPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 10f);
    }

    protected void MoveIntoScene()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetEntryPosition, entrySpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetEntryPosition) < 0.1f)
        {
            currentState = State.Attacking;
            OnEnterComplete();
        }
    }

    protected void ExitScene()
    {
        transform.Translate(Vector3.up * Mathf.Sin(Time.time * 2f) * Time.deltaTime); // Movimiento en espiral
        transform.Translate(Vector3.forward * exitSpeed * Time.deltaTime);
    }

    protected void StartExit()
    {
        currentState = State.Exiting;
        Invoke("StartReEntry", 3f);
    }

    protected void StartReEntry()
    {
        transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-6f, 6f), 20f);
        SetEntryPosition();
        lifeTimer = lifeTime;
        currentState = State.Entering;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Explode();
        }
    }

    protected void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var obj in hitObjects)
        {
            EnemyBase otherEnemy = obj.GetComponent<EnemyBase>();
            if (otherEnemy != null && otherEnemy != this)
            {
                otherEnemy.TakeDamage(explosionDamage);
            }
        }

        // Aquí puedes instanciar un efecto de explosión
        Destroy(gameObject);
    }

    protected abstract void OnEnterComplete();
    
  

}
