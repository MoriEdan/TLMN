using UnityEngine;
using System.Collections;

public class UserModel
{

    private string idUser = "";

    public string IdUser
    {
        get { return idUser; }
        set { idUser = value; }
    }
    private string username = "";

    public string Username
    {
        get { return username; }
        set { username = value; }
    }

    private string nickname = "";

    public string Nickname
    {
        get { return nickname; }
        set { nickname = value; }
    }

    private string email = "";

    public string Email
    {
        get { return email; }
        set { email = value; }
    }

    private string avatar = "";

    public string Avatar
    {
        get { return avatar; }
        set { avatar = value; }
    }
}
