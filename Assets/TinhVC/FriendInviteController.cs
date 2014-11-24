using UnityEngine;
using System.Collections;

public class FriendInviteController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseProfile()
    {
        transform.parent.gameObject.transform.parent.gameObject.SendMessage("CloseFriendList");
        //GameObject.Destroy(gameObject.transform.parent.gameObject);
    }
}
