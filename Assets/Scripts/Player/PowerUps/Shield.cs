using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class Shield : MonoBehaviour
{
    [SerializeField] private bool haveShield, getDamaged, canGetDamage;
    [SerializeField] private GameObject shieldHUD;
    [SerializeField] private Image shieldBar;
    [SerializeField] private TMP_Text shieldTMP;
    [SerializeField] private float shield, shieldMax, timer;
    private PlayerLife playerLife;
    private GameObject canvas, shieldBarGameObject;
    void Start()
    {
        canGetDamage = true;
        canvas = GameObject.Find("Canvas");
        shieldHUD = canvas.transform.Find("ShieldBorder").gameObject;
        shieldBar = shieldHUD.transform.Find("ShieldBar").GetComponent<Image>();
        shieldBarGameObject = shieldHUD.transform.Find("ShieldBar").gameObject;
        shieldTMP = shieldBarGameObject.transform.Find("ShieldTMP").GetComponent<TMP_Text>();
        playerLife = gameObject.GetComponent<PlayerLife>();
    }

    void Update()
    {
        if (haveShield)
        {
            ActivateShield();
            ShieldValue();
        }
        else
        {
            DeactiveShield();
        }
        TimerDamage();
    }

    private void ShieldValue()
    {
        shieldBar.fillAmount = shield / shieldMax;
        shieldTMP.text = $"Shield = {+shield}";
        if (shield <= 0)
        {
            haveShield = false;
        }
        if(shield >= shieldMax)
        {
            shield = shieldMax;
        }
    }
    public void GetDamage(float damage, bool getDamage)
    {
        
        if (canGetDamage && haveShield && !playerLife.haveBubble)
        {
            timer = 0;
            shield -= damage;
            shieldBar.fillAmount = shield / shieldMax;
            shieldTMP.text = $"Shield = {+shield}";
            getDamaged = getDamage;
            canGetDamage = false;
        }
        
        
    }
    private void TimerDamage()
    {
        if (getDamaged)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 1)
        {
            canGetDamage = true;
        }
    }
    private void ActivateShield()
    {
        shieldHUD.SetActive(true);
        playerLife.canGetHit = false;
    }
    private void DeactiveShield()
    {
        shieldHUD.SetActive(false);
        playerLife.canGetHit = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            shield = shieldMax;
            haveShield = true;
            Money.score += 150 * Money.multiplier;
            Destroy(other.gameObject);
        }
    }
}
