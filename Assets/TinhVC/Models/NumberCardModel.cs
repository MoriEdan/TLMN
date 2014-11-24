using UnityEngine;
using System.Collections;

public class NumberCardModel {

    private string message = "";
    private string amount = "";

    public string Message
    {
        get { return message; }
        set { message = value; }
    }

    public string Amount
    {
        get { return amount; }
        set { amount = value; }
    }
}
