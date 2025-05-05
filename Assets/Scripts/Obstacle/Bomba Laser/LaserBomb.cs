using UnityEngine;

public class LaserBomb : MonoBehaviour
{
    public int maxHealth = 20;
    [SerializeField]private int currentHealth;


    public GameObject laserBeamPrefab; 
    public float laserDuration = 3f;   
    public float laserLength = 10f;

    private bool exploded = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (exploded) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;
        SpawnLaser(Vector3.down);
        SpawnLaser(Vector3.up);
        SpawnLaser(Vector3.left);
        SpawnLaser(Vector3.right);
        Destroy(gameObject); 
    }

    void SpawnLaser(Vector3 direction)
    {
        GameObject laser = Instantiate(laserBeamPrefab, transform.position, Quaternion.identity);
        laser.transform.forward = direction;
        laser.transform.localScale = new Vector3(0.2f, 0.2f, laserLength); 

        LaserDamage ld = laser.GetComponent<LaserDamage>();
        if (ld != null)
        {
            ld.duration = laserDuration;
            ld.damage = 60;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("PlayerBullet"))
        {
            TakeDamage(20);
            
        }
    }
}
