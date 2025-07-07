using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    public float damage = 40f;
    public float duration = 4f;
    private bool alreadyHit = false;

    private void Start()
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyHit && other.CompareTag("Player"))
        {
            alreadyHit = true;
            Debug.Log("Player hit by explosion!");
            PlayerLife life = other.GetComponent<PlayerLife>();

            Shield shield = other.GetComponent<Shield>();

            Bubble bubble = other.GetComponent<Bubble>();

            if (shield.haveShield)
            {
                shield.GetDamage(30, true);
                Debug.Log("Escudo Funciona Lets go");
            }
            else if (life != null && life.canGetHit && !shield.haveShield)
            {
                life.getHit = true;
                Debug.Log("Player got Hit");
            }
            else if (life != null && life.canGetHit && life.haveBubble)
            {
                bubble.getHitBubble = true;
            }

        }
    }
}