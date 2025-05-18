using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int maxHealth = 2000;
    private int currentHealth;

    public GameObject weakPoint; 
    public Slider healthBar;     

    private enum BossPhase { Phase1, Phase2, Phase3, Phase4 }
    private BossPhase currentPhase;

    private bool isAttacking;
    private float phaseTimer;

    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = BossPhase.Phase1;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        weakPoint.SetActive(false);
    }

    void Update()
    {
        if (!isAttacking)
        {
            StartCoroutine(BossRoutine());
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator BossRoutine()
    {
        isAttacking = true;

        
        ChooseAttackPattern();

        
        weakPoint.SetActive(true);
        yield return new WaitForSeconds(5f);
        weakPoint.SetActive(false);

        
        yield return StartCoroutine(PerformAttack());

        isAttacking = false;
    }

    private void ChooseAttackPattern()
    {
        if (currentHealth < 2000)
            currentPhase = BossPhase.Phase1;
        else if (currentHealth < 1500)
            currentPhase = BossPhase.Phase2;
        else if (currentHealth < 1000)
            currentPhase = BossPhase.Phase3;
        else if(currentHealth < 500)
            currentPhase = BossPhase.Phase4;

    }

    private System.Collections.IEnumerator PerformAttack()
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                yield return StartCoroutine(FistBarrage());
                break;
            case BossPhase.Phase2:
                yield return StartCoroutine(MissileShot());
                break;
          
        }
    }

    private System.Collections.IEnumerator FistBarrage()
    {
        Debug.Log("Rafaga de puños");
        
        yield return new WaitForSeconds(2f);
    }
        

    private System.Collections.IEnumerator MissileShot()
    {
        Debug.Log("Disparo de misiles");
        yield return new WaitForSeconds(4f);
    }
   

    private void Die()
    {
        Debug.Log("El jefe ha sido derrotado.");
        Destroy(gameObject);
    }
}
