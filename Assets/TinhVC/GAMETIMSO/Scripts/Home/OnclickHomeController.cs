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

public class OnclickHomeController : MonoBehaviour {
    public GameObject friendListPrefab;
    public tk2dTextMesh txtNotice;
    // SMARTFOX
    public static SmartFox sfsConnection;
    void Awake()
    {
        txtNotice.gameObject.SetActive(false);
        if (SmartFoxConnection.IsInitialized)
        {
            initHandeler(false);
        }

    }
	// Use this for initialization
	void Start () {
        getInfoProfile();
        getAvatarUser();
        if (!SmartFoxConnection.IsInitialized)
        {
            initHandeler(true);
            SmartFoxConnection.Connect(sfsConnection);
        }
        else
        {
            if (!sfsConnection.IsConnected)
            {
                initHandeler(true);
                SmartFoxConnection.Connect(sfsConnection);
            }
        }
	}

    void initDefaltVariable() {
        VariableApplication.sUsername1 = "";
        VariableApplication.sUsername2 = "";
        VariableApplication.sUsername3 = "";
        VariableApplication.sUsername4 = "";
    }
	
	// Update is called once per frame
	void Update () {
	
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
        if (SmartFoxConnection.IsInitialized)
        {
            RequestGame(CheckEnoughMoney());
        }
        else
        {
            initHandeler(true);
            SmartFoxConnection.Connect(sfsConnection);
        }
    }

    bool isCreate = false;
    public void StartGamePrivate()
    {
        isCreate = true;
        if (SmartFoxConnection.IsInitialized)
        {
            RequestGame(CheckEnoughMoney());
        }
        else
        {
            initHandeler(true);
            SmartFoxConnection.Connect(sfsConnection);
        }
    }
    public bool CheckEnoughMoney()
    {
        return double.Parse(LamaControllib.getInstance().getMoneyUserModel().MoneyChip) > double.Parse((Utilities.GetMoneyQuick() * 1000).ToString());
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
        Debug.Log("tinhvc SFS: " + sfsConnection.IsConnected);
        // Register callback delegate
        sfsConnection.AddEventListener(SFSEvent.CONNECTION, OnConnected);
        sfsConnection.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfsConnection.AddEventListener(SFSEvent.LOGIN, OnLoginSuccess);
        sfsConnection.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfsConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    void RequestGame(bool isEnoughMoney)
    {
        if (isEnoughMoney)
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
        else
        {
            txtNotice.gameObject.SetActive(true);
            StartCoroutine(WaitForNotice());
        }
    }

    IEnumerator WaitForNotice()
    {
        yield return new WaitForSeconds(2.0f);
        txtNotice.gameObject.SetActive(false);
    }

    // HANDLE CONNECT EVENT

    void OnConnected(Sfs2X.Core.BaseEvent evt)
    {
        bool success = (bool)evt.Params[MySFSParams.PARAM_SUCCESS];

        if (success)
        {
            //string  userName = SystemInfo.deviceName + SystemInfo.deviceUniqueIdentifier;
            SmartFoxConnection.Login(sfsConnection, LamaControllib.getInstance().getUserModel().IdUser);
        }
        else
        {
            StartCoroutine(RequestServer());
        }
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        Debug.Log("tinhvc OnConnectionLost");
        UnregisterSFSSceneCallbacks();
        sfsConnection = null;
        SmartFoxConnection.Connection = sfsConnection;
    }

    void OnLoginSuccess(Sfs2X.Core.BaseEvent evt)
    {
        SmartFoxConnection.Connection = sfsConnection;
        //RequestGame();
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
        if(sfsConnection != null){
            sfsConnection.RemoveAllEventListeners();
        }
        
    }

    void OnDestroy()
    {
       UnregisterSFSSceneCallbacks();
    }

    IEnumerator RequestServer()
    {
       // Debug.Log("tinhvc load url server");
        WWW www = new WWW("http://google.com");
        yield return www;
    }
    
    GameObject cloneFriendList;
    public void OnClickFriendList()
    {
        cloneFriendList = Instantiate(friendListPrefab, transform.position, transform.rotation) as GameObject;
        cloneFriendList.transform.parent = transform;
        cloneFriendList.GetComponent<Camera>().depth = 3.0f;
    }
    public void CloseFriendList()
    {
        //enableButton(true);
        GameObject.Destroy(cloneFriendList);
        //showFriendList(false);
    }


    void OnClickDeposit()
    {
        LamaControllib.getInstance().LoadLevel("Deposit");
    }

    void onClickBack()
    {
        Application.LoadLevel("OptionScreen");
    }

    public GameObject tvMoneyChip;
    public GameObject tvLevel;
    public tk2dUIScrollbar progressBarLevel;
    public GameObject tvPersentLevel;
    void getInfoProfile()
    {
        LamaControllib.getInstance().getAllMoneyUserService(LamaControllib.getInstance().getUserModel().IdUser, getMoneyUserCallback, this);
        LamaControllib.getInstance().getXPModelService(LamaControllib.getInstance().getUserModel().IdUser, getXPUserCallback, this);
    }

    void getMoneyUserCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            tvMoneyChip.GetComponent<tk2dTextMesh>().text = "$ " + Utilities.getVNCurrency(int.Parse(LamaControllib.getInstance().getMoneyUserModel().MoneyChip)) + " Đ";
            //tvMoneyXen.GetComponent<tk2dTextMesh>().text = "$ " + Utilities.getVNCurrency(int.Parse(LamaControllib.getInstance().getMoneyUserModel().MoneyXen)) + " Đ";
        }
    }

    void getXPUserCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            tvLevel.GetComponent<tk2dTextMesh>().text = "Lv: " + LamaControllib.getInstance().getXPModel().Level;
            progressBarLevel.Value = float.Parse(LamaControllib.getInstance().getXPModel().Persent);
            tvPersentLevel.GetComponent<tk2dTextMesh>().text = float.Parse(LamaControllib.getInstance().getXPModel().Persent) * 100 + "%";
        }
    }
    public GameObject pictureObj;
    void getAvatarUser()
    {
        StartCoroutine(Utilities.setAvatarUser(pictureObj, LamaControllib.getInstance().getUserModel().Avatar, 1f));
        // LamaControllib.getInstance().getAvatarService(LamaControllib.getInstance().getUserModel().IdUser, getAvatarCallback, this);
    }

    void getAvatarCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            StartCoroutine(Utilities.setAvatarUser(pictureObj, LamaControllib.getInstance().getUserModel().Avatar, 1f));
        }
    }
}
