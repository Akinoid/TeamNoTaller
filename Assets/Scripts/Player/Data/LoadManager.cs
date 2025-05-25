using UnityEngine;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private Money money;
    [SerializeField] private Shop shop;
    void Start()
    {
        Load();
        Save();
    }
    void Update()
    {
        
    }

    public void Save()
    {
        if(shop != null)
        {
            SaveManager.SavePlayerData(shop);
        }
        if(money != null)
        {
            SaveManager.SaveMoneyData(money);
        }
    }

    public void Load()
    {
        PlayerData playerData = SaveManager.LoadPlayerData();
        if(shop != null)
        {
            shop.saveFireballBuyed = playerData.fireballBuyed;
            shop.saveShieldBuyed = playerData.shieldBuyed;
            shop.saveBubbleBuyed = playerData.blasterBuyed;
            shop.saveSigilBuyed = playerData.sigilBuyed;
            shop.saveBlasterBuyed = playerData.blasterBuyed;
            shop.saveHaveEletricBuff = playerData.HaveEletricBuff;
        }       

        MoneyData moneyData = SaveManager.LoadMoneyData();

        money.money = moneyData.money;
    }
}
