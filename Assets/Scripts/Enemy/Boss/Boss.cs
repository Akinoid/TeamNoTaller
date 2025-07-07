using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Boss : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 2000;
    [SerializeField] private float currentHealth;
    public Slider healthBar;

    [Header("Fases")]
    public List<BossPhase> phases;
    [SerializeField] private BossPhase currentPhase;

    [Header("Visual")]
    public GameObject weakPoint;

    private bool isAttacking = false;

    public Action OnBossDeath;

    [SerializeField] private Animator animator;

    void Start()
    {
        healthBar = GameObject.Find("Boss Life").GetComponent<Slider>();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        weakPoint.SetActive(false);

        UpdatePhase();
        StartCoroutine(BossLoop());
    }

    void UpdatePhase()
    {
        foreach (var phase in phases)
        {
            if (currentHealth <= phase.minHealth)
                currentPhase = phase;
        }
    }
       

    IEnumerator BossLoop()
    {
        while (currentHealth > 0)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                
                weakPoint.SetActive(true);
                yield return new WaitForSeconds(5f);
                weakPoint.SetActive(false);

               
                var attacks = currentPhase.GetAttacks();
                if (attacks.Count > 0)
                {
                    var attack = attacks[UnityEngine.Random.Range(0, attacks.Count)];
                    yield return StartCoroutine(attack.Execute(this));
                }

                isAttacking = false;
            }

            yield return null;
        }
    }
    public void TakeWeakPointDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        Debug.Log("Boss hit on weak point! Remaining HP: " + currentHealth);
        UpdatePhase();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        AudioManager.Instance.Play("Enemy Die");
        Money.score += 1000 * Money.multiplier;
        Debug.Log("Boss derrotado.");
        OnBossDeath?.Invoke();
        Destroy(gameObject);
    }
}