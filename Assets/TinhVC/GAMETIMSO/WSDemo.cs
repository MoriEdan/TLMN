using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

public class WSDemo : MonoBehaviour
{


    void Start()
    {
        string url = "http://103.20.148.244:8180/Cent_Test/game_money/getAllMoneyType/";
        string key = "7d5cb12c25767e7b275048034235b10b";
        string number = "1";
        //string param = "{\"user_id\":11,\"money_id\"=3}";


        StartCoroutine(getJSONFromUrl(url, "", key, number));


        DemoPutJson();

        //////////
       
    }
 
    IEnumerator getJSONFromUrl(string url, string param, string key, string number)
    {
        
        var form = new WWWForm();
        var headers = form.headers;
        var rawData = form.data;
        headers["ContentType"] = "application/x-www-form-urlencoded";
        headers["key"] = key;
        headers["number"] = number;
        string postData = "par=" + param;
        rawData = Encoding.UTF8.GetBytes(postData);
        var www = new WWW(url, rawData, headers);
        yield return www;

        string respone;
        if(www.error!=null){
            respone = "null";
        }else{
            respone = www.text;
        }
      //  Debug.Log("tinhvc respone: "+respone);

        DemoJsonParser(respone);
        
    }

    void DemoPutJson() {
        JSON js = new JSON();
        
        js["myString"] = "This is a string ";
        js["myStringArray"] = new string[] { "string value 1", "string value 2", "string value 3" };
        // int
        js["myInt"] = 1234;
        js["myIntArray"] = new int[] { 1, 2, 3 };
        // float
        js["myFloat"] = 0.5f;
        js["myFloatArray"] = new float[] { 1.5f, 2.5f, 3.5f };
        // boolean
        js["myBoolean"] = true;
        js["myBooleanArray"] = new bool[] { true, false, true };

      //  Debug.Log("tinhvc :" + js.serialized);
        sJson = js.serialized;
    }

    void DemoJsonParser(string json) {
        JSON jss = new JSON();
        // Parse JSON string
        jss.serialized = json;

       // Debug.Log("tinhvc: " + jss.ToString("SUCCESS"));

        JSON[] myObjArray = jss.ToArray<JSON>("VALUE");

        for (int i = 0; i < myObjArray.Length; i++)
        {
            MyMoney myMoney = new MyMoney();
           // Debug.Log("tinhvc: " + myObjArray[i].ToString("Amount"));
            sJson = sJson + myObjArray[i].ToString("Amount");
        }
    }

    public class MyMoney
    {
        public string Amount { get; set; }

        public int ID { get; set; }

    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 800, 200), sJson);
    }
    string sJson = "";
}
