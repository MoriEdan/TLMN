//using UnityEngine;
//using System.Collections;

//public class ServiceFramwokDemo : MonoBehaviour
//{
//    string key = "7d5cb12c25767e7b275048034235b10b";
//    string number = "1";
//    // Use this for initialization
//    void Start () {
//        getXPModelService("11");
//        sendFeedbackService("11", "User gửi feedback");
//        sendNumberCardService("11", "023487294793742374");
//        addFriendService("2", "8");
//        getListFriendService("2");
//        sendReportService("2","11");
//    }
	
//    // Update is called once per frame
//    void Update () {
	
//    }

//    public delegate void jsonCallback(JSON jsonResult);

//    /**
//     *GET XP MODEL SERVICE 
//     */
//    void getXPModelService(string idUser) {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;
//        jsonSend["game_id"] = 3;

//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_XP_URL, jsonSend.serialized, key, number, idUser, 0, getXPCallback));
//    }

//    void getXPCallback(JSON jsonResult)
//    {
//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            XPModel xpModel = new XPModel();
//            JSON jsonValue = jsonResult.ToJSON("VALUE");

//            xpModel.Point = jsonValue.ToString("point");
//            xpModel.Level = jsonValue.ToString("level");
//            xpModel.Type = jsonValue.ToString("type");
//            xpModel.Medal = jsonValue.ToString("medal");

//            LamaControllib.getInstance().setXPModel(xpModel);

//            Debug.Log("tinhvc: "+LamaControllib.getInstance().getXPModel().Medal);
//        }
//    }

//    /**
//     *SEND FEEDBACK SERVICE 
//     */
//    void sendFeedbackService(string idUser, string content) {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;
//        jsonSend["content"] = content;

//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_FEEDBACK_URL, jsonSend.serialized, key, number, idUser, 0, sendFeedbackCallback));
//    }

//    void sendFeedbackCallback(JSON jsonResult)
//    {
//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            string value = jsonResult.ToString("VALUE");
//           // Debug.Log("tinhvc: " + value);
//        }
//    }

//    /**
//     *SEND NUMBER CARD SERVICE 
//     */
//    void sendNumberCardService(string idUser, string numberCard)
//    {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;
//        jsonSend["card_type"] = "viettel";
//        jsonSend["pin"] = "testpin";
//        jsonSend["serial"] = numberCard;
//        jsonSend["invite_id"] = 1;

//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_NUMBER_CARD_URL, jsonSend.serialized, key, number, idUser, 0, sendNumberCardCallback));
//    }

//    void sendNumberCardCallback(JSON jsonResult)
//    {
//            JSON jsonValue = jsonResult.ToJSON("VALUE");
//            string mess = jsonValue.ToString("message");
//            string amount = jsonValue.ToString("amount");

//            NumberCardModel numberCardModel = new NumberCardModel();
//            numberCardModel.Message = mess;
//            numberCardModel.Amount = amount;

//            LamaControllib.getInstance().setNumberCardModel(numberCardModel);
//            Debug.Log("tinhvc: " +  LamaControllib.getInstance().getNumberCardModel().Message);
//    }

//    /**
//     *ADD FRIEND SERVICE 
//     */
//    void addFriendService(string idUser, string idFriend)
//    {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;
//        jsonSend["friend_id"] = idFriend;
        
//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.ADD_FRIEND_URL, jsonSend.serialized, key, number, idUser, 0, addFriendCallback));
//    }

//    void addFriendCallback(JSON jsonResult)
//    {

//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            Debug.Log("tinhvc add friend success");
            
//        }
//        else {
//            Debug.Log("tinhvc: " + jsonResult.ToString("VALUE"));
//        }
//    }

//    /**
//     *GET LIST FRIEND SERVICE 
//     */
//    void getListFriendService(string idUser)
//    {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;

//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_FRIEND_URL, jsonSend.serialized, key, number, idUser, 0, getListFriendCallback));
//    }

//    void getListFriendCallback(JSON jsonResult)
//    {

//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            Debug.Log("tinhvc get list friend success ");
//            JSON[] listFriendJsonArray = jsonResult.ToArray<JSON>("VALUE");

//            for (int i = 0; i < listFriendJsonArray.Length; i++)
//            {
//                FriendModel friendModel = new FriendModel();
//                // Debug.Log("tinhvc: " + myObjArray[i].ToString("Amount"));
//                friendModel.IdFriend = listFriendJsonArray[i].ToString("id_friend");
//                friendModel.IdFriend = listFriendJsonArray[i].ToString("name_friend");
//                LamaControllib.getInstance().getListFriend().Add(friendModel);
//            }
//        }
//        else
//        {
//            Debug.Log("tinhvc get list friend null ");
//        }
//    }

