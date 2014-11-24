using UnityEngine;
using System.Collections;
using MBS;

public class WULProfileImg : MonoBehaviour {

	WULogin 
		login;

	public WULGravatarTypes
		gravatar_type = WULGravatarTypes.Wavatar;

	static public Texture2D
		ProfileImage = null;

	// Use this for initialization
	void Start () {
		login = GameObject.FindObjectOfType<WULogin>();
		if (null == login)
			enabled = false;
		else
			login.onLoggedIn += GetProfileImage;
	}

	void GetProfileImage(object data)
	{
		login.FetchProfileImage( SetProfileImage, gravatar_type );
	}

	void SetProfileImage(Texture2D image)
	{
		ProfileImage = image;
	}
}
