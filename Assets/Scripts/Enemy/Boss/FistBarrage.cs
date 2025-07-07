using System.Collections;
using UnityEngine;

public class FistBarrage : MonoBehaviour, IBossAttack
{
    public GameObject warningPrefab;
    public GameObject fistImpactPrefab;
    public int numberOfPunches = 5;
    public float delayBetweenPunches = 0.1f;
    public float warningDuration = 1.5f;
    public float destroyDelay = 2f;
    public float attackRadius = 4f;

    public IEnumerator Execute(Boss boss)
    {
        Debug.Log("Fist Barrage iniciado");

        Animator animator = boss.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool("Golpe", true);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("FistBarrage: No se encontró el jugador.");
            yield break;
        }
        PlayerLife life = player.GetComponent<PlayerLife>();

        for (int i = 0; i < numberOfPunches; i++)
        {

            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(1f, attackRadius);
            Vector3 targetPos = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, 0);


            GameObject warning = Instantiate(warningPrefab, targetPos, warningPrefab.transform.rotation);
            Destroy(warning, destroyDelay);


            yield return new WaitForSeconds(warningDuration);
            GameObject fist = Instantiate(fistImpactPrefab, targetPos, Quaternion.identity);
            fist.transform.localScale = new Vector3(200, 200, 200);

            var bc = warning.GetComponent<BoxCollider>();
            if (bc != null)
            {
                // Calcula el tamaño en mundo:
                Vector3 worldSize = Vector3.Scale(bc.size, warning.transform.lossyScale);
                Vector3 halfExtents = worldSize * 0.5f;
                // Calcula el centro en mundo (ten en cuenta bc.center si no está en (0,0,0)):
                Vector3 worldCenter = warning.transform.position + warning.transform.rotation * Vector3.Scale(bc.center, warning.transform.lossyScale);

                // Ahora sí:
                Collider[] hits = Physics.OverlapBox(
                    worldCenter,
                    halfExtents,
                    warning.transform.rotation
                );
                foreach (var h in hits)
                {
                    if (h.CompareTag("Player"))
                    {
                        Debug.Log("Player HIT by DangerZone!");
                        DamagePlayer(h.gameObject);
                    }
                }
                Destroy(warning, destroyDelay);
                Destroy(fist, destroyDelay);

                yield return new WaitForSeconds(delayBetweenPunches);



            }

        }

        Debug.Log("Fist Barrage terminado");
        animator.SetBool("Golpe", false);
    }

    private void DamagePlayer(GameObject player)
    {
        PlayerLife life = player.GetComponent<PlayerLife>();

        Shield shield = player.GetComponent<Shield>();

        Bubble bubble = player.GetComponent<Bubble>();

        if (shield.haveShield)
        {
            shield.GetDamage(50, true);
            
        }
        else if (life != null && life.canGetHit && !shield.haveShield)
        {
            life.getHit = true;
            
        }
        else if (life != null && life.canGetHit && life.haveBubble)
        {
            bubble.getHitBubble = true;
        }
    }



}

