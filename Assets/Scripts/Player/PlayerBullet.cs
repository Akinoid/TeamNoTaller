using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speedBullet;
    [SerializeField] private float timerBullet;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        TimerBullet();
    }

    private void FixedUpdate()
    {
        BulletMovement();
    }
    private void BulletMovement()
    {
        rb.AddForce(new Vector3(0, 0, speedBullet), ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
            Invoke(nameof(BulletAutoDestroy), 0.05f);
        }
    }

    private void TimerBullet()
    {
        timerBullet += Time.deltaTime;
        if(timerBullet >= 3f)
        {
            Invoke(nameof(BulletAutoDestroy), 0.05f);
        }
    }
    private void BulletAutoDestroy()
    {
        Destroy(gameObject);

    }
}
