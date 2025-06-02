using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public Boss boss;
    private PlayerActions playerActions;

    private void Start()
    {
        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && playerActions.gunType == PlayerActions.GunType.baseShoot)
        {
            boss.TakeWeakPointDamage(playerActions.baseShootDmg);
        }
        if (other.CompareTag("PlayerBullet") && playerActions.gunType == PlayerActions.GunType.blasterShoot)
        {
            boss.TakeWeakPointDamage(playerActions.blasterShootDmg);
        }
    }
}
