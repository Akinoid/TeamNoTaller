using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Shield : MonoBehaviour
{
    [SerializeField] private bool haveShield;
    [SerializeField] private GameObject shieldHUD;
    [SerializeField] private Image shieldBar;
    [SerializeField] private int shield = 100, shieldMax = 100;
    private GameObject canvas;
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        shieldHUD = canvas.transform.Find("ShieldBorder").gameObject;
        shieldBar = canvas.transform.Find("ShieldBar").GetComponent<Image>();
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
    }

    private void DeactiveShield()
    {
        shieldHUD.SetActive(false);
    }

    private void ShieldValue()
    {
        shieldBar.fillAmount = shield / shieldMax;
        if(shield <= 0)
        {
            haveShield = false;
        }
    }
    private void ActivateShield()
    {        
        shieldHUD.SetActive(true);
    }
    
}
