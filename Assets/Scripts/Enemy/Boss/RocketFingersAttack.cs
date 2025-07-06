using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketFingersAttack : MonoBehaviour, IBossAttack
{
    public GameObject rocketFingerPrefab;
    public int numberOfFingers = 3;
    public float delayBetweenLaunches = 0.5f;
    public float delayBeforeReturn = 3f;

    public float forwardDistance = 5f;

    public IEnumerator Execute(Boss boss)
    {
        Debug.Log("RocketFingersAttack iniciado");

        Animator animator = boss.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool("Finger", true);
        }
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null)
        {
            Debug.LogWarning("RocketFingers: No se encontró al jugador");
            yield break;
        }

        Vector3 bossPos = boss.transform.position;
        List<GameObject> spawnedFingers = new List<GameObject>();

        // Etapa 1: Disparo desde frente del boss hacia Z = -20
        for (int i = 0; i < numberOfFingers; i++)
        {
            Vector3 playerPos = playerGO.transform.position;
            Vector3 spawnPos = bossPos + Vector3.forward * forwardDistance + Vector3.right * (i - numberOfFingers / 2f);
            Vector3 backTarget = new Vector3(playerPos.x, playerPos.y, -20f);

            GameObject finger = Instantiate(rocketFingerPrefab, spawnPos, Quaternion.identity);
            RocketFinger rf = finger.GetComponent<RocketFinger>();
            if (rf != null)
            {
                rf.LaunchTo(backTarget, false);
            }

            spawnedFingers.Add(finger);
            yield return new WaitForSeconds(delayBetweenLaunches);
        }

        yield return new WaitForSeconds(delayBeforeReturn);

        // Etapa 2: Regreso desde Z = -20 hasta Z = 20, pasando por el jugador (Z = 0)
        foreach (GameObject finger in spawnedFingers)
        {
            if (finger != null)
            {
                Vector3 playerPos = playerGO.transform.position;
                Vector3 warningPos = new Vector3(playerPos.x, playerPos.y, 0f);     // Z = 0 (jugador)
                Vector3 returnTarget = new Vector3(playerPos.x, playerPos.y, 20f);  // Z = 20 (pasar de largo)

                RocketFinger rf = finger.GetComponent<RocketFinger>();
                if (rf != null)
                {
                    rf.LaunchTo(returnTarget, true, warningPos);
                }
            }
            
            yield return new WaitForSeconds(delayBetweenLaunches);
        }
        

        yield return new WaitForSeconds(3f);
        Debug.Log("RocketFingersAttack terminado");
        animator.SetBool("Finger", false);
        foreach (GameObject finger in spawnedFingers)
        {
            Destroy(finger);
        }


    }
}