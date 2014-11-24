using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

public class JsonMethod
{

    public IEnumerator getJSONFromUrl(string url, string param, string key, string number, string idUser, int position, jsonCallback callback)
    {
        var form = new WWWForm();
        var headers = form.headers;
        var rawData = form.data;
        headers["ContentType"] = "application/x-www-form-urlencoded";
        headers["key"] = key;
        headers["number"] = number;
        string postData = "par=" + param;
        Debug.Log("tinhvc param: " + postData);
        Debug.Log("tinhvc url: " + url);
        rawData = Encoding.UTF8.GetBytes(postData);
        var www = new WWW(url, rawData, headers);
        yield return www;

        string respone;
        if (www.error != null)
        {
            respone = "null";
        }
        else
        {
            respone = www.text;
        }
        JSON jsonResult = new JSON();
        jsonResult.serialized = respone;
        Debug.Log("respone");
        jsonResult["id_user"] = idUser;
        jsonResult["position"] = position;
        callback(jsonResult);

    }

    public delegate void jsonCallback(JSON jsonResult);


    public IEnumerator setAvatarUser(GameObject picture, string url, float scale)
    {
        Texture2D mainImage;

        WWW www = new WWW(url);
        yield return www;
        mainImage = www.texture;
        float w = 0;
        float h = 0;
        if (null != picture)
        {
            w = picture.GetComponent<SpriteRenderer>().sprite.texture.width;
            h = picture.GetComponent<SpriteRenderer>().sprite.texture.height;
            float xScale = w / mainImage.width * 0.8f * scale;
            float yScale = h / mainImage.height * 0.8f * scale;

            Sprite anhgoc = Sprite.Create(mainImage, new Rect(0, 0, mainImage.width, mainImage.height), new Vector2(0.5f, 0.5f));
            //Sprite anhgoc = Sprite.Create(mainImage, new Rect(0, 0, mainImage.width, mainImage.height), new Vector2(0.5f, 0.5f), 300f);
            if (null != picture)
            {
                picture.GetComponent<SpriteRenderer>().sprite = anhgoc;
                picture.transform.localScale = new Vector3(xScale, yScale, 1);
            }
        }
    }
}
