using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    public float damage = 40f;
    public float duration = 0.5f;

    private void Start()
    {
        Destroy(gameObject, duration); // Se autodestruye luego del tiempo
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by explosion!");
            // Aqu� puedes aplicar da�o si tienes un sistema de salud en el jugador
            // Ejemplo:
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}