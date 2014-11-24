using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
using System.Collections.Generic;

public class ReadyClickController : MonoBehaviour {

    private SmartFox sfConnection;
    private SFSObject receivedData;

    public GameObject btnReady1;
    public GameObject btnReady2;
    public GameObject btnReady3;
    public GameObject btnReady4;
    public GameObject btnReady5;
    public GameObject btnReady6;

    public GameObject player1;
    public GameObject player11;
    public GameObject player2;
    public GameObject player22;

    private GameObject[] listButtons = new GameObject[6];

    Dictionary<string, GameObject> mapUserLocarion = new Dictionary<string, GameObject>();

    void Awake()
    {

        if (SmartFoxConnection.IsInitialized)
        {
            sfConnection = SmartFoxConnection.Connection;
            initHandeler();
        }
        else
        {
            //Debug.Log("tinhvc Connection lost");
        }
    }

    private  List<PositionUserModel> listPositionModel = new List<PositionUserModel>();
    // Use this for initialization
    void Start()
    {
        if (!VariableApplication.isOffline)
        {
            Debug.Log("tinhvc start: " + listButtons.Length);
            //initHandeler();
            addListPosition();

            listButtons[0] = btnReady1;
            listButtons[1] = btnReady2;
            listButtons[2] = btnReady3;
            listButtons[3] = btnReady4;
            listButtons[4] = btnReady5;
            listButtons[5] = btnReady6;

            for (int i = 0; i < listButtons.Length; i++)
            {
                listButtons[i].GetComponent<UIButton>().enabled = true;
            }

            mapUserLocarion = mapUserLocarion = new Dictionary<string, GameObject>();
            getUserReady();
            StartCoroutine(PingServer());
        }
    }

    void addListPosition()
    {
        PositionUserModel positionModel1 = new PositionUserModel();
        positionModel1.setData(false, player1);
        PositionUserModel positionModel2 = new PositionUserModel();
        positionModel2.setData(false, player11);
        PositionUserModel positionModel3 = new PositionUserModel();
        positionModel3.setData(false, player2);
        PositionUserModel positionModel4 = new PositionUserModel();
        positionModel4.setData(false, player22);

        if(VariableApplication.iPlayerCount == 2){
            listPositionModel.Add(positionModel1);
            listPositionModel.Add(positionModel3);
        }
        else
        {
            listPositionModel.Add(positionModel1);
            listPositionModel.Add(positionModel2);
            listPositionModel.Add(positionModel3);
            listPositionModel.Add(positionModel4);
        }
    }

    void FixedUpdate()
    {
        if (null != sfConnection)
        {
            sfConnection.ProcessEvents();
        }
    }

    void initHandeler()
    {
        // Register callback delegate
        sfConnection.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfConnection.AddEventListener(SFSEvent.OBJECT_MESSAGE, OnObjectMesssage);
        sfConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        // sfsConnection.AddEventListener(SFCommands.CM_READY_RIGHT_ROOM, OnReadyFightRoom);

    }

