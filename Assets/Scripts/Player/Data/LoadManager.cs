using UnityEngine;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private Money money;
    [SerializeField] private Shop shop;
    int firstSave;
    public bool reset;
    private void Awake()
    {
        Time.timeScale = 1;
        if (reset)
        {
           PlayerPrefs.SetInt("FirstSave", 0);
            Debug.Log(firstSave);

        }
        firstSave = PlayerPrefs.GetInt("FirstSave", 0);
        Debug.Log(firstSave);
        if (firstSave == 0)
        {
            Save();
        }
        Load();
        Save();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void Save()
    {
        if(shop != null)
        {
            SaveManager.SavePlayerData(shop);
            Debug.Log("Saved");
        }
        if(money != null)
        {
            SaveManager.SaveMoneyData(money);
        }
    }

    public void Load()
    {
        
        PlayerData playerData = SaveManager.LoadPlayerData();
        Debug.Log("Loaded");
        if (shop != null)
        {
            shop.saveFireballBuyed = playerData.fireballBuyed;
            shop.saveShieldBuyed = playerData.shieldBuyed;
            shop.saveBubbleBuyed = playerData.bubbleBuyed;
            shop.saveSigilBuyed = playerData.sigilBuyed;
            shop.saveBlasterBuyed = playerData.blasterBuyed;
            shop.saveHaveEletricBuff = playerData.HaveEletricBuff;
        }
        else
        {
            Debug.Log("Shop es null");
        }

        MoneyData moneyData = SaveManager.LoadMoneyData();

        money.money = moneyData.money;
    }

    public void ResetButton()
    {
        shop.saveFireballBuyed = false;
        shop.saveShieldBuyed = false;
        shop.saveBubbleBuyed = false;
        shop.saveSigilBuyed = false;
        shop.saveBlasterBuyed = false;
        shop.saveHaveEletricBuff = false;
        shop.ChangesBools();
        shop.ChangeBuyButtonTMP();
    }
}
