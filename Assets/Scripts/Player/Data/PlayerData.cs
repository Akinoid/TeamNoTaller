[System.Serializable]
public class PlayerData
{
    public bool fireballBuyed, shieldBuyed, bubbleBuyed, sigilBuyed, blasterBuyed, HaveEletricBuff;
    public PlayerData(Shop shop)
    {
        fireballBuyed = shop.saveFireballBuyed;
        shieldBuyed = shop.saveShieldBuyed;
        bubbleBuyed = shop.saveBubbleBuyed;
        sigilBuyed = shop.saveSigilBuyed;
        blasterBuyed = shop.saveBlasterBuyed;
        HaveEletricBuff = shop.saveHaveEletricBuff;
    }
}
