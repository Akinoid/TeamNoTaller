using System.Collections;
using UnityEngine;

public class FistBarrage : MonoBehaviour,IBossAttack
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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("FistBarrage: No se encontró el jugador.");
            yield break;
        }
        PlayerLife life = player.GetComponent<PlayerLife>();

        for (int i = 0; i < numberOfPunches; i++)
        {
            // Elegimos una posición aleatoria alrededor del jugador
            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(1f, attackRadius);
            Vector3 targetPos = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, 0);

            // Instancia del área de peligro
            GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);
            Destroy(warning, destroyDelay);

            // Espera del telégrafo
            yield return new WaitForSeconds(warningDuration);

            // Instancia del impacto del puño
            GameObject fist=Instantiate(fistImpactPrefab, targetPos, Quaternion.identity);
            Collider[] hits = Physics.OverlapBox(warning.transform.position, warning.transform.localScale / 2);
            foreach (var h in hits)
                if (h.CompareTag("Player"))
                {
                    Debug.Log("BrawlerEnemy: Player HIT by DangerZone!");
                    life.getHit = true;
                }
            Destroy(warning, destroyDelay);
            Destroy(fist, destroyDelay);
           
            // Espera antes del siguiente golpe
            yield return new WaitForSeconds(delayBetweenPunches);

            
        }

        Debug.Log("Fist Barrage terminado");
    }
}

