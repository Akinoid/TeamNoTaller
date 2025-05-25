using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveManager
{
    public static void SavePlayerData(Shop shop)
    {
        PlayerData playerData = new PlayerData(shop);
        string dataPath = Application.persistentDataPath + "/playerShop.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
    }
    public static void SaveMoneyData(Money money)
    {
        MoneyData moneyData = new MoneyData(money);
        string dataPath = Application.persistentDataPath + "/playerMoney.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, moneyData);
        fileStream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        string dataPath = Application.persistentDataPath + "/playerShop.save";
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            PlayerData playerData = (PlayerData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return playerData;
        }
        else
        {
            return null;
        }
    }
    public static MoneyData LoadMoneyData()
    {
        string dataPath = Application.persistentDataPath + "/playerMoney.save";
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MoneyData moneyData = (MoneyData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return moneyData;
        }
        else
        {
            return null;
        }
    }
}
