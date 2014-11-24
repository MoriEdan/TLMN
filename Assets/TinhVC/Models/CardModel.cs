using UnityEngine;
using System.Collections;

public class CardModel {
    private string card = "";
    private string xen = "";
    private string chip = "";

    public string Card
    {
        get { return card; }
        set { card = value; }
    }

    public string Xen
    {
        get { return xen; }
        set { xen = value; }
    }

    public string Chip
    {
        get { return chip; }
        set { chip = value; }
    }
}
