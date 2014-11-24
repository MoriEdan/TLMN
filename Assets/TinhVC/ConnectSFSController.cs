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

public class ConnectSFSController : MonoBehaviour {

    // SMARTFOX
    public static SmartFox sfsConnection;


    //void Awake()
    //{

    //    if (SmartFoxConnection.IsInitialized)
    //    {
    //        sfsConnection = SmartFoxConnection.Connection;
    //        initHandeler(false);
    //        StartCoroutine(PingServer());
    //    }
    //}
    // Use this for initialization
    void Start()
    {
        try
        {
            UnregisterSFSSceneCallbacks();
            SmartFoxConnection.Connection.Disconnect();
        }
        catch
        {
            
        }
        initHandeler(true);
        SmartFoxConnection.Connect(sfsConnection);
    }

    void initDefaltVariable()
    {
        VariableApplication.sUsername1 = "";
        VariableApplication.sUsername2 = "";
        VariableApplication.sUsername3 = "";
        VariableApplication.sUsername4 = "";
    }

    void FixedUpdate()
    {
        if (null != sfsConnection)
        {
            sfsConnection.ProcessEvents();
        }
    }

    void initHandeler(bool isInitNew)
    {
        if (isInitNew)
        {
            sfsConnection = new SmartFox(true);
        }
        else
        {
            sfsConnection = SmartFoxConnection.Connection;
        }
        
        // Register callback delegate
        sfsConnection.AddEventListener(SFSEvent.CONNECTION, OnConnected);
        sfsConnection.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfsConnection.AddEventListener(SFSEvent.LOGIN, OnLoginSuccess);
        sfsConnection.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfsConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    // HANDLE CONNECT EVENT

    void OnConnected(Sfs2X.Core.BaseEvent evt)
    {
        bool success = (bool)evt.Params[MySFSParams.PARAM_SUCCESS];

        if (success)
        {
            SmartFoxConnection.Login(sfsConnection, LamaControllib.getInstance().getUserModel().IdUser);
        }
        else
        {
           // StartCoroutine(RequestServer());
        }
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        UnregisterSFSSceneCallbacks();
        sfsConnection = null;
        SmartFoxConnection.Connection = sfsConnection;
    }

    void OnLoginSuccess(Sfs2X.Core.BaseEvent evt)
    {
        SmartFoxConnection.Connection = sfsConnection;
        StartCoroutine(PingServer());
        Debug.Log("tinhvc login success");
    }

    IEnumerator PingServer()
    {
        yield return new WaitForSeconds(5);
        if (sfsConnection != null && sfsConnection.IsConnected)
        {
            sfsConnection.Send(new ExtensionRequest(SFCommands.USER_PING_SERVER, new SFSObject()));
        }
        StartCoroutine(PingServer());
    }

    void OnLoginError(Sfs2X.Core.BaseEvent evt)
    {
        //StartCoroutine(RequestServer());
    }


    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;
    }

    private void UnregisterSFSSceneCallbacks()
    {
        if (sfsConnection != null)
        {
            sfsConnection.RemoveAllEventListeners();
        }

    }

    void OnDestroy()
    {
        UnregisterSFSSceneCallbacks();
    }
}
