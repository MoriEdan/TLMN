using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MBS {
	public enum WULStates			{	Dummy, LoginChallenge, AccountMenu, RegisterAccount, ValidateLoginStatus, LoginMenu, Logout,
										PasswordReset, PasswordChange, FetchAccountDetails, UpdateAccountDetails, AccountInfo, ProfileImage, 
										Count, none_action ,SendDID }
	public enum WULGravatarTypes	{	MysteryMan, Identicon, Monsterid, Wavatar, Retro, Blank }

	//This class is the actual Wordpress user account system
	//This is the interface between the server and any front ends you might want to use.


	/*
	 * It's use is rather simplistic, actually.
	 * It contains a bunch of functions, each one corresponding to an action to perform on the server
	 * Each action, upon successful completion of the action, it will call a delegate function which you can
	 * hook into to perform your custom behavior.
	 * 
	 * By default, no action is taken upon an action failure as failure means a problem with the database
	 * or your code or soemthing that should be taken care of pre-deployment of your project. The server
	 * does have a delegate function it calls when something goes wrong so you can always ook into that
	 * during development.
	 * 
	 * So, to sum it up real simply, create your GUI code and have it call the functions defined in this class
	 * Attach functiosn to the relevant response delegates and you are good to go.
	 * 
	 * Due to this very generic nature of the kit it means you can use this with OnGUI, NGUI, UGUI, Daikon Forge
	 * and any other display system you might be using. 
	 * 
	 * Displaying an interface is up to you. Making it functional is this class's job...
	 * 
	 */

	/*
	   AVAILABLE FUNCTIONS
	   -------------------
	   	protected void RegisterAccount(cmlData fields)
		protected void ResetPassword(cmlData fields)
		protected void ChangePassword(cmlData fields)
		protected void AttemptAutoLogin()
		protected void LogOut()
		protected void AttemptToLogin(cmlData fields)
		protected void FetchPersonalInfo()
		protected void UpdatePersonalInfo(cmlData fields)
	 */

	/*
	  AVAILABLE RESPONSE DELEGATES
	  ----------------------------
		onRegistered
		onReset
		onPasswordChanged
		onLoggedIn
		onLoggedOut
		onAccountInfoReceived
		onInfoUpdated
	 */


	public class WULogin : WUServer {

		static public string 
			display_name = "",
			nickname = "";

		public System.Action<object>
			onRegistered,
			onReset,
			onLoggedIn,
			onLoggedOut,
			onAccountInfoReceived,
			onInfoUpdated,
			onPasswordChanged,
            onSendDID;

		public System.Action<Texture2D>
			onProfileImageReceived;

		protected mbsSlider
			activePanel,
			nextPanel;

		protected void RegisterAccount(cmlData fields)
		{
			onServerContactSucceeded = onRegisteredSuccess;
            ContactServer(onError, WULStates.RegisterAccount, fields);
		}
		
		protected void ResetPassword(cmlData fields)
		{
			onServerContactSucceeded = onResetSuccess;
            ContactServer(onError, WULStates.PasswordReset, fields);
		}

		protected void ChangePassword(cmlData fields)
		{
			onServerContactSucceeded = onPasswordChangedSuccess;
            ContactServer(onError, WULStates.PasswordChange, fields);
		}
		
		protected void AttemptAutoLogin()
		{
			onServerContactSucceeded = onLoginSuccess;
            ContactServer(onError, WULStates.ValidateLoginStatus);
		}
		
		protected void LogOut()
		{
			onServerContactSucceeded = onLogOutSuccess;
            ContactServer(onError, WULStates.Logout);
		}
		
		protected void AttemptToLogin(cmlData fields)
		{
			onServerContactSucceeded = onLoginSuccess;
            ContactServer(onError, WULStates.LoginChallenge, fields);
		}

		public void FetchProfileImage(System.Action<Texture2D> response)
		{
			FetchProfileImage(response, WULGravatarTypes.Identicon);
		}
		public void FetchProfileImage(System.Action<Texture2D> response, WULGravatarTypes gravatar_type)
		{
			onProfileImageReceived = response;

			if (null == onProfileImageReceived)
				return;

			onServerContactSucceeded = onProfileImageFetched;
			cmlData data = new cmlData();
			data.Set("gravatar_type", gravatar_type.ToString());
            ContactServer(onError, WULStates.ProfileImage, data);
		}

		protected void FetchPersonalInfo()
		{
			onServerContactSucceeded = onAccountInfoReceivedSuccess;
            ContactServer(onError, WULStates.FetchAccountDetails);
		}
		
		protected void UpdatePersonalInfo(cmlData fields)
		{
			onServerContactSucceeded = onUpdateInfoSuccess;
            ContactServer(onError, WULStates.UpdateAccountDetails, fields);
		}
		
		virtual public void onRegisteredSuccess(object data)
		{
			if (null != onRegistered)
				onRegistered(data);
		}
		
		virtual public void onResetSuccess(object data)
		{
			if (null != onReset)
				onReset(data);
		}

		virtual public void onPasswordChangedSuccess(object data)
		{
			if (null != onPasswordChanged)
				onPasswordChanged(data);
		}
		
		virtual public void onLoginSuccess(object data)
		{
			if (null != onLoggedIn)
				onLoggedIn(data);
		}

        virtual public void onSendDIDSuccess(object data)
        {
            if (null != onSendDID)
                onSendDID(data);
        }
		
		virtual public void onLogOutSuccess(object data)
		{
			if (null != onLoggedOut)
				onLoggedOut(data);
		}

		virtual public void onAccountInfoReceivedSuccess(object data)
		{
			if (null != onAccountInfoReceived)
				onAccountInfoReceived(data);
		}

		virtual public void onUpdateInfoSuccess(object data)
		{
			if (null != onInfoUpdated)
				onInfoUpdated(data);
		}

		virtual public void onProfileImageFetched(object data)
		{
			cmlData response = (cmlData) data;

			string gravatar_type_name = response.String("gravatar_type");
			WULGravatarTypes type = WULGravatarTypes.Blank;
			for (WULGravatarTypes t = WULGravatarTypes.MysteryMan; t < WULGravatarTypes.Blank; t++)
			{
				if (t.ToString().ToLower() == gravatar_type_name.ToLower())
					type = t;
			}
			StartCoroutine (ContactGravatar(response.String("gravatar"), type));
		}

		public IEnumerator ContactGravatar(string gravatar)
		{
			yield return ContactGravatar(gravatar, WULGravatarTypes.Identicon);
		}

		public IEnumerator ContactGravatar(string gravatar, WULGravatarTypes gravatar_type = WULGravatarTypes.Identicon)
		{

			string URL = "http://www.gravatar.com/avatar/"+gravatar+"?s=128&d="+ gravatar_type.ToString().ToLower();
			WWW w = new WWW(URL);
			yield return w;
			Texture2D avatar = null;
			if (w.error != null)
			{
				avatar = new Texture2D(1,1);
				avatar.SetPixel(0,0, Color.white);
				avatar.Apply();
			} else
			{
				avatar= w.texture;
			}
			onProfileImageReceived(avatar);
		}

 
       void onError(string error) { 

        }

	}

}
