using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    public float damage = 40f;
    public float duration = 0.5f;
    private bool alreadyHit = false;

    private void Start()
    {
        Destroy(gameObject, duration); // Se autodestruye luego del tiempo
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyHit && other.CompareTag("Player"))
        {
            alreadyHit = true;
            Debug.Log("Player hit by explosion!");
            DamageUtils.DamagePlayer(other.gameObject);
        }
    }
}