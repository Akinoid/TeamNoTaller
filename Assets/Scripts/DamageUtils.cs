using UnityEngine;

public static class DamageUtils
{
    public static void DamagePlayer(GameObject player)
    {
        PlayerLife life = player.GetComponent<PlayerLife>();
        if (life != null && life.canGetHit)
        {
            life.getHit = true;
            Debug.Log("Player got Hit!");
        }
    }
}
