using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

public class Utilities {

    public static int[] reshuffle(int[] texts)
    {
        for (int t = 0; t < texts.Length; t++)
        {
            int tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
        return texts;
    }

    public static IEnumerator AnimationMove(GameObject gameObject, float x, float y, float time, eventMovetoListener eventMovetoCallback)
    {
        Hashtable args1 = new Hashtable();
        args1.Add("position", new Vector3(x, y, 0));
        args1.Add("time", time);
        iTween.MoveTo(gameObject, args1);

        yield return new WaitForSeconds(2.5f);
        eventMovetoCallback(gameObject);
    }

    public delegate void eventMovetoListener(GameObject gameObject);

    public static void setMoney1vs1(int money) {
        PlayerPrefs.SetInt(Constance.MONEY_1VS1, money);
    }

    public static int getMoney1vs1() {
        return PlayerPrefs.GetInt(Constance.MONEY_1VS1);
       
    }

    public static void setMoney2vs2(int money)
    {
        PlayerPrefs.SetInt(Constance.MONEY_2VS2, money);
    }

    public static int getMoney2vs2()
    {
        return PlayerPrefs.GetInt(Constance.MONEY_2VS2);
    }


    public static void setTypeRoom1(int roomFast)
    {
        PlayerPrefs.SetInt(Constance.TYPE_ROOM_1VS1, roomFast);
        Debug.Log("tinhvc type: " + roomFast);
    }

    public static int getTypeRoom1()
    {
        Debug.Log("tinhvc type: " + PlayerPrefs.GetInt(Constance.TYPE_ROOM_1VS1));
        return PlayerPrefs.GetInt(Constance.TYPE_ROOM_1VS1);
    }

    public static void setTypeRoom2(int roomFast)
    {
        PlayerPrefs.SetInt(Constance.TYPE_ROOM_2VS2, roomFast);
        Debug.Log("tinhvc type: "+roomFast);
    }

    public static int getTypeRoom2()
    {
        return PlayerPrefs.GetInt(Constance.TYPE_ROOM_2VS2);
    }

    // TLMN
    public static void SetMoneyQuick(int money)
    {
        PlayerPrefs.SetInt(Constance.MONEY_QUICK, money);
    }

    public static int GetMoneyQuick()
    {
        return PlayerPrefs.GetInt(Constance.MONEY_QUICK);

    }

    public static void SetMoneyFriend(int money)
    {
        PlayerPrefs.SetInt(Constance.MONEY_FRIEND, money);
    }

    public static int GetMoneyFriend()
    {
        return PlayerPrefs.GetInt(Constance.MONEY_FRIEND);
    }


    public static void SetTypeRoom(int roomFast)
    {
        PlayerPrefs.SetInt(Constance.TYPE_ROOM, roomFast);
        Debug.Log("tinhvc type: " + roomFast);
    }

    public static int GetTypeRoom()
    {
        Debug.Log("tinhvc type: " + PlayerPrefs.GetInt(Constance.TYPE_ROOM));
        return PlayerPrefs.GetInt(Constance.TYPE_ROOM);
    }

    public static void SetSpeedRoom(int roomFast)
    {
        PlayerPrefs.SetInt(Constance.SPEED_ROOM, roomFast);
        Debug.Log("tinhvc type: " + roomFast);
    }

    public static int GetSpeedRoom()
    {
        return PlayerPrefs.GetInt(Constance.SPEED_ROOM);
    }

    public static string ConvertMoney(long score)
    {
        if (score >= 100000000)
        {
            return (score / 1000000D).ToString("0.##M");
        }
        if (score >= 1000000)
        {
            return (score / 1000000D).ToString("0.##M");
        }
        if (score >= 100000)
        {
            return (score / 1000D).ToString("0.##K");
        }
        if (score >= 10000)
        {
            return (score / 1000D).ToString("0.##K");
        }
        if (score >= 1000)
        {
            return (score / 1000D).ToString("0.##K");
        }
        return score.ToString();

    }

    public static string getVNCurrency(long money)
    {
        decimal amount = money;
        string result = amount.ToString("#,##0");

        return result;
    }


    public static bool IsValidEmail(string strIn)
    {
        bool isEmail = Regex.IsMatch(strIn, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        return isEmail;
    }

    public static string GetUniqueIdentifier()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    public static string GetDeviceName()
    {
        return SystemInfo.deviceName;
    }
    public static string GetDeviceModel()
    {
        return SystemInfo.deviceModel;
    }

    public static IEnumerator setAvatarUser(GameObject picture, string url, float scale)
    {
        Texture2D mainImage;
        // string url = "http://media4.s-nbcnews.com/i/newscms/2014_01/95466/2d11014967-g-cvr-131230-alexis-shapiro-tease-720a_cd538a19fc8ab8eed51110db5891ae77.jpg";

        WWW www = new WWW(url);
        yield return www;
        mainImage = www.texture;

        float w = picture.GetComponent<SpriteRenderer>().sprite.texture.width;
        float h = picture.GetComponent<SpriteRenderer>().sprite.texture.height;

        float xScale = w / mainImage.width * 0.8f * scale;
        float yScale = h / mainImage.height * 0.8f * scale;


        Sprite anhgoc = Sprite.Create(mainImage, new Rect(0, 0, mainImage.width, mainImage.height), new Vector2(0.5f, 0.5f));

        //Sprite anhgoc = Sprite.Create(mainImage, new Rect(0, 0, mainImage.width, mainImage.height), new Vector2(0.5f, 0.5f), 300f);
        picture.GetComponent<SpriteRenderer>().sprite = anhgoc;
        picture.transform.localScale = new Vector3(xScale, yScale, 1);
    }
}