    void getUserReady()
    {
        SFSObject mParamsRequest = new SFSObject();
        SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_GET_USER_READY);
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
       // Debug.Log("tinhvc OnConnectionLost");
        UnregisterSFSSceneCallbacks();
        sfConnection = null;
        SmartFoxConnection.Connection = sfConnection;
    }

    void OnObjectMesssage(Sfs2X.Core.BaseEvent evt)
    {
        // User sender = evt.Params["sender"] as User;
        //ISFSObject dataObj = evt.Params["message"] as SFSObject;
        //Debug.Log("tinhvc On Object Messsage");
    }
   
    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
       
        string sCmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;

       // Debug.Log("tinhvc Ready Click extension response: " + sCmd);
        switch (sCmd)
        {
            case SFCommands.CM_READY_FIGHT_ROOM:

                string username = dataObject.GetUtfString(SFCommands.USERNAME_KEY);
                int location = dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION);

               // Debug.Log("tinhvc ready at: " + dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION));

                if (VariableApplication.iPlayerCount == 2)
                {
                    listButtons[dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION) -1].GetComponentInChildren<UILabel>().text = "Ready!";
                    mapUserLocarion.Remove(username);
                    mapUserLocarion.Add(username, listButtons[dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION) - 1]);
                    if (VariableApplication.sUserNameSFS == username)
                    {
                        isClickReady = true;
                    }
                    else
                    {
                        listButtons[dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION) - 1].GetComponent<UIButton>().enabled = false;
                    }
                }
                else {
                    listButtons[dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION) + 1].GetComponentInChildren<UILabel>().text = "Ready!";
                    mapUserLocarion.Remove(username);
                    mapUserLocarion.Add(username, listButtons[dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION) + 1]);

                    if (VariableApplication.sUserNameSFS == username)
                    {
                        isClickReady = true;
                    }
                    else
                    {
                        listButtons[dataObject.GetInt(SFCommands.READY_FIGHT_ROOM_LOCATION) + 1].GetComponent<UIButton>().enabled = false;
                    }
                }
                break;

            case SFCommands.CM_READY_START_GAME:

                //Debug.Log("username user 1: "+dataObject.GetUtfString(SFCommands.USERNAME_1));
                

                int idHost = dataObject.GetInt(SFCommands.ID_USER_HOST);
               // Debug.Log("ID User host: " + idHost);

                if(VariableApplication.iPlayerCount == 2){

                    if(!isClickReady){
                        // for viewer
                        VariableApplication.iIdYourPartnerSFS = dataObject.GetInt(SFCommands.ID_USER_1);
                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                    }
                    
                    VariableApplication.sUsername1 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                    VariableApplication.iIdUser1 = dataObject.GetInt(SFCommands.ID_USER_1);

                    VariableApplication.sUsername2 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                    VariableApplication.iIdUser2 = dataObject.GetInt(SFCommands.ID_USER_2);

                    if (VariableApplication.iIdUserSFS == VariableApplication.iIdUser1)
                    {
                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                    }
                    else{
                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                    }
                }else{
                    VariableApplication.sUsername1 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                    VariableApplication.iIdUser1 = dataObject.GetInt(SFCommands.ID_USER_1);

                    VariableApplication.sUsername2 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                    VariableApplication.iIdUser2 = dataObject.GetInt(SFCommands.ID_USER_2);

                    VariableApplication.sUsername3 = dataObject.GetUtfString(SFCommands.USERNAME_3);
                    VariableApplication.iIdUser3 = dataObject.GetInt(SFCommands.ID_USER_3);

                    VariableApplication.sUsername4 = dataObject.GetUtfString(SFCommands.USERNAME_4);
                    VariableApplication.iIdUser4 = dataObject.GetInt(SFCommands.ID_USER_4);

                    if (VariableApplication.iIdUserSFS == VariableApplication.iIdUser1)
                    {
                        VariableApplication.iIdYourPartnerSFS = VariableApplication.iIdUser2;

                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser11 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_3);
                        VariableApplication.idUser22 = dataObject.GetUtfString(SFCommands.USERNAME_4);
                    }
                    else if (VariableApplication.iIdUserSFS == VariableApplication.iIdUser2)
                    {
                        VariableApplication.iIdYourPartnerSFS = VariableApplication.iIdUser1;
                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                        VariableApplication.idUser11 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_3);
                        VariableApplication.idUser22 = dataObject.GetUtfString(SFCommands.USERNAME_4);
                    }
                    else if (VariableApplication.iIdUserSFS == VariableApplication.iIdUser3)
                    {
                        VariableApplication.iIdYourPartnerSFS = VariableApplication.iIdUser4;

                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_3);
                        VariableApplication.idUser11 = dataObject.GetUtfString(SFCommands.USERNAME_4);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser22 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                    }
                    else if (VariableApplication.iIdUserSFS == VariableApplication.iIdUser4)
                    {
                        VariableApplication.iIdYourPartnerSFS = VariableApplication.iIdUser3;

                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_4);
                        VariableApplication.idUser11 = dataObject.GetUtfString(SFCommands.USERNAME_3);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser22 = dataObject.GetUtfString(SFCommands.USERNAME_2);

                    }else{
                        //for viewer
                        VariableApplication.iIdYourPartnerSFS = VariableApplication.iIdUser1;
                        VariableApplication.iIdYourPartnerSFS1 = VariableApplication.iIdUser2;

                        VariableApplication.idUser1 = dataObject.GetUtfString(SFCommands.USERNAME_1);
                        VariableApplication.idUser11 = dataObject.GetUtfString(SFCommands.USERNAME_2);
                        VariableApplication.idUser2 = dataObject.GetUtfString(SFCommands.USERNAME_3);
                        VariableApplication.idUser22 = dataObject.GetUtfString(SFCommands.USERNAME_4);
                    }
                }


                //Debug.Log("ID UserName1: " + VariableApplication.idUser1);
                //Debug.Log("ID UserName2: " + VariableApplication.idUser11);
                //Debug.Log("ID UserName3: " + VariableApplication.idUser2);
                //Debug.Log("ID UserName4: " + VariableApplication.idUser22);

                if (!isClickReady)
                {
                    VariableApplication.isViewer = true;
                }
                else {
                    VariableApplication.isViewer = false;
                }

                if (idHost == VariableApplication.iIdUserSFS)
                {
                    VariableApplication.isHost = true;
                }
                else {
                    VariableApplication.isHost = false;
                }


                VariableApplication.listNumbers = dataObject.GetIntArray(SFCommands.LIST_NUMBER);

                CloneNumberController.isStartGame = true;
                //Application.LoadLevel("PlayGame");
                StartCoroutine(getMoneyUser());


                break;

            case SFCommands.CM_GET_USER_READY:


                bool[] listTeamA = dataObject.GetBoolArray(SFCommands.SIZE_TEAM_A);
                bool[] listTeamB = dataObject.GetBoolArray(SFCommands.SIZE_TEAM_B);

                if (listTeamA.Length == 1)
                {
                    for (int i = 0; i < listTeamA.Length; i++ )
                    {
                        Debug.Log("tinhvc team A" + i + " = " + listTeamA[i]);
                        Debug.Log("tinhvc team B" + i + " = " + listTeamB[i]);
                        if(listTeamA[i]){
                            listButtons[i].GetComponentInChildren<UILabel>().text = "Ready!";
                            listButtons[i].GetComponent<UIButton>().enabled = false;
                        }

                        if (listTeamB[i])
                        {
                            listButtons[i+1].GetComponentInChildren<UILabel>().text = "Ready!";
                            listButtons[i+1].GetComponent<UIButton>().enabled = false;
                        }

                    }
                }
                else {
                    for (int i = 0; i < listTeamA.Length; i++)
                    {
                      //  Debug.Log("tinhvc team A" + i + " = " + listTeamA[i]);
                       // Debug.Log("tinhvc team B" + i + " = " + listTeamB[i]);
                        if (listTeamA[i])
                        {
                            listButtons[i+2].GetComponentInChildren<UILabel>().text = "Ready!";
                            listButtons[i+2].GetComponent<UIButton>().enabled = false;
                        }

                        if (listTeamB[i])
                        {
                            listButtons[i+4].GetComponentInChildren<UILabel>().text = "Ready!";
                            listButtons[i+4].GetComponent<UIButton>().enabled = false;
                        }

                    }
                
                }

                break;

            case SFCommands.CM_USER_DISCONNECT:
               
                int idUserDisconnect = dataObject.GetInt(SFCommands.ID_USER_DISCONNECT);
                string UsernameUserDisconnect = dataObject.GetUtfString (SFCommands.USERNAME_USER_DISCONNECT);
                int LocationUserDisconnect = dataObject.GetInt(SFCommands.LOCATION_USER_DISCONNECT);

                //Debug.Log("tinhvc lolcation leave: " + LocationUserDisconnect);
                GameObject btnLeave = mapUserLocarion[UsernameUserDisconnect] as GameObject;

                btnLeave.GetComponentInChildren<UILabel>().text = "Waiting for user!";
                if (!isClickReady)
                {
                    btnLeave.GetComponent<UIButton>().enabled = true;
                }
                
                break;
        }
    }

    void jsonCallback(JSON jsonResult) {
        GameObject labelMoney = listPositionModel[jsonResult.ToInt("position")].getPlayerObj().transform.GetChild(1).gameObject;
        labelMoney.GetComponent<UILabel>().text = jsonResult.ToString("VALUE");
    }

    IEnumerator getMoneyUser()
    {
        List<string> listIdUser = new List<string>();
        if (VariableApplication.iPlayerCount == 2)
        {
            listIdUser.Add(VariableApplication.idUser1);
            listIdUser.Add(VariableApplication.idUser2);
        }
        else {
            listIdUser.Add(VariableApplication.idUser1);
            listIdUser.Add(VariableApplication.idUser11);
            listIdUser.Add(VariableApplication.idUser2);
            listIdUser.Add(VariableApplication.idUser22);
        }
       
       
        for (int i = 0; i < VariableApplication.iPlayerCount; i++)
        {

            yield return new WaitForSeconds(0.1f);

            JSON jsonSend = new JSON();

            jsonSend["user_id"] = listIdUser[i];
            jsonSend["money_id"] = 3;
            string key = "7d5cb12c25767e7b275048034235b10b";
            string number = "1";

            JsonMethod jsonMethod = new JsonMethod();
            StartCoroutine(jsonMethod.getJSONFromUrl(ApiEntity.GET_GAME_MONEY_USER_URL, jsonSend.serialized, key, number, listIdUser[i], i, jsonCallback));
        }
    }

    private void UnregisterSFSSceneCallbacks()
    {
        // This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
        if (sfConnection != null)
            sfConnection.RemoveAllEventListeners();
    }

    void OnDestroy()
    {
        try
        {
            UnregisterSFSSceneCallbacks();
        }
        catch { 
        
        }
    }

    /**
     * CONTROL CLICK
     */

    public void OnClickReady1()
    {
        Debug.Log("OnClickReady1: " + isClickReady);
        if (!isClickReady)
        {
//            Debug.Log("OnClickReady1");
           // initHandeler();
            SFSObject mParamsRequest = new SFSObject();
            mParamsRequest.PutInt(SFCommands.READY_FIGHT_ROOM_LOCATION, 1);
            SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_READY_FIGHT_ROOM);
        }
    }

    public void OnClickReady2()
    {
        if (!isClickReady)
        {
           // initHandeler();
            SFSObject mParamsRequest = new SFSObject();
            mParamsRequest.PutInt(SFCommands.READY_FIGHT_ROOM_LOCATION, 2);
            SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_READY_FIGHT_ROOM);
        }
    }

    public void OnClickReady3()
    {
        if (!isClickReady)
        {
          //  initHandeler();
            SFSObject mParamsRequest = new SFSObject();
            mParamsRequest.PutInt(SFCommands.READY_FIGHT_ROOM_LOCATION, 1);
            SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_READY_FIGHT_ROOM);
        }
    }

    public void OnClickReady4()
    {
        if (!isClickReady)
        {
           // initHandeler();
            SFSObject mParamsRequest = new SFSObject();
            mParamsRequest.PutInt(SFCommands.READY_FIGHT_ROOM_LOCATION, 2);
            SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_READY_FIGHT_ROOM);
        }
    }

    public void OnClickReady5()
    {
        if (!isClickReady)
        {
           // initHandeler();
            SFSObject mParamsRequest = new SFSObject();
            mParamsRequest.PutInt(SFCommands.READY_FIGHT_ROOM_LOCATION, 3);
            SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_READY_FIGHT_ROOM);
        }
    }

    public void OnClickReady6()
    {
        if(!isClickReady){
           // initHandeler();
            SFSObject mParamsRequest = new SFSObject();
            mParamsRequest.PutInt(SFCommands.READY_FIGHT_ROOM_LOCATION, 4);
            SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_READY_FIGHT_ROOM);
        }
        
    }

    private int iClickWaiting = -1;
    private bool isClickReady = false;
    void setPlayoutReady() {

        for (int i = 0; i < 6; i++) {
            //if (i == iClickWaiting)
            //{
            //    listButtons[i].GetComponentInChildren<UILabel>().text = "Ready!";
            //}
            //else {
            //    listButtons[i].GetComponentInChildren<UILabel>().text = "Waiting for user!";
            //}
            listButtons[i].GetComponent<UIButton>().enabled = false;
        }
    }

    IEnumerator PingServer()
    {
        //Debug.Log("ping");
        yield return new WaitForSeconds(5);
        if (sfConnection != null && sfConnection.IsConnected)
        {
            sfConnection.Send(new ExtensionRequest(SFCommands.USER_PING_SERVER, new SFSObject()));
        }
        StartCoroutine(PingServer());
    }

   public void OnClickBack() {
       try
       {
           UnregisterSFSSceneCallbacks();
           SmartFoxConnection.Connection.Disconnect();
       }
       catch { 
       
       }
        Application.LoadLevel("Home");
    }
}
