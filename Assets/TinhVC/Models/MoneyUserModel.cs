using UnityEngine;
using System.Collections;

public class MoneyUserModel  {

    private string moneyOnePay = "";
    private string moneyXen = "";
    private string moneyChip = "";
    private string moneyDiamond = "";


    public string MoneyOnePay
    {
        get { return moneyOnePay; }
        set { moneyOnePay = value; }
    }

    public string MoneyXen
    {
        get { return moneyXen; }
        set { moneyXen = value; }
    }

    public string MoneyChip
    {
        get { return moneyChip; }
        set { moneyChip = value; }
    }

    public string MoneyDiamond
    {
        get { return moneyDiamond; }
        set { moneyDiamond = value; }
    }
}
