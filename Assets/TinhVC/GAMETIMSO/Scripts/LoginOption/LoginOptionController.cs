using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LoginOptionController : MonoBehaviour {


	// Use this for initialization
	void Start () {
        CheckRememberLogin();
        CallFBInit();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CheckRememberLogin()
    {
        if (LamaControllib.getInstance().getIsRememberLogin())
        {
            LamaControllib.getInstance().getUserModel().IdUser = LamaControllib.getInstance().getUserId();
            LamaControllib.getInstance().getUserModel().Username = LamaControllib.getInstance().getUsername();

            LamaControllib.getInstance().sendDIDService(LamaControllib.getInstance().getUserModel().IdUser, Utilities.GetUniqueIdentifier(), Utilities.GetDeviceName(), callBackSendDID, this);
        }
        else
        {

        }
    }


    void OnClickGuest()
    {
        LamaControllib.getInstance().signupUserGuestService("DID_" + Utilities.GetUniqueIdentifier(), Utilities.GetUniqueIdentifier(), Utilities.GetUniqueIdentifier() + "@centguesta.com", callBackSignupGuest, this);
    }
    void callBackSignupGuest(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            LamaControllib.getInstance().getUserModel().Username = "DID_" + Utilities.GetUniqueIdentifier();
            LamaControllib.getInstance().getUserModel().Nickname =  Utilities.GetDeviceName();
            LamaControllib.getInstance().setIsRememberLogin(true, LamaControllib.getInstance().getUserModel().IdUser, "DID_" + Utilities.GetUniqueIdentifier(), Utilities.GetUniqueIdentifier());

            // send device id
            LamaControllib.getInstance().sendDIDService(LamaControllib.getInstance().getUserModel().IdUser, Utilities.GetUniqueIdentifier(), Utilities.GetDeviceName(), callBackSendDID, this);
        }
        else
        {
            Debug.Log("tinhvc signup with device ID faile");
            //showError(jsonResult.ToString("VALUE"));
        }
    }

    void callBackSendDID(bool isSuccess, JSON jsonResult)
    {
       // Debug.Log("tinhvc send device ID: " + isSuccess);
        Application.LoadLevel("OptionScreen");
    }


    void OnClickFacbook()
    {
        CallFBLogin();
    }

    void OnClickSignup()
    {
        LamaControllib.getInstance().LoadLevel("Register");
    }

    void OnClickLogin()
    {
        LamaControllib.getInstance().LoadLevel("Login");
    }

    /**
    * FACEBOOk
    */
    #region FB.Init() example

    private bool isInit = false;

    private void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
        isInit = true;
        CallFBPublishInstall();
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }

    #endregion

    #region FB.Login() example

    private void CallFBLogin()
    {
        FB.Login("email,publish_actions", LoginCallback);
    }

    void LoginCallback(FBResult result)
    {
        if (result.Error != null)
            Debug.Log("tinhvc Error Response:\n" + result.Error);
        else if (!FB.IsLoggedIn)
        {
            Debug.Log("tinhvc Login cancelled by Player");
        }
        else
        {
            Debug.Log("tinhvc FB Result: " + result.Text);
            Debug.Log("tinhvc Login was successful!");
            FB.API("me", Facebook.HttpMethod.GET, LogInfoCallback);
        }
    }

    //WWW url = new WWW("https" + "://graph.facebook.com/" + userId + "/picture?type=large"); //+ "?access_token=" + FB.AccessToken);

    //        Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.DXT1, false); //TextureFormat must be DXT5

    //        yield return url;
    //        profilePic.renderer.material.mainTexture = textFb2;
    //        url.LoadImageIntoTexture(textFb2);
    //        Debug.Log("Working");

    void LogInfoCallback(FBResult response)
    {
        Debug.Log(response.Text);

        if (!response.Text.Equals(""))
        {
            JSON jsonFacebook = new JSON();
            jsonFacebook.serialized = response.Text;

            FacebookModel facebookModel = new FacebookModel();
            facebookModel.IdFacebook = jsonFacebook.ToString("id");
            facebookModel.Email = jsonFacebook.ToString("email");
            facebookModel.Username = jsonFacebook.ToString("name");
            LamaControllib.getInstance().setFacebookModel(facebookModel);
            string urlAvatar = "https" + "://graph.facebook.com/" + facebookModel.IdFacebook + "/picture?type=large";
            LamaControllib.getInstance().signupUserFacebookService(facebookModel.Username,facebookModel.Username,facebookModel.Email,urlAvatar,facebookModel.IdFacebook,callBackSignupFacebook,this);
        }

    }

    void callBackSignupFacebook(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            LamaControllib.getInstance().getUserModel().Username = LamaControllib.getInstance().getFacebookModel().Username;
            LamaControllib.getInstance().getUserModel().Nickname = LamaControllib.getInstance().getFacebookModel().Username;
            LamaControllib.getInstance().setIsRememberLogin(true, LamaControllib.getInstance().getUserModel().IdUser, LamaControllib.getInstance().getFacebookModel().Username, "");

            // send device id
            LamaControllib.getInstance().sendDIDService(LamaControllib.getInstance().getUserModel().IdUser, Utilities.GetUniqueIdentifier(), Utilities.GetDeviceName(), callBackSendDID, this);
        }
        else
        {
            Debug.Log("tinhvc signup with facebook ID faile");
            //showError(jsonResult.ToString("VALUE"));
        }
    }

    private void CallFBLogout()
    {
        FB.Logout();
    }
    #endregion

    #region FB.PublishInstall() example

    private void CallFBPublishInstall()
    {
        FB.PublishInstall(PublishComplete);
    }

    private void PublishComplete(FBResult result)
    {
        Debug.Log("publish response: " + result.Text);
    }

    #endregion

    #region FB.AppRequest() Friend Selector

    public string FriendSelectorTitle = "";
    public string FriendSelectorMessage = "Derp";
    public string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";
    public string FriendSelectorData = "{}";
    public string FriendSelectorExcludeIds = "";
    public string FriendSelectorMax = "";

    private void CallAppRequestAsFriendSelector()
    {
        // If there's a Max Recipients specified, include it
        int? maxRecipients = null;
        if (FriendSelectorMax != "")
        {
            try
            {
                maxRecipients = Int32.Parse(FriendSelectorMax);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // include the exclude ids
        string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');

        FB.AppRequest(
            FriendSelectorMessage,
            null,
            FriendSelectorFilters,
            excludeIds,
            maxRecipients,
            FriendSelectorData,
            FriendSelectorTitle,
            Callback
        );
    }
    #endregion

    #region FB.AppRequest() Direct Request

    public string DirectRequestTitle = "";
    public string DirectRequestMessage = "Herp";
    private string DirectRequestTo = "";

    private void CallAppRequestAsDirectRequest()
    {
        if (DirectRequestTo == "")
        {
            throw new ArgumentException("\"To Comma Ids\" must be specificed", "to");
        }
        FB.AppRequest(
            DirectRequestMessage,
            DirectRequestTo.Split(','),
            "",
            null,
            null,
            "",
            DirectRequestTitle,
            Callback
        );
    }

    #endregion

    #region FB.Feed() example

    public string FeedToId = "";
    public string FeedLink = "";
    public string FeedLinkName = "";
    public string FeedLinkCaption = "";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";
    public bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

    private void CallFBFeed()
    {
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: feedProperties,
            callback: Callback
        );
    }

    #endregion

    #region FB.Canvas.Pay() example

    public string PayProduct = "";

    private void CallFBPay()
    {
        FB.Canvas.Pay(PayProduct);
    }

    #endregion

    #region FB.API() example

    public string ApiQuery = "";

    private void CallFBAPI()
    {
        FB.API(ApiQuery, Facebook.HttpMethod.GET, Callback);
    }

    #endregion

    #region FB.GetDeepLink() example

    private void CallFBGetDeepLink()
    {
        FB.GetDeepLink(Callback);
    }

    #endregion

    #region FB.AppEvent.LogEvent example

    public float PlayerLevel = 1.0f;

    public void CallAppEventLogEvent()
    {
        var parameters = new Dictionary<string, object>();
        parameters[Facebook.FBAppEventParameterName.Level] = "Player Level";
        FB.AppEvents.LogEvent(Facebook.FBAppEventName.AchievedLevel, PlayerLevel, parameters);
        PlayerLevel++;
    }

    #endregion

    #region FB.Canvas.SetResolution example

    public string Width = "800";
    public string Height = "600";
    public bool CenterHorizontal = true;
    public bool CenterVertical = false;
    public string Top = "10";
    public string Left = "10";

    public void CallCanvasSetResolution()
    {
        int width;
        if (!Int32.TryParse(Width, out width))
        {
            width = 800;
        }
        int height;
        if (!Int32.TryParse(Height, out height))
        {
            height = 600;
        }
        float top;
        if (!float.TryParse(Top, out top))
        {
            top = 0.0f;
        }
        float left;
        if (!float.TryParse(Left, out left))
        {
            left = 0.0f;
        }
        if (CenterHorizontal && CenterVertical)
        {
            FB.Canvas.SetResolution(width, height, false, 0, FBScreen.CenterVertical(), FBScreen.CenterHorizontal());
        }
        else if (CenterHorizontal)
        {
            FB.Canvas.SetResolution(width, height, false, 0, FBScreen.Top(top), FBScreen.CenterHorizontal());
        }
        else if (CenterVertical)
        {
            FB.Canvas.SetResolution(width, height, false, 0, FBScreen.CenterVertical(), FBScreen.Left(left));
        }
        else
        {
            FB.Canvas.SetResolution(width, height, false, 0, FBScreen.Top(top), FBScreen.Left(left));
        }
    }

    #endregion

    void Callback(FBResult result)
    {
        //lastResponseTexture = null;
        // Some platforms return the empty string instead of null.
        if (!String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Error Response:\n" + result.Error);
        }
        else if (!ApiQuery.Contains("/picture"))
        {
            Debug.Log("Success Response:\n" + result.Text);
        }
        else
        {
            //lastResponseTexture = result.Texture;
            Debug.Log("Success Response:\n");
        }
    }
}