//    /**
//     *GET LIST CARD SERVICE 
//     */
//    void getListCardService(string idUser)
//    {
//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_CARD_URL, new JSON().serialized, key, number, idUser, 0, getListCardCallback));
//    }

//    void getListCardCallback(JSON jsonResult)
//    {

//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            Debug.Log("tinhvc get list card success ");
//            JSON[] listCardJsonArray = jsonResult.ToArray<JSON>("VALUE");

//            for (int i = 0; i < listCardJsonArray.Length; i++)
//            {
//                CardModel cardModel = new CardModel();
//                // Debug.Log("tinhvc: " + myObjArray[i].ToString("Amount"));
//                cardModel.Card = listCardJsonArray[i].ToString("card");
//                cardModel.Xen = listCardJsonArray[i].ToString("xen");
//                LamaControllib.getInstance().getListCard().Add(cardModel);
//            }
//        }
//        else
//        {
//            Debug.Log("tinhvc get list card null ");
//        }
//    }

//    /**
//     *GET LIST AVATAR SERVICE 
//     */
//    void getListAvatarService(string idUser)
//    {
//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_LIST_AVATAR_URL, new JSON().serialized, key, number, idUser, 0, getListAvatarCallback));
//    }

//    void getListAvatarCallback(JSON jsonResult)
//    {

//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            Debug.Log("tinhvc get list avatar success ");
//            JSON[] listAvatarJsonArray = jsonResult.ToArray<JSON>("VALUE");

//            for (int i = 0; i < listAvatarJsonArray.Length; i++)
//            {
//                AvatarModel avatarModel = new AvatarModel();
//                // Debug.Log("tinhvc: " + myObjArray[i].ToString("Amount"));
//                avatarModel.Price = listAvatarJsonArray[i].ToString("price");
//                avatarModel.Id = listAvatarJsonArray[i].ToString("id");
//                avatarModel.Url = listAvatarJsonArray[i].ToString("url");
//                LamaControllib.getInstance().getListAvatar().Add(avatarModel);
//            }
//        }
//        else
//        {
//            Debug.Log("tinhvc get list avatar null ");
//        }
//    }

//    /**
//    *SEND REPORT USER SERVICE 
//    */
//    void sendReportService(string idUser, string idUserBad)
//    {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;
//        jsonSend["bad_id"] = idUserBad;

//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.SEND_REPORT_USER_URL, jsonSend.serialized, key, number, idUser, 0, sendReportCallback));
//    }

//    void sendReportCallback(JSON jsonResult)
//    {

//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            Debug.Log("tinhvc send report success ");
//        }
//        else
//        {
//            Debug.Log("tinhvc send report fail");
//        }
//    }

//    /**
//    *GET ALL MONEY USER SERVICE 
//    */
//    void getAllMoneyUserService(string idUser, string idUserBad)
//    {
//        JSON jsonSend = new JSON();

//        jsonSend["user_id"] = idUser;

//        JsonMethod jsonMethod = new JsonMethod();
//        StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_ALL_MONEY_USER_URL, jsonSend.serialized, key, number, idUser, 0, getAllMoneyUserCallback));
//    }

//    void getAllMoneyUserCallback(JSON jsonResult)
//    {

//        if (jsonResult.ToBoolean("SUCCESS"))
//        {
//            JSON[] listMoneyJsonArray = jsonResult.ToArray<JSON>("VALUE");

//            if (listMoneyJsonArray.Length>=4)
//            {
//                MoneyUserModel moneyUserModel = new MoneyUserModel();
//                // Debug.Log("tinhvc: " + myObjArray[i].ToString("Amount"));
//                moneyUserModel.MoneyOnePay = listMoneyJsonArray[0].ToString("money");
//                moneyUserModel.MoneyXen = listMoneyJsonArray[1].ToString("money");
//                moneyUserModel.MoneyChip = listMoneyJsonArray[2].ToString("money");
//                moneyUserModel.MoneyDiamond = listMoneyJsonArray[3].ToString("money");
//                LamaControllib.getInstance().setMoneyUserModel(moneyUserModel);
//            }
//            else
//            {

//            }
//        }
//        else
//        {
//            Debug.Log("tinhvc send report fail");
//        }
//    }
    
//}
