using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class Shield : MonoBehaviour
{
    [SerializeField] public bool haveShield, getDamaged, canGetDamage;
    [SerializeField] private GameObject shieldHUD;
    [SerializeField] private Image shieldBar;
    [SerializeField] private TMP_Text shieldTMP;
    [SerializeField] private float shield, shieldMax, timer;
    [SerializeField] private Sprite HUD100;
    [SerializeField] private Sprite HUD30;
    private PlayerLife playerLife;
    private PlayerActions playerActions;
    private GameObject canvas, shieldBarGameObject;
    void Start()
    {
        canGetDamage = true;
        canvas = GameObject.Find("Canvas");
        //shieldHUD = canvas.transform.Find("ShieldBorder").gameObject;
        shieldBar = canvas.transform.Find("ShieldBar").GetComponent<Image>();
        shieldBarGameObject = canvas.transform.Find("ShieldHUD").gameObject;
        shieldTMP = shieldBarGameObject.transform.Find("ShieldTMP").GetComponent<TMP_Text>();
        playerLife = gameObject.GetComponent<PlayerLife>();
        playerActions = gameObject.GetComponent<PlayerActions>();
    }

    void Update()
    {
       
        if (haveShield)
        {
            ActivateShield();
            ShieldValue();
            TimerDamage();
        }
        else
        {
            shieldBar.fillAmount = shield / shieldMax;
            shieldTMP.text = $"Shield: {+shield}";
            DeactiveShield();
        }
        
    }

    private void ShieldValue()
    {
        shieldBar.fillAmount = shield / shieldMax;
        shieldTMP.text = $"Shield: {+shield}";
        if (shield <= 0)
        {
            haveShield = false;
            shield = 0;
        }
        if(shield >= shieldMax)
        {
            shield = shieldMax;
            shieldBar.sprite = HUD100;
        }
        if(shield <= shieldMax / 3)
        {
            shieldBar.sprite = HUD30;
        }
    }
    public void GetDamage(float damage, bool getDamage)
    {
        
        if (canGetDamage && haveShield && !playerLife.haveBubble)
        {
            timer = 0;
            shield -= damage;
            Debug.Log("Ganamos Shield sufrio daño");
            Money.startRest = true;
            shieldBar.fillAmount = shield / shieldMax;
            shieldTMP.text = $"Shield = {+shield}";
            getDamaged = getDamage;
            Money.startRest = false;
            canGetDamage = false;
        }
        
        
    }
    private void TimerDamage()
    {
        if (getDamaged)
        {
            AudioManager.Instance.Play("Player Hit");
            timer += Time.deltaTime;
        }
        if (!canGetDamage)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 1 && playerActions.state != PlayerActions.MovementState.dashing)
        {
            getDamaged = false;
            canGetDamage = true;
            timer = 0;
        }
        if(playerActions.state == PlayerActions.MovementState.dashing)
        {
            timer = 0;
            canGetDamage = false;
        }
    }
    private void ActivateShield()
    {
        //shieldHUD.SetActive(true);
        playerLife.canGetHit = false;
    }
    private void DeactiveShield()
    {
        //shieldHUD.SetActive(false);
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
