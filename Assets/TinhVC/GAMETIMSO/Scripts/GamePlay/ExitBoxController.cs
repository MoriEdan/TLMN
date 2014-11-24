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

public class ExitBoxController : MonoBehaviour
{

    // SMARTFOX
    public static SmartFox sfsConnection;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (null != sfsConnection)
        {
            sfsConnection.ProcessEvents();
        }
    }

    public void StartRequestGame()
    {
        isCreate = false;
        try
        {
            UnregisterSFSSceneCallbacks();
            SmartFoxConnection.Connection.Disconnect();
        }
        catch
        {
        }
        initHandeler();
        SmartFoxConnection.Connect(sfsConnection);
    }

    bool isCreate = false;

    void initHandeler()
    {

        sfsConnection = new SmartFox(true);

        // Register callback delegate
        sfsConnection.AddEventListener(SFSEvent.CONNECTION, OnConnected);
        sfsConnection.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfsConnection.AddEventListener(SFSEvent.LOGIN, OnLoginSuccess);
        sfsConnection.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfsConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    void RequestGame()
    {
        SFSObject mParamsRoom = new SFSObject();
        mParamsRoom.PutByte(SFCommands.KIND_GAME_MESS, 1);
        //mParamsRoom.PutInt(SFCommands.KIND_GAME_FIGHT, VariableApplication.iPlayerCount);
        if (VariableApplication.isQuick)
        {
            mParamsRoom.PutInt(SFCommands.MONEY_GAME, Utilities.GetMoneyQuick());
        }
        else
        {
            mParamsRoom.PutInt(SFCommands.MONEY_GAME, Utilities.GetMoneyFriend());
        }
        if (Utilities.GetTypeRoom() == 1)
        {
            mParamsRoom.PutBool(SFCommands.PRIVATE_GAME, true);
        }
        else
        {
            mParamsRoom.PutBool(SFCommands.PRIVATE_GAME, false);
        }
        if (Utilities.GetSpeedRoom() == 1)
        {
            mParamsRoom.PutBool(SFCommands.SPEED_GAME, true);
        }
        else
        {
            mParamsRoom.PutBool(SFCommands.SPEED_GAME, false);
        }

        Debug.Log("tinhvc private: " + mParamsRoom.GetBool(SFCommands.PRIVATE_GAME));

        mParamsRoom.PutBool(SFCommands.IS_CREATE_ROOM_NEW, isCreate);

        SmartFoxConnection.SendReadyMsg(mParamsRoom, SFCommands.REQUEST_QUICK_GAME);
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
            StartCoroutine(RequestServer());
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
        RequestGame();
    }

    void OnLoginError(Sfs2X.Core.BaseEvent evt)
    {
        StartCoroutine(RequestServer());
    }


    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;

        switch (cmd)
        {
            case "ReadyStart":
                {
                    SmartFoxConnection.PersistentData = dataObject;
                    UnregisterSFSSceneCallbacks();
                    Application.LoadLevel("TLMN_Play");
                    break;
                }
            // wait for another player | player create room
            case SFCommands.CM_CREATE_ROOM:
                bool isCreateSuccess = dataObject.GetBool(SFCommands.IS_CREATE_ROOM_SUCCESS);
                if (isCreateSuccess)
                {
                    VariableApplication.iIdUserSFS = dataObject.GetInt(SFCommands.ID_USER_KEY);
                    SmartFoxConnection.PersistentData = dataObject;
                    UnregisterSFSSceneCallbacks();
                    Application.LoadLevel("PlayGame");
                }
                else
                {
                    try
                    {
                        if (sfsConnection.IsConnected)
                        {
                            SmartFoxConnection.Connection.Disconnect();
                        }
                    }
                    catch
                    {

                    }
                }
                break;

            case SFCommands.CM_JOIN_ROOM:
                bool isJoinSuccess = dataObject.GetBool(SFCommands.IS_JOIN_ROOM_SUCCESS);
                if (isJoinSuccess)
                {
                    VariableApplication.iIdUserSFS = dataObject.GetInt(SFCommands.ID_USER_KEY);
                    SmartFoxConnection.PersistentData = dataObject;
                    UnregisterSFSSceneCallbacks();
                    Application.LoadLevel("PlayGame");
                }
                else
                {
                    try
                    {
                        if (sfsConnection.IsConnected)
                        {
                            SmartFoxConnection.Connection.Disconnect();
                        }
                    }
                    catch
                    {

                    }
                }
                break;
        }
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

    IEnumerator RequestServer()
    {
        WWW www = new WWW("http://google.com");
        yield return www;
    }
}
