using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LamaControllib
{

    private static LamaControllib lamaController;
    string key = "7d5cb12c25767e7b275048034235b10b";
    string number = "1";

    private LamaControllib()
    {
    }
    public static LamaControllib getInstance()
    {
        if (lamaController == null)
        {
            lamaController = new LamaControllib();
        }
        return lamaController;
    }

    /**
     * GET XP MODEL
     */
    private XPModel xpModel;

    public void setXPModel(XPModel _xpModel)
    {
        xpModel = _xpModel;
    }

    public XPModel getXPModel()
    {
        if (null == xpModel)
        {
            xpModel = new XPModel();
        }
        return xpModel;
    }

    /**
     * GET NUMBER CARD MODEL
     */
    private NumberCardModel numberCardModel;

    public void setNumberCardModel(NumberCardModel _numberCardModel)
    {
        numberCardModel = _numberCardModel;
    }

    public NumberCardModel getNumberCardModel()
    {
        if (null == numberCardModel)
        {
            numberCardModel = new NumberCardModel();
        }
        return numberCardModel;
    }

    /**
     * GET LIST FRIEND MODEL
     */
    private List<FriendModel> listFriend = new List<FriendModel>();
    public void setListFriend(List<FriendModel> _listFriend)
    {
        listFriend = _listFriend;
    }

    public List<FriendModel> getListFriend()
    {
        if (null == listFriend)
        {
            listFriend = new List<FriendModel>();
        }
        return listFriend;
    }

    /**
   * GET LIST CARD MODEL
   */
    private List<CardModel> listCard = new List<CardModel>();
    public void setListCard(List<CardModel> _listCard)
    {
        listCard = _listCard;
    }

    public List<CardModel> getListCard()
    {
        if (null == listCard)
        {
            listCard = new List<CardModel>();
        }
        return listCard;
    }

    /**
   * GET LIST SMS MODEL
   */
    private List<SMSModel> listSMS = new List<SMSModel>();
    public void setListSMS(List<SMSModel> _listSMS)
    {
        listSMS = _listSMS;
    }

    public List<SMSModel> getListSMS()
    {
        if (null == listSMS)
        {
            listSMS = new List<SMSModel>();
        }
        return listSMS;
    }

    /**
    * GET LIST AVATAR MODEL
    */
    private List<AvatarModel> listAvatar = new List<AvatarModel>();
    public void setListAvatar(List<AvatarModel> _listAvatar)
    {
        listAvatar = _listAvatar;
    }

    public List<AvatarModel> getListAvatar()
    {
        if (null == listAvatar)
        {
            listAvatar = new List<AvatarModel>();
        }
        return listAvatar;
    }

    /**
    * GET MONEY USER MODEL
    */
    private MoneyUserModel moneyUserModel = new MoneyUserModel();
    public void setMoneyUserModel(MoneyUserModel _moneyUserModel)
    {
        moneyUserModel = _moneyUserModel;
    }

    public MoneyUserModel getMoneyUserModel()
    {
        if (null == moneyUserModel)
        {
            moneyUserModel = new MoneyUserModel();
        }
        return moneyUserModel;
    }

    /**
     * GET USER MODEL
     */
    private UserModel userModel = new UserModel();
    public void setUserModel(UserModel _userModel)
    {
        userModel = _userModel;
    }

    public UserModel getUserModel()
    {
        if (null == userModel)
        {
            userModel = new UserModel();
        }
        return userModel;
    }


    /**
    * GET USER PARTNER MODEL
    */
    private PartnerModel partModel = new PartnerModel();
    public void setPartnerModel(PartnerModel _partModel)
    {
        partModel = _partModel;
    }

    public PartnerModel getPartnerModel()
    {
        if (null == partModel)
        {
            partModel = new PartnerModel();
        }
        return partModel;
    }
    /**
     * GET USER FACEBOOK MODEL
     */
    private FacebookModel facebookModel = new FacebookModel();
    public void setFacebookModel(FacebookModel _facebookModel)
    {
        facebookModel = _facebookModel;
    }

    public FacebookModel getFacebookModel()
    {
        if (null == facebookModel)
        {
            facebookModel = new FacebookModel();
        }
        return facebookModel;
    }

    /**
     * FUNCTION DELEGATE
     */
    public delegate void jsonCallback(JSON jsonResult);

    /**
     *GET XP MODEL SERVICE 
     */
    public delegate void callBackGetXP(bool isSuccess, JSON jsonResult);
    private callBackGetXP callBackGetXPService;
    public void getXPModelService(string idUser, callBackGetXP callBack, MonoBehaviour context)
    {
        callBackGetXPService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;
        jsonSend["game_id"] = 3;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_XP_URL, jsonSend.serialized, key, number, idUser, 0, getXPCallback));
    }

    void getXPCallback(JSON jsonResult)
    {
        if (jsonResult.ToBoolean("SUCCESS"))
        {
            XPModel xpModel = new XPModel();
            JSON jsonValue = jsonResult.ToJSON("VALUE");

            xpModel.Point = jsonValue.ToString("point");
            xpModel.Level = jsonValue.ToString("level");
            xpModel.Type = jsonValue.ToString("type");
            xpModel.Medal = jsonValue.ToString("medal");

            xpModel.NextLevel = jsonValue.ToString("next_level");
            xpModel.CurrentLevel = jsonValue.ToString("this_level");

            Double persentLevel = (float.Parse(xpModel.Point) - float.Parse(xpModel.CurrentLevel)) / (float.Parse(xpModel.NextLevel) - float.Parse(xpModel.CurrentLevel));

            persentLevel = Math.Round(persentLevel, 2);

            xpModel.Persent = "" + persentLevel;

            LamaControllib.getInstance().setXPModel(xpModel);

            callBackGetXPService(true, jsonResult);
        }
        else
        {
            callBackGetXPService(false, jsonResult);
        }
    }

    /**
     *SEND FEEDBACK SERVICE 
     */
    public delegate void callBacksendFeedback(bool isSuccess, JSON jsonResult);
    private callBacksendFeedback callBacksendFeedbackService;
    public void sendFeedbackService(string idUser, string content, callBacksendFeedback callBack, MonoBehaviour context)
    {
        callBacksendFeedbackService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;
        jsonSend["content"] = content;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_FEEDBACK_URL, jsonSend.serialized, key, number, idUser, 0, sendFeedbackCallback));
    }

    void sendFeedbackCallback(JSON jsonResult)
    {
        if (jsonResult.ToBoolean("SUCCESS"))
        {
            string value = jsonResult.ToString("VALUE");
            callBacksendFeedbackService(true, jsonResult);
        }
        else
        {
            callBacksendFeedbackService(false, jsonResult);
        }
    }

    /**
     *SEND NUMBER CARD SERVICE 
     */
    public delegate void callBacksendNumberCard(bool isSuccess, JSON jsonResult);
    private callBacksendNumberCard callBacksendNumberCardService;
    public void sendNumberCardService(string idUser, string cardType, string codeCard, string serialCard, string moneyID, callBacksendNumberCard callBack, MonoBehaviour context)
    {
        callBacksendNumberCardService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;
        jsonSend["card_type"] = cardType;
        jsonSend["pin"] = codeCard;
        jsonSend["serial"] = serialCard;
        jsonSend["invite_id"] = moneyID;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_NUMBER_CARD_URL, jsonSend.serialized, key, number, idUser, 0, sendNumberCardCallback));
    }

    void sendNumberCardCallback(JSON jsonResult)
    {
        JSON jsonValue = jsonResult.ToJSON("VALUE");
        string mess = jsonValue.ToString("message");
        string amount = jsonValue.ToString("amount");

        NumberCardModel numberCardModel = new NumberCardModel();
        numberCardModel.Message = mess;
        numberCardModel.Amount = amount;

        LamaControllib.getInstance().setNumberCardModel(numberCardModel);
        if (jsonResult.ToBoolean("SUCCESS"))
        {
            callBacksendNumberCardService(true, jsonResult);
        }
        else
        {
            callBacksendNumberCardService(false, jsonResult);
        }

    }

    /**
     *ADD FRIEND SERVICE 
     */
    public delegate void callBackAddFriend(bool isSuccess, JSON jsonResult);
    private callBackAddFriend callBackAddFriendService;
    public void addFriendService(string idUser, string idFriend, callBackAddFriend callBack, MonoBehaviour context)
    {
        callBackAddFriendService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;
        jsonSend["friend_id"] = idFriend;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.ADD_FRIEND_URL, jsonSend.serialized, key, number, idUser, 0, addFriendCallback));
    }

    void addFriendCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            callBackAddFriendService(true, jsonResult);
        }
        else
        {
            callBackAddFriendService(false, jsonResult);
        }
    }

    /**
     *GET LIST FRIEND SERVICE 
     */
    public delegate void callBackListFriend(bool isSuccess, JSON jsonResult);
    private callBackListFriend callBackListFriendService;
    public void getListFriendService(string idUser, callBackListFriend callBack, MonoBehaviour context)
    {
        callBackListFriendService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_FRIEND_URL, jsonSend.serialized, key, number, idUser, 0, getListFriendCallback));
    }

    void getListFriendCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON[] listFriendJsonArray = jsonResult.ToArray<JSON>("VALUE");
            LamaControllib.getInstance().setListFriend(new List<FriendModel>());
            for (int i = 0; i < listFriendJsonArray.Length; i++)
            {
                FriendModel friendModel = new FriendModel();
                friendModel.IdFriend = listFriendJsonArray[i].ToString("ID");
                friendModel.NameFriend = listFriendJsonArray[i].ToString("NAME");
                friendModel.AvatarFriend = listFriendJsonArray[i].ToString("avatar");
                LamaControllib.getInstance().getListFriend().Add(friendModel);
            }
            callBackListFriendService(true, jsonResult);
        }
        else
        {
            callBackListFriendService(false, jsonResult);
        }
    }

    /**
     *GET LIST CARD SERVICE 
     */
    public delegate void callBackListCard(bool isSuccess, JSON jsonResult);
    private callBackListCard callBackListCardService;
    public void getListCardService(string idUser, callBackListCard callBack, MonoBehaviour context)
    {
        callBackListCardService = callBack;
        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_CARD_URL, new JSON().serialized, key, number, idUser, 0, getListCardCallback));
    }

    void getListCardCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON[] listCardJsonArray = jsonResult.ToArray<JSON>("VALUE");
            LamaControllib.getInstance().setListCard(new List<CardModel>());
            for (int i = 0; i < listCardJsonArray.Length; i++)
            {
                CardModel cardModel = new CardModel();
                cardModel.Card = listCardJsonArray[i].ToString("card");
                cardModel.Xen = listCardJsonArray[i].ToString("xen");
                cardModel.Chip = listCardJsonArray[i].ToString("chip");
                LamaControllib.getInstance().getListCard().Add(cardModel);
            }
            callBackListCardService(true, jsonResult);
        }
        else
        {
            callBackListCardService(false, jsonResult);
        }
    }

    /**
     *GET LIST SMS SERVICE 
     */
    public delegate void callBackListSMS(bool isSuccess, JSON jsonResult);
    private callBackListSMS callBackListSMSService;
    public void getListSMSService(string idUser, callBackListSMS callBack, MonoBehaviour context)
    {
        callBackListSMSService = callBack;
        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_SMS_URL, new JSON().serialized, key, number, idUser, 0, getListSMSCallback));
    }

    void getListSMSCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON[] listCardJsonArray = jsonResult.ToArray<JSON>("VALUE");
            LamaControllib.getInstance().setListSMS(new List<SMSModel>());
            for (int i = 0; i < listCardJsonArray.Length; i++)
            {
                SMSModel cardModel = new SMSModel();
                cardModel.SMS = listCardJsonArray[i].ToString("sms");
                cardModel.Xen = listCardJsonArray[i].ToString("xen");
                cardModel.Chip = listCardJsonArray[i].ToString("chip");
                cardModel.SendNUmber = listCardJsonArray[i].ToString("send");
                LamaControllib.getInstance().getListSMS().Add(cardModel);
            }
            callBackListSMSService(true, jsonResult);
        }
        else
        {
            callBackListSMSService(false, jsonResult);
        }
    }

    /**
     *GET LIST AVATAR SERVICE 
     */
    public delegate void callBackListAvatar(bool isSuccess, JSON jsonResult);
    private callBackListAvatar callBackListAvatarService;
    public void getListAvatarService(string idUser, callBackListAvatar callBack, MonoBehaviour context)
    {
        callBackListAvatarService = callBack;
        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_AVATAR_URL, new JSON().serialized, key, number, idUser, 0, getListAvatarCallback));
    }

    void getListAvatarCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON[] listAvatarJsonArray = jsonResult.ToArray<JSON>("VALUE");
            LamaControllib.getInstance().setListAvatar(new List<AvatarModel>());
            for (int i = 0; i < listAvatarJsonArray.Length; i++)
            {
                AvatarModel avatarModel = new AvatarModel();
                avatarModel.Price = listAvatarJsonArray[i].ToString("price");
                avatarModel.Id = listAvatarJsonArray[i].ToString("id");
                avatarModel.Url = listAvatarJsonArray[i].ToString("url");
                LamaControllib.getInstance().getListAvatar().Add(avatarModel);
            }
            callBackListAvatarService(true, jsonResult);
        }
        else
        {
            callBackListAvatarService(false, jsonResult);
        }
    }

    /**
    *SEND REPORT USER SERVICE 
    */
    public delegate void callBackReportUser(bool isSuccess, JSON jsonResult);
    private callBackReportUser callBackReportUserService;
    public void sendReportService(string idUser, string idUserBad, callBackReportUser callBack, MonoBehaviour context)
    {
        callBackReportUserService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;
        jsonSend["bad_id"] = idUserBad;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_REPORT_USER_URL, jsonSend.serialized, key, number, idUser, 0, sendReportCallback));
    }

    void sendReportCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            callBackReportUserService(true, jsonResult);
        }
        else
        {
            callBackReportUserService(false, jsonResult);
        }
    }

    /**
    *GET ALL MONEY USER SERVICE 
    */
    public delegate void callBackGetAllMoney(bool isSuccess, JSON jsonResult);
    private callBackGetAllMoney callBackGetAllMoneyService;
    public void getAllMoneyUserService(string idUser, callBackGetAllMoney callBack, MonoBehaviour context)
    {
        callBackGetAllMoneyService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_ALL_MONEY_USER_URL, jsonSend.serialized, key, number, idUser, 0, getAllMoneyUserCallback));
    }

    void getAllMoneyUserCallback(JSON jsonResult)
    {

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON[] listMoneyJsonArray = jsonResult.ToArray<JSON>("VALUE");

            if (listMoneyJsonArray.Length >= 4)
            {
                MoneyUserModel moneyUserModel = new MoneyUserModel();
                moneyUserModel.MoneyOnePay = listMoneyJsonArray[0].ToString("money");
                moneyUserModel.MoneyXen = listMoneyJsonArray[1].ToString("money");
                moneyUserModel.MoneyChip = listMoneyJsonArray[2].ToString("money");
                moneyUserModel.MoneyDiamond = listMoneyJsonArray[3].ToString("money");
                setMoneyUserModel(moneyUserModel);

                callBackGetAllMoneyService(true, jsonResult);
            }
            else
            {
                callBackGetAllMoneyService(false, jsonResult);
            }

        }
        else
        {
            callBackGetAllMoneyService(false, jsonResult);
        }
    }

    /**
    *USER SIGNUP SERVICE 
    */
    public delegate void callBackSignup(bool isSuccess, JSON jsonResult);
    private callBackSignup callBackSignupService;
    public void signupUserService(string username, string password, string email, callBackSignup callBack, MonoBehaviour context)
    {
        callBackSignupService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["username"] = username;
        jsonSend["password"] = password;
        jsonSend["email"] = email;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.REGISTER_USER_URL, jsonSend.serialized, key, number, "", 0, userRegisterCallback));
    }

    void userRegisterCallback(JSON jsonResult)
    {
        Debug.Log(jsonResult.serialized);

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            //JSON[] listMoneyJsonArray = jsonResult.ToArray<JSON>("VALUE");
            getUserModel().IdUser = jsonResult.ToString("VALUE");
            callBackSignupService(true, jsonResult);
        }
        else
        {
            callBackSignupService(false, jsonResult);
        }
    }

    /**
    *USER SIGNUP GUEST SERVICE 
    */
    public delegate void callBackSignupGuest(bool isSuccess, JSON jsonResult);
    private callBackSignupGuest callBackSignupGuestService;
    public void signupUserGuestService(string username, string password, string email, callBackSignupGuest callBack, MonoBehaviour context)
    {
        callBackSignupGuestService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["username"] = username;
        jsonSend["password"] = password;
        jsonSend["email"] = email;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.REGISTER_USER_GUEST_URL, jsonSend.serialized, key, number, "", 0, userRegisterGuestCallback));
    }

    void userRegisterGuestCallback(JSON jsonResult)
    {
        Debug.Log(jsonResult.serialized);

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            //JSON[] listMoneyJsonArray = jsonResult.ToArray<JSON>("VALUE");
            getUserModel().IdUser = jsonResult.ToString("VALUE");
            callBackSignupGuestService(true, jsonResult);
        }
        else
        {
            callBackSignupGuestService(false, jsonResult);
        }
    }

    /**
   *USER SIGNUP FACEBOOK SERVICE 
   */
    public delegate void callBackSignupFacebook(bool isSuccess, JSON jsonResult);
    private callBackSignupFacebook callBackSignupFacebookService;
    public void signupUserFacebookService(string username, string displayName, string email, string avatar, string fbid, callBackSignupFacebook callBack, MonoBehaviour context)
    {
        callBackSignupFacebookService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["username"] = username;
        jsonSend["display_name"] = displayName;
        jsonSend["email"] = email;
        jsonSend["avatar"] = avatar;
        jsonSend["fbid"] = fbid;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.REGISTER_USER_FACEBOOK_URL, jsonSend.serialized, key, number, "", 0, userRegisterFacebookCallback));
    }

    void userRegisterFacebookCallback(JSON jsonResult)
    {
        Debug.Log(jsonResult.serialized);

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            //JSON[] listMoneyJsonArray = jsonResult.ToArray<JSON>("VALUE");
            getUserModel().IdUser = jsonResult.ToString("VALUE");
            callBackSignupFacebookService(true, jsonResult);
        }
        else
        {
            callBackSignupFacebookService(false, jsonResult);
        }
    }


    /**
    *SEND DEVICE ID SERVICE 
    */
    public delegate void callBackSendDID(bool isSuccess, JSON jsonResult);
    private callBackSendDID callBackSendDIDService;
    public void sendDIDService(string userID, string deviceID, string deviceName, callBackSendDID callBack, MonoBehaviour context)
    {
        callBackSendDIDService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = userID;
        jsonSend["device_id"] = deviceID;
        jsonSend["device_name"] = deviceName;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_DEVICE_ID_URL, jsonSend.serialized, key, number, "", 0, sendDIDCallback));
    }

    void sendDIDCallback(JSON jsonResult)
    {
        Debug.Log(jsonResult.serialized);

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            callBackSendDIDService(true, jsonResult);
        }
        else
        {
            callBackSendDIDService(false, jsonResult);
        }
    }

    /**
   *USER GET AVATAR SERVICE 
   */
    public delegate void callGetAvatar(bool isSuccess, JSON jsonResult);
    private callGetAvatar callGetAvatarService;
    public void getAvatarService(string idUser, callGetAvatar callBack, MonoBehaviour context, int pos)
    {
        callGetAvatarService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_AVATAR_URL, jsonSend.serialized, key, number, "", pos, callGetAvatarCallback));
    }

    void callGetAvatarCallback(JSON jsonResult)
    {
        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON jsonInfo = jsonResult.ToJSON("VALUE");
            getUserModel().Nickname = jsonInfo.ToString("name");
            JSON jsonAvatar = jsonInfo.ToJSON("avatar");
            getUserModel().Avatar = jsonAvatar.ToString("url");
            callGetAvatarService(true, jsonResult);
        }
        else
        {
            callGetAvatarService(false, jsonResult);
        }
    }
    /**
     *USER GET AVATAR PARTNER SERVICE 
     */
    public delegate void callGetAvatarPartner(bool isSuccess, JSON jsonResult);
    private callGetAvatarPartner callGetAvatarPartnerService;
    public void getAvatarPartnerService(string idUser, callGetAvatarPartner callBack, MonoBehaviour context)
    {
        callGetAvatarPartnerService = callBack;
        JSON jsonSend = new JSON();

        jsonSend["user_id"] = idUser;

        JsonMethod jsonMethod = new JsonMethod();
        context.StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_AVATAR_URL, jsonSend.serialized, key, number, "", 0, callGetAvatarPartnerCallback));
    }

    void callGetAvatarPartnerCallback(JSON jsonResult)
    {
        Debug.Log(jsonResult.serialized);

        if (jsonResult.ToBoolean("SUCCESS"))
        {
            JSON jsonInfo = jsonResult.ToJSON("VALUE");
            getPartnerModel().Nickname = jsonInfo.ToString("name");
            JSON jsonAvatar = jsonInfo.ToJSON("avatar");
            getPartnerModel().Avatar = jsonAvatar.ToString("url");
            callGetAvatarPartnerService(true, jsonResult);
        }
        else
        {
            callGetAvatarPartnerService(false, jsonResult);
        }
    }


    public void setIsSendDID(bool isSet)
    {
        if (isSet)
        {
            PlayerPrefs.SetInt("IS_DID", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IS_DID", 0);
        }
    }

    public bool getIsSendDID()
    {
        if (PlayerPrefs.GetInt("IS_DID") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setUsername(string username)
    {
        PlayerPrefs.SetString("USER_USERNAME", username);
    }

    public string getUsername()
    {
        return PlayerPrefs.GetString("USER_USERNAME");
    }

    public void setUserPass(string password)
    {
        PlayerPrefs.SetString("USER_PASSWORD", password);
    }

    public string getUserPass()
    {
        return PlayerPrefs.GetString("USER_PASSWORD");
    }

    public void setUserId(string id)
    {
        PlayerPrefs.SetString("USER_ID", id);
    }

    public string getUserId()
    {
        return PlayerPrefs.GetString("USER_ID");
    }

    public void setIsRememberLogin(bool isRemember, string idUser, string username, string pass)
    {
        if (isRemember)
        {
            PlayerPrefs.SetInt("IS_REMEMBER", 1);
            setUserPass(pass);
            setUsername(username);
            setUserId(idUser);
        }
        else
        {
            PlayerPrefs.SetInt("IS_REMEMBER", 0);
            setUserPass("");
            setUsername("");
            setUserId("");
        }
    }

    public bool getIsRememberLogin()
    {
        if (PlayerPrefs.GetInt("IS_REMEMBER") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    List<string> listNameBack = new List<string>();
    public void LoadLevel(string name)
    {
        listNameBack.Add(Application.loadedLevelName);
        Application.LoadLevel(name);
    }

    public void OnBackPress()
    {
        //listNameScreenBack.
        Application.LoadLevel(listNameBack[listNameBack.Count - 1]);
        listNameBack.RemoveAt(listNameBack.Count - 1);
    }

    public void Logout()
    {
        setIsRememberLogin(false, "", "", "");
        setUserModel(new UserModel());
        Application.LoadLevel("OptionLogin");
    }
}
