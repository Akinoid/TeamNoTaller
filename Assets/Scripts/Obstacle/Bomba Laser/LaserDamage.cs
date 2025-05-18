using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public float duration = 3f;
    public int damage = 60;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //damage al escudo
        }
    }
}