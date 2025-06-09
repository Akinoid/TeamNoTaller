[System.Serializable]
public class MoneyData
{
    public float money;
    public MoneyData(Money money)
    {
        this.money = money.money;
    }
}
