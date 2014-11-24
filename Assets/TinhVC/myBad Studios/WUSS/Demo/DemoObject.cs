using UnityEngine;
using System.Collections;
using MBS;

//This script demonstrates how you can pop the login prefab back into view again from your own code
//and make it show any menu you like...
public class DemoObject : MonoBehaviour {

	public WULoginGUI
		AccountPrefab;

	void OnGUI()
	{
		if (AccountPrefab.LoginState != WULStates.Dummy)
			return;

		if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 30), WULogin.display_name))
		{
			//select the "I am logged in, show me my account info" menu to show
			AccountPrefab.LoginState = WULStates.AccountMenu;

			//then slide the window into view...
			AccountPrefab.displayArea.Activate();

			//Done!
		}
	}

}
