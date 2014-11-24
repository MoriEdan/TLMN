using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class TLMNButtonController : MonoBehaviour {

    public SFSGameRoom gameManager;
    public GameObject myProfilePrefab;
    public GameObject depositPrefab;
    public GameObject friendListPrefab;
    public GameObject boxExit;
    public GameObject btnCloseBoxExit;
    public Deck2 deck;

    //ID
    public static string idUser = "-1";

	// Use this for initialization
	void Start () {
        //boxExit.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Btn Deal Card Click
    void DealCard()
    {
        
    }

    void Sort()
    {
        Hand hand = gameManager.players[gameManager.Order].QuickFindHand();
        if(hand!=null)
        {
            for (int i = 0; i < hand.CardCount(); i++)
            {
                hand.Cards[i].MouseUpHandler();
            }
        }
    }

    void SendHandToServer()
    {
        if (gameManager.players[gameManager.Order].State != PlayerState.Win)
        {
            Hand putHand = gameManager.players[gameManager.Order].GetMove();
            if (putHand != null)
            {
                SmartFoxConnection.SendHandToServer(putHand, gameManager.Order, false);
            }
        }
    }
    void SendSuperWinToServer()
    {
        Hand putHand = new Hand(gameManager.players[gameManager.Order].Deck.Cards);
        SmartFoxConnection.SendHandToServer(putHand, gameManager.Order, true);
    }

    void SendPassToServer()
    {
        if(!gameManager.players[gameManager.Order].IsNewRoundPlayer)
        {
            if (gameManager.players[gameManager.Order].State != PlayerState.Win)
            {
                if (gameManager.players[gameManager.Order].IsCurrentPlayer())
                {
                    SmartFoxConnection.SendPassToServer(gameManager.Order);
                }
            }
        }
    }

    void Quit()
    {
        SmartFoxConnection.Connection.Disconnect();
        Application.LoadLevel("OptionScreen");
    }

    void TestFriend()
    {
        Application.LoadLevel("Invite Friend");
    }

    void Avatar0Click()
    {
        if(SFSGameRoom.isStarted)
        {
            idUser = LamaControllib.getInstance().getUserModel().IdUser;
            Debug.Log("LALALA " + idUser);
            ShowProfile();
        }
    }

    void Avatar1Click()
    {
        if (SFSGameRoom.isStarted)
        {
            //idUser = VariableApplication.tlmnIdUser1;
            idUser = VariableApplication.tlmnIdUser[(Constants.MAX_NUMBER_PLAYER - SFSGameRoom.order + 1) % Constants.MAX_NUMBER_PLAYER];
            ShowProfile();
        }
    }

    void Avatar2Click()
    {
        if (SFSGameRoom.isStarted)
        {
            //idUser = VariableApplication.tlmnIdUser2;
            idUser = VariableApplication.tlmnIdUser[(Constants.MAX_NUMBER_PLAYER - SFSGameRoom.order + 2) % Constants.MAX_NUMBER_PLAYER];
            ShowProfile();
        }
    }

    void Avatar3Click()
    {
        if (SFSGameRoom.isStarted)
        {
            //idUser = VariableApplication.tlmnIdUser3;
            idUser = VariableApplication.tlmnIdUser[(Constants.MAX_NUMBER_PLAYER - SFSGameRoom.order + 3) % Constants.MAX_NUMBER_PLAYER];
            ShowProfile();
        }
    }

    GameObject cloneFriendList;
    GameObject cloneDeposit;
    GameObject cloneProfile;
    void ShowProfile()
    {
        cloneProfile = Instantiate(myProfilePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        cloneProfile.GetComponent<Camera>().depth = 3.0f;
        cloneProfile.transform.parent = transform;
    }

    public void OnClickDeposit()
    {
        cloneDeposit = Instantiate(depositPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        cloneDeposit.transform.parent = transform;
        cloneDeposit.GetComponent<Camera>().depth = 3.0f;
    }

    public void OnClickFriendList()
    {
        cloneFriendList = Instantiate(friendListPrefab, transform.position, transform.rotation) as GameObject;
        cloneFriendList.transform.parent = transform;
        cloneFriendList.GetComponent<Camera>().depth = 3.0f;
    }

    public void CloseProfile()
    {
        GameObject.Destroy(cloneProfile);
        //showProfile(false);
    }

    public void CloseDeposit()
    {
        //enableButton(true);
        GameObject.Destroy(cloneDeposit);
        //showDeposit(false);
    }

    public void CloseFriendList()
    {
        //enableButton(true);
        GameObject.Destroy(cloneFriendList);
        //showFriendList(false);
    }

    public void OnClickBack()
    {
        boxExit.SetActive(!boxExit.activeSelf);
    }

    public void QuitRoom()
    {
        try
        {
            UnregisterSFSSceneCallbacks();
            SmartFoxConnection.Connection.Disconnect();
        }
        catch
        {

        }
        Application.LoadLevel("OptionScreen");
    }

    public void StandupRoom()
    {
        SFSObject mParamsRequest = new SFSObject();
        SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.USER_GIVEUP_KEY);
        VariableApplication.isViewer = true;
        //OnClickCloseBoxExit();
    }

    public void CreateNewRoom()
    {
        //OnClickCloseBoxExit();
        try
        {
            UnregisterSFSSceneCallbacks();
            SmartFoxConnection.Connection.Disconnect();
        }
        catch
        {

        }
        Application.LoadLevel("CreateRoom");
    }

    public void SwitchRoom()
    {

    }

    private void UnregisterSFSSceneCallbacks()
    {
        // This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
        if (gameManager.SfConnectObj != null)
            gameManager.SfConnectObj.RemoveAllEventListeners();
    }

    void Put()
    {
        if(gameManager.RoundPlayer == gameManager.CurrentPlayer)
        { 
            gameManager.CurrentHand = new Hand(new List<Card>());
            //gameManager.NewRound(gameManager.CurrentPlayer, gameManager);            
        }
        //gameManager.PutCard(gameManager.FindCardOnHand());
        Hand putHand = gameManager.CurrentPlayer.GetMove();
        if (putHand != null)
        {
            gameManager.CurrentPlayer.PutCard(putHand);
            gameManager.RoundPlayer = gameManager.CurrentPlayer;
            gameManager.SetIdle(gameManager.CurrentPlayer);
            //gameManager.CheckWin(gameManager.CurrentPlayer);
            //gameManager.SetPlay();
        }
    }

    void Pass()
    {
        gameManager.SetPass(gameManager.CurrentPlayer);
        //gameManager.SetPlay();
    }

}
