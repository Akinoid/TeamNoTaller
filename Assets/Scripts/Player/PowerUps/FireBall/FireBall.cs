using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private EnemyFound enemyFound;
    [SerializeField] private ObstacleFound obstacleFound;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    private void Awake()
    {
        Rb();
    }
    private void Rb()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        FindObjective();
        Move();
    }

    private void FindObjective()
    {
        EnemyFound[] enemyFounds = FindObjectsOfType<EnemyFound>();
        ObstacleFound[] obstacleFounds = FindObjectsOfType<ObstacleFound>();

        if(enemyFounds.Length > 0)
        {
            EnemyFound enemyNear = enemyFounds[0];
            float distanceEnemyNear = Vector3.Distance(transform.position, enemyNear.transform.position);
            foreach (EnemyFound enemy in enemyFounds)
            {
                float distanceOfEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceEnemyNear > distanceOfEnemy)
                {
                    enemyNear = enemy;
                    distanceEnemyNear = distanceOfEnemy;
                }
            }
            enemyFound = enemyNear;
            target = enemyNear.transform;
        }
        else if(obstacleFounds.Length > 0)
        {
            enemyFound = null;
            ObstacleFound obstacleNear = obstacleFounds[0];
            float distanceObstacleNear = Vector3.Distance(transform.position, obstacleNear.transform.position);
            foreach (ObstacleFound obstacle in obstacleFounds)
            {
                float distanceOfObstacle = Vector3.Distance(transform.position, obstacle.transform.position);
                if (distanceObstacleNear > distanceOfObstacle)
                {
                    obstacleNear = obstacle;
                    distanceObstacleNear = distanceOfObstacle;
                }
            }
            obstacleFound = obstacleNear;
            target = obstacleNear.transform;
        }
        else
        {
            enemyFound = null;
            obstacleFound = null;
            target = null;
        }
    }

    private void Move()
    {
        if(enemyFound != null || obstacleFound != null)
        {
            direction = target.position - transform.position;
            direction = direction.normalized;
            direction *= speed;
            rb.linearVelocity = direction;
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, 0, speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

}
