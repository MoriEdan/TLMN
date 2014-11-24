using UnityEngine;
using System.Collections;

public class FacebookModel {

    private string idFacebook = "";
    private string email = "";
    private string username = "";

    public string IdFacebook
    {
        get { return idFacebook; }
        set { idFacebook = value; }
    }

    public string Email
    {
        get { return email; }
        set { email = value; }
    }

    public string Username
    {
        get { return username; }
        set { username = value; }
    }
}
