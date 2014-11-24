using UnityEngine;
using System.Collections;

public class SettingController : MonoBehaviour {

    public tk2dTextMesh tvUsername;

	// Use this for initialization
	void Start () {
        tvUsername.text = LamaControllib.getInstance().getUserModel().Username;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClickLogout()
    {
        LamaControllib.getInstance().Logout();
    }
}
