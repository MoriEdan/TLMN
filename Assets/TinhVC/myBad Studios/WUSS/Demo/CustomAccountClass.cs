using UnityEngine;
using System.Collections;
using MBS;

//This script demonstrates that you need never modify the LoginGUI class as it is merely a front end
//The underlaying WUServer sends out events that you can plug into and that way link your code with
//the login kit...

public class CustomAccountClass : MonoBehaviour {
	
	public WULoginGUI
		wu_login;

	void Start()
	{
		if (null == wu_login)
		{
			Debug.LogError("Not connected to the WULogin prefab");
			return;
		}

		wu_login.onLoggedIn += OnLoggedIn;
		wu_login.onLoggedOut += OnLoggedOut;
	}

	void OnLoggedIn(object data)
	{
		Debug.Log ("Yeah! Logged in! Now I can load my level!");
	}

	void OnLoggedOut(object data)
	{
		Debug.Log("Oh, no, yo! Like in game over, yo. Time to load the main menu scene again");
	}
}
