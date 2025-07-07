using UnityEngine;
using System.Collections;

public class RocketFinger : MonoBehaviour
{
    public float chargeSpeed = 20f;
    public float attackHitRadius = 1f;
    public GameObject dangerSymbolPrefab;
    public float warningDuration = 1.5f;

    private bool isReturning = false;

    public void LaunchTo(Vector3 target, bool returning, Vector3? warningPos = null)
    {
        isReturning = returning;
        StartCoroutine(AttackRoutine(target, warningPos));
    }

    private IEnumerator AttackRoutine(Vector3 targetPos, Vector3? warningPos)
    {

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null)
        {
            Debug.LogWarning("RocketFinger: No se encontró al jugador");
            yield break;
        }

        // Mostrar advertencia solo si es la fase de regreso
        if (isReturning && dangerSymbolPrefab != null && warningPos.HasValue)
        {
            GameObject warning = Instantiate(dangerSymbolPrefab, warningPos.Value, dangerSymbolPrefab.transform.rotation);
            Renderer rend = warning.GetComponentInChildren<Renderer>();

            float t = 0f;
            while (t < warningDuration)
            {
                if (rend != null)
                    rend.enabled = !rend.enabled;
                yield return new WaitForSeconds(0.2f);
                t += 0.2f;
            }

            Destroy(warning);
        }

        AudioManager.Instance.Play("Boss Shoot");
        // Movimiento hacia el objetivo
        transform.LookAt(targetPos);
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Debug.Log("Player hit by finger");

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