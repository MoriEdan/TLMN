using UnityEngine;
using System.Collections;

public class AvatarModel {

    private string price = "";
    private string id = "";
    private string url = "";

    public string Price
    {
        get { return price; }
        set { price = value; }
    }

    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Url
    {
        get { return url; }
        set { url = value; }
    }
}
