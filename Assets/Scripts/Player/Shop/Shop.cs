using UnityEngine;
using TMPro;
public class Shop : MonoBehaviour
{
    public static bool fireballBuyed, shieldBuyed, bubbleBuyed, sigilBuyed, blasterBuyed;
    [SerializeField] private TMP_Text bubbleTMP, bubbleDescriptionTMP, bubbleBuyTMP;

    private void Start()
    {
        bubbleTMP = transform.Find("BubbleTMP").GetComponent<TMP_Text>();
        bubbleDescriptionTMP = bubbleTMP.transform.Find("BubbleDescriptionTMP").GetComponent<TMP_Text>();
        bubbleBuyTMP = bubbleTMP.transform.Find("BubbleBuyButton").Find("BubbleBuyTMP").GetComponent<TMP_Text>();
    }
    public void FireballBuyButton()
    {
        if (!fireballBuyed && Money.money >= 200)
        {
            Money.money -= 200;
            fireballBuyed = true;
        }
    }
    public void ShieldBuyButton()
    {
        if (!shieldBuyed && Money.money >= 1000)
        {
            Money.money -= 1000;
            shieldBuyed = true;
        }
    }
    public void BubbleBuyButton()
    {
        if (!bubbleBuyed && Money.money >= 10000)
        {
            Money.money -= 10000;
            bubbleTMP.text = $"Electric Bubble";
            bubbleBuyTMP.text = $"Buy $20000";
            bubbleDescriptionTMP.text = $"When you catch a Electric Bubble, block all the incoming damage and if you collision with an enemy, him receive 50 points of damage. Last 15 seconds after you receive a hit.";
            bubbleBuyed = true;
        }
        if (bubbleBuyed && !Bubble.haveElectricBuff && Money.money >= 20000)
        {
            Money.money -= 20000;
            Bubble.haveElectricBuff = true;
        }
    }
    public void SigilBuyButton()
    {
        if (!sigilBuyed && Money.money >= 10000)
        {
            Money.money -= 10000;
            sigilBuyed = true;
        }
    }
    public void BlasterBuyButton()
    {
        if (!blasterBuyed && Money.money >= 25000)
        {
            Money.money -= 25000;
            blasterBuyed = true;
        }
    }
}
