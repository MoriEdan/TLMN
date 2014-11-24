using UnityEngine;
using System.Collections;

public class ApiEntity
{

    public static string BASE_URL = "http://103.20.148.244:8180/Cent_Managers/game_money/";

    public static string ADD_MONEY_URL = BASE_URL + "addMoneyForUser/";
    public static string GET_ALL_MONEY_TYPE_URL = BASE_URL + "getAllMoneyType/";
    public static string GET_ALL_MONEY_USER_URL = BASE_URL + "getAllMoneyOfUser/";
    public static string GET_GAME_MONEY_USER_URL = BASE_URL + "getGameMoneyOfUser/";
    public static string MONEY_EXCHANGE_URL = BASE_URL + "moneyExchange/";

    public static string GET_XP_URL = "http://103.20.148.244:8180/Cent_Managers/xp/getXp/";

    public static string SEND_FEEDBACK_URL = "http://103.20.148.244:8180/Cent_Managers/user/feedback/";

    public static string SEND_NUMBER_CARD_URL = "http://103.20.148.244:8180/Cent_Managers/game_money/onePayCard/";

    public static string ADD_FRIEND_URL = "http://103.20.148.244:8180/Cent_Managers/user/addFriend/";

    public static string GET_LIST_FRIEND_URL = "http://103.20.148.244:8180/Cent_Managers/user/getFriendList/";

    public static string GET_LIST_CARD_URL = "http://103.20.148.244:8180/Cent_Managers/item/getCardList/";

    public static string GET_LIST_SMS_URL = "http://103.20.148.244:8180/Cent_Managers/item/getSmsList/";

    public static string GET_LIST_AVATAR_URL = "http://103.20.148.244:8180/Cent_Managers/item/getAvatarList";

    public static string SEND_REPORT_USER_URL = "http://103.20.148.244:8180/Cent_Managers/user/reportUser";

    public static string REGISTER_USER_URL = "http://103.20.148.244:8180/Cent_Managers/user/register";

    public static string REGISTER_USER_GUEST_URL = "http://103.20.148.244:8180/Cent_Managers/user/registerGuest";

    public static string REGISTER_USER_FACEBOOK_URL = "http://103.20.148.244:8180/Cent_Managers/user/registerFb";

    public static string SEND_DEVICE_ID_URL = "http://103.20.148.244:8180/Cent_Managers/item/saveDeviceId";

    public static string GET_AVATAR_URL = "http://103.20.148.244:8180/Cent_Managers/item/getCurrentAvatarOfUser";
}
