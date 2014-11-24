using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Sfs2X.Logging;
//using ANMiniJSON;

public class ServerDefine1 : MonoBehaviour
{

    /// <summary>
    /// Server configuration
    /// </summary>
    //public const string SERVER_NAME = "103.20.148.119";
    //public const string SERVER_NAME = "localhost";
    public static string SERVER_NAME;// = "go.cent.vn";
    public static int SERVER_PORT;// = 9933;
    //
    public static string ZONE;// = "DauVoi";
    //public const string ZONE = "BasicExamples";

    public string url = "http://cent.vn/appnoti/dauvoi_info.txt";
    private IDictionary results;

    /// <summary>
    /// Default is DEBUG to get more information
    /// 
    /// </summary>
    public static LogLevel logLevel = LogLevel.DEBUG;

    void Awake()
    {
        //StartCoroutine(RequestServer());
        SERVER_NAME = "localhost";
        SERVER_PORT = 9933;
        ZONE = "TLMN";
    }

    IEnumerator RequestServer()
    {
        WWW www = new WWW(url);
        yield return www;
        //results = (IDictionary)Json.Deserialize(www.text);

        SERVER_NAME = results["smartfox_url1"].ToString();
        SERVER_PORT = 9933;
        ZONE = results["name_zone"].ToString(); 
    }




}