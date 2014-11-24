using UnityEngine;
using System.Collections;

public class FriendModel
{

    private string idFriend = "";
    private string nameFriend = "";
    private string avatarFriend = "";

    public string AvatarFriend
    {
        get { return avatarFriend; }
        set { avatarFriend = value; }
    }

    public string IdFriend
    {
        get { return idFriend; }
        set { idFriend = value; }
    }

    public string NameFriend
    {
        get { return nameFriend; }
        set { nameFriend = value; }
    }
}
