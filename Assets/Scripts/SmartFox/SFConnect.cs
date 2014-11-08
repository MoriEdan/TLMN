using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;

public class SFConnect : MonoBehaviour {

    // SMARTFOX
    private SmartFox sfConnectObj;
    private string userName;
    private string password = "";
    private string loginError;
    //private string sfStatus;
    private SmartFoxState sfState = SmartFoxState.Offline;

    // Text in game
    private GameObject sfStatus;
    public Sprite connecting;
    public Sprite error;
    public Sprite noInternet;

    public string url = "http://sfs.cent.vn/z.php";
    
    void Awake()
    {
        //userName = SystemInfo.deviceName + SystemInfo.deviceUniqueIdentifier;
        userName = SystemInfo.deviceName + Random.Range(0, 100);
        //Application.runInBackground = true;
        CreateConnectObject();
    }

    // Use this for initialization
    void Start()
    {
        sfStatus = GameObject.Find("TxtConnectStatus") as GameObject;
        sfStatus.GetComponent<tk2dTextMesh>().text = "Connecting...";


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        sfConnectObj.ProcessEvents();
    }

    void OnMouseUp() {
        //text.GetComponent<SpriteRenderer>().sprite = connecting;
        sfStatus.GetComponent<tk2dTextMesh>().text = "connecting";
        InvokeRepeating("Connect", 1, 5);
    }

    void CreateConnectObject()
    {
        // Create connect object
        if (SmartFoxConnection.IsInitialized)
        {
            sfConnectObj = SmartFoxConnection.Connection;
        }
        else
        {
            sfConnectObj = new SmartFox(true);
        }
        //Debug.LogWarning("API Version: " + sfConnectObj.Version);

        // Register callback delegate
        sfConnectObj.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfConnectObj.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfConnectObj.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfConnectObj.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfConnectObj.AddEventListener(SFSEvent.LOGOUT, OnLogout);
        sfConnectObj.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnExitRoom);
        sfConnectObj.AddEventListener(SFSEvent.ROOM_JOIN, OnEnterRoom);
        sfConnectObj.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    IEnumerator ConnectAndLogin() {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!sfConnectObj.IsConnected)
                sfConnectObj.Connect(ServerDefine.SERVER_NAME, ServerDefine.SERVER_PORT);

            yield return new WaitForSeconds(1);

            Login();
        }
        else {
            sfStatus.GetComponent<tk2dTextMesh>().text = "No Internet access!";
            //text.GetComponent<SpriteRenderer>().sprite = noInternet;
        }
    }

    void Connect()
    {
        if (sfConnectObj.MySelf != null) {
            CancelInvoke();
            sfStatus.SetActive(false);
            //text.SetActive(false);
            sfState = SmartFoxState.Loged_in;
            return;
        }
        //Debug.Log("Trying to connect...");
        sfStatus.SetActive(true);
        //text.SetActive(true);
        sfStatus.GetComponent<tk2dTextMesh>().text = "Connecting...";
        //text.GetComponent<SpriteRenderer>().sprite = connecting;
        StartCoroutine(ConnectAndLogin());
    }

    void Login()
    {
        if (sfConnectObj.IsConnected && sfConnectObj.MySelf == null)
        {
            sfConnectObj.Send(new LoginRequest(userName, password, ServerDefine.ZONE));
        }
    }

    void RequestGame()
    {
        //Debug.Log("request game");
        if(sfState == SmartFoxState.Loged_in)
        {
            SFSObject param = new SFSObject();
            param.PutByte("GameKind", 1);
            SmartFoxConnection.SendReadyMsg(param);
        }
            
    }

    // HANDLE CONNECT EVENT

    void OnConnection(Sfs2X.Core.BaseEvent evt)
    {
        bool success = (bool)evt.Params[MySFSParams.PARAM_SUCCESS];
        if (success)
        {
            CancelInvoke();
            SmartFoxConnection.Connection = sfConnectObj;
            sfState = SmartFoxState.Connected;

        }
        else
        {
            //text.GetComponent<SpriteRenderer>().sprite = error;
            sfStatus.GetComponent<tk2dTextMesh>().text = "Error";
            //CallServerZ();
        }
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        sfState = SmartFoxState.Disconnected;
        InvokeRepeating("Connect", 1, 5);
    }

    void OnLogin(Sfs2X.Core.BaseEvent evt)
    {
        // In v1, userName = deviceName
        // in case username conflict, we random a name for user
        //userName = SystemInfo.deviceName;
        sfState = SmartFoxState.Loged_in;
      
        if (sfState == SmartFoxState.Loged_in)
        {
            RequestGame();
        }
        //text.SetActive(false);
        sfStatus.SetActive(false);
    }

    void OnLoginError(Sfs2X.Core.BaseEvent evt)
    {
        sfStatus.GetComponent<tk2dTextMesh>().text = "Error, Please Try Again...";
        //text.GetComponent<SpriteRenderer>().sprite = error;
        //CallServerZ();
    }


    void OnLogout(Sfs2X.Core.BaseEvent evt)
    {
        //Debug.Log("TV.Vinh User: user has loged out");
        sfState = SmartFoxState.Connected;
    }

    void OnExitRoom(Sfs2X.Core.BaseEvent evt)
    {
        //Debug.Log("exit room");
    }

    void OnEnterRoom(Sfs2X.Core.BaseEvent evt)
    {
        //Debug.Log("TV.Vinh User: " + sfConnectObj.MySelf.Name + " has joined room: " + sfConnectObj.LastJoinedRoom.Name);
    }

    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        //Debug.Log("TV.Vinh User: extension response arrived");
        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;

        switch (cmd)
        {
            // wait for another player | player create room
            case "ReadyStart":
                SmartFoxConnection.PersistentData = dataObject;
                UnregisterSFSSceneCallbacks();
                Application.LoadLevel("TLMN_Play");
                break;
        }
    }

    private void UnregisterSFSSceneCallbacks()
    {
        // This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
        sfConnectObj.RemoveAllEventListeners();
    }

    void OnDestroy()
    {
        UnregisterSFSSceneCallbacks();
    }

    void CallServerZ()
    {
        WWW www = new WWW(url);
        //yield return www;
    }
}
