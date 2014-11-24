using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class FriendManager : MonoBehaviour {

    public GameObject friendPrefab;
    public tk2dUIScrollableArea scrollArea;
    List<FriendModel> listFriend;
    List<GameObject> listFriendClone = new List<GameObject>();
    private SmartFox sfConnectObj;
    List<GameObject> friendObjects;
    void Awake()
    {
        if (SmartFoxConnection.IsInitialized)
        {
            sfConnectObj = SmartFoxConnection.Connection;
        }
        else
        {
            Application.LoadLevel("TLMN_Menu");
        }
        // Register callback delegate
        sfConnectObj.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfConnectObj.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }
	// Use this for initialization
	void Start () {
        listFriend = new List<FriendModel>();
        friendObjects = new List<GameObject>();
        LamaControllib.getInstance().getListFriendService(LamaControllib.getInstance().getUserModel().IdUser, GetListFriendCallBack, this);
        
	}

    void GetListFriendCallBack(bool success, JSON json)
    {
       
        listFriend = LamaControllib.getInstance().getListFriend();

        //for (int j = 0; j < 20; j++)
        //{
        //    listFriend.Add(listFriend[0]);
        //}

        setListFriendView(listFriend);
        SmartFoxConnection.SendGetFriendListRequest();
    }

    void setListFriendView(List<FriendModel> _listFriend)
    {
        for (int j = 0; j < listFriendClone.Count; j++)
        {
            //_listFriend.Add(_listFriend[0]);
            GameObject.Destroy(listFriendClone[j]);
        }

        for (int i = 0; i < _listFriend.Count; i++)
        {
            GameObject friendInstance = Instantiate(friendPrefab, new Vector3(0, i * 1, 0), Quaternion.identity) as GameObject;
            friendInstance.transform.FindChild("name").GetComponent<tk2dTextMesh>().text = _listFriend[i].NameFriend;
            friendInstance.transform.parent = scrollArea.transform.FindChild("Content").transform;
            friendInstance.transform.localPosition = new Vector3(0.2f, -(i * 0.2f), 0);
            friendInstance.transform.FindChild("check").gameObject.SetActive(false);
            friendObjects.Add(friendInstance);
            friendInstance.name = "" + _listFriend[i].IdFriend;
            JsonMethod jsonMethod = new JsonMethod();
            listFriendClone.Add(friendInstance);
            StartCoroutine(jsonMethod.setAvatarUser(friendInstance.transform.FindChild("IconFriend").gameObject, _listFriend[i].AvatarFriend, 1.5f));
        }
        scrollArea.ContentLength = (((float)_listFriend.Count / 4) * 0.8f);
    }

    List<FriendModel> listFriendSearch;
    void searchFriend(string keySearch)
    {
        if(keySearch.Equals("")){
            listFriendSearch = listFriend;
        }
        else
        {
            listFriendSearch = new List<FriendModel>();
            for (int i = 0; i < listFriend.Count; i++)
            {
                if (listFriend[i].NameFriend.Contains(keySearch))
                {
                    listFriendSearch.Add(listFriend[i]);
                }
            }
        }
        
        setListFriendView(listFriendSearch);
    }

    public tk2dUITextInput inputSearch;
    void ClickSearch()
    {
        //if (!inputSearch.Text.Equals(""))
        //{
            searchFriend(inputSearch.Text);
        //}
    }
   
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        sfConnectObj.ProcessEvents();
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        StopAllCoroutines();
        //sfStatus.GetComponent<tk2dTextMesh>().text = "Connection was lost, reason: " + (string)evt.Params[MySFSParams.PARAM_REASON];
    }

    void FindOnlineFriend(int[] userNames)
    {
        for(int i = 0; i < listFriend.Count; i++)
        {
            for(int j = 0 ;  j < userNames.Length; j ++)
            {
                if(listFriend[i].IdFriend.Equals(userNames[j].ToString()))
                {
                    friendObjects[i].transform.FindChild("check").gameObject.SetActive(true);
                }
            }
        }
    }

    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        //ISFSObject mParams = new SFSObject();
        ISFSObject dataObj = evt.Params["params"] as SFSObject;
        switch (cmd)
        {
            case "Success":
                {
                    int size = dataObj.GetInt("Size");
                    int[] userNames = new int[size];
                    userNames = dataObj.GetIntArray("UserNames");
                    FindOnlineFriend(userNames);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
