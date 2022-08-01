using UnityEngine;

public class Coins
{
    private const string KEY = "MONEY";
    public int Golds { get; private set; }

    public Coins()
    {
        Golds = 0;
    }

    public void Add(int value)
    {
        Golds += value;
    }

    public void Spend(int value)
    {
        if (!isEnoughtMoney(value))
            return;
        Golds -= value;
    }


    private bool isEnoughtMoney(int value)
    {
        return value <= Golds;
    }


    private void Save()
    {
        PlayerPrefs.SetInt(KEY, Golds);
    }
}
