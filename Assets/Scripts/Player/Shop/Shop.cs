using UnityEngine;
using TMPro;
public class Shop : MonoBehaviour
{
    public bool saveFireballBuyed, saveShieldBuyed, saveBubbleBuyed, saveSigilBuyed, saveBlasterBuyed, saveHaveEletricBuff;
    public static bool fireballBuyed, shieldBuyed, bubbleBuyed, sigilBuyed, blasterBuyed;
    [SerializeField] private TMP_Text bubbleTMP, bubbleDescriptionTMP, bubbleBuyTMP;
    [SerializeField] private TMP_Text fireBallTMP, fireBallDescriptionTMP, fireBallBuyTMP;
    [SerializeField] private TMP_Text shieldTMP, shieldDescriptionTMP, shieldBuyTMP;
    [SerializeField] private TMP_Text sigilTMP, sigilDescriptionTMP, sigilBuyTMP;
    [SerializeField] private TMP_Text blasterTMP, blasterDescriptionTMP, blasterBuyTMP;
    [SerializeField] private Money money;
    [SerializeField] private LoadManager load;

    private void Awake()
    {
        
    }
    private void Start()
    {
        ChangesBools();
        Debug.Log(saveFireballBuyed);
        Debug.Log(fireballBuyed);
        TMPS();
        ChangeBuyButtonTMP();
        money = GameObject.Find("MoneyManager").GetComponent<Money>();
    }
    private void TMPS()
    {
        fireBallTMP = transform.Find("ShopPanel").Find("FireBallTMP").GetComponent<TMP_Text>();
        fireBallDescriptionTMP = fireBallTMP.transform.Find("FireBallDescriptionTMP").GetComponent<TMP_Text>();
        fireBallBuyTMP = fireBallTMP.transform.Find("FireBallBuyButton").Find("FireBallBuyTMP").GetComponent<TMP_Text>();

        bubbleTMP = transform.Find("ShopPanel").Find("BubbleTMP").GetComponent<TMP_Text>();
        bubbleDescriptionTMP = bubbleTMP.transform.Find("BubbleDescriptionTMP").GetComponent<TMP_Text>();
        bubbleBuyTMP = bubbleTMP.transform.Find("BubbleBuyButton").Find("BubbleBuyTMP").GetComponent<TMP_Text>();

        shieldTMP = transform.Find("ShopPanel").Find("ShieldTMP").GetComponent<TMP_Text>();
        shieldDescriptionTMP = shieldTMP.transform.Find("ShieldDescriptionTMP").GetComponent<TMP_Text>();
        shieldBuyTMP = shieldTMP.transform.Find("ShieldBuyButton").Find("ShieldBuyTMP").GetComponent<TMP_Text>();

        sigilTMP = transform.Find("ShopPanel").Find("SigilTMP").GetComponent<TMP_Text>();
        sigilDescriptionTMP = sigilTMP.transform.Find("SigilDescriptionTMP").GetComponent<TMP_Text>();
        sigilBuyTMP = sigilTMP.transform.Find("SigilBuyButton").Find("SigilBuyTMP").GetComponent<TMP_Text>();

        blasterTMP = transform.Find("ShopPanel").Find("BlasterTMP").GetComponent<TMP_Text>();
        blasterDescriptionTMP = blasterTMP.transform.Find("BlasterDescriptionTMP").GetComponent<TMP_Text>();
        blasterBuyTMP = blasterTMP.transform.Find("BlasterBuyButton").Find("BlasterBuyTMP").GetComponent<TMP_Text>();
    }
    public void ChangesBools()
    {
        fireballBuyed = saveFireballBuyed;
        shieldBuyed = saveShieldBuyed;
        bubbleBuyed = saveBubbleBuyed;
        sigilBuyed = saveSigilBuyed;
        blasterBuyed = saveBlasterBuyed;
        Bubble.haveElectricBuff = saveHaveEletricBuff;

    }
    public void ChangeBuyButtonTMP()
    {
        if (fireballBuyed)
        {
            fireBallBuyTMP.text = $"Already buyed";
        }
        else
        {
            fireBallBuyTMP.text = "Buy $200";
        }
        if (shieldBuyed)
        {
            shieldBuyTMP.text = $"Already buyed";
        }
        else
        {
            shieldBuyTMP.text = "Buy $1000";
        }
        if (bubbleBuyed)
        {
            bubbleTMP.text = $"Electric Bubble";
            bubbleBuyTMP.text = $"Buy $20000";
            bubbleDescriptionTMP.text = $"When you catch a Electric Bubble, block all the incoming damage and if you collision with an enemy, him receive 50 points of damage. Last 15 seconds after you receive a hit.";
        }
        else
        {
            bubbleTMP.text = "Bubble: ";
            bubbleBuyTMP.text = "Buy $10000";
            bubbleDescriptionTMP.text = "When you catch a Bubble, block all incoming damage. Last 10 seconds after you receive a hit and shorter if you receive more hits.";
        }
        if (Bubble.haveElectricBuff)
        {
            bubbleBuyTMP.text = $"Already buyed";
        }
        if (sigilBuyed)
        {
            sigilBuyTMP.text = $"Already buyed";
        }
        else
        {
            sigilBuyTMP.text = "Buy $10000";
        }
        if (blasterBuyed)
        {
            blasterBuyTMP.text = $"Already buyed";
        }
        else
        {
            blasterBuyTMP.text = "Buy $25000";
        }
    }
    public void FireballBuyButton()
    {
        if (!fireballBuyed && money.money >= 200)
        {
            money.money -= 200;
            saveFireballBuyed = true;
            load.Save();
            ChangesBools();
        }
        if (fireballBuyed)
        {
            fireBallBuyTMP.text = $"Already buyed";
        }
    }
    public void ShieldBuyButton()
    {
        if (!shieldBuyed && money.money >= 1000)
        {
            money.money -= 1000;
            saveShieldBuyed = true;
            load.Save();
            ChangesBools();
        }
        if (shieldBuyed)
        {
            shieldBuyTMP.text = $"Already buyed";
        }
    }
    public void BubbleBuyButton()
    {
        if (!bubbleBuyed && money.money >= 10000)
        {
            saveBubbleBuyed = true;
            ChangesBools();
            money.money -= 10000;
            load.Save();
            Debug.Log(saveBubbleBuyed + "bubble");
        }
        if (saveBubbleBuyed)
        {
            bubbleTMP.text = $"Electric Bubble";
            bubbleBuyTMP.text = $"Buy $20000";
            bubbleDescriptionTMP.text = $"When you catch a Electric Bubble, block all the incoming damage and if you collision with an enemy, him receive 50 points of damage. Last 15 seconds after you receive a hit.";
        }
        if (bubbleBuyed && !Bubble.haveElectricBuff && money.money >= 20000)
        {
            money.money -= 20000;
            saveHaveEletricBuff = true;
            load.Save();
            ChangesBools();
        }
        if (Bubble.haveElectricBuff)
        {
            bubbleBuyTMP.text = $"Already buyed";
        }
    }
    public void SigilBuyButton()
    {
        if (!sigilBuyed && money.money >= 10000)
        {
            money.money -= 10000;
            saveSigilBuyed = true;
            load.Save();
            ChangesBools();
        }
        if (sigilBuyed)
        {
            sigilBuyTMP.text = $"Already buyed";
        }
    }
    public void BlasterBuyButton()
    {
        if (!blasterBuyed && money.money >= 25000)
        {
            money.money -= 25000;
            saveBlasterBuyed = true;
            load.Save();
            ChangesBools();
        }
        if (blasterBuyed)
        {
            blasterBuyTMP.text = $"Already buyed";
        }
    }
}
