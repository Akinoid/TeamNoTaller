using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class Shield : MonoBehaviour
{
    [SerializeField] private bool haveShield, getDamaged;
    [SerializeField] private GameObject shieldHUD;
    [SerializeField] private Image shieldBar;
    [SerializeField] private TMP_Text shieldTMP;
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private float shield, shieldMax, timer;
    private GameObject canvas, shieldBarGameObject;
    void Start()
    {
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
        getDamaged = getDamage;
        if (timer >= 1)
        {
            shield -= damage;
            shieldBar.fillAmount = shield / shieldMax;
            shieldTMP.text = $"Shield = {+shield}";
            timer = 0;
            getDamaged = false;
        }
        
    }
    private void TimerDamage()
    {
        if (getDamaged)
        {
            timer += Time.deltaTime;
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
        }
    }
}
