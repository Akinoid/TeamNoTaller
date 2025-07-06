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

            if (shield.haveShield)
            {
                shield.GetDamage(30, true);
            }
            else if (life != null && life.canGetHit && !shield.haveShield)
            {
                life.getHit = true;
                Debug.Log("Player got Hit");
            }

        }
    }
}