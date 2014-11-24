using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
using System.Collections.Generic;

public class CloneNumberController : MonoBehaviour {
    public AudioClip clickCorrect;
    public AudioClip clickWrong;
    public AudioClip audioWin;
    public AudioClip audioClose;

	public GameObject buttonNumber;

	public Camera camera;
	public static int level = 1;

    private SmartFox sfConnection;
    private SFSObject receivedData;

    const int MAX_NUMBER = 99;

    private GameObject[] listButtonClone = new GameObject[119];

    public GameObject levelTeamO;
    public GameObject levelTeamX;


    //public GameObject WinLabel;
    public GameObject panelWin;
    public GameObject animationObj;
    public GameObject spriteWin;

    public GameObject TimedownLabel;

    public GameObject nextNumberFind;

    public GameObject boxExit;
    public GameObject btnCloseBoxExit;

    public GameObject WaitingTwoPlayerPanel;
    public GameObject WaitingFourPlayerPanel;
    public GameObject playerTimeDown;

    public static bool isStartGame = false;

 
    void Awake()
    {

        if (SmartFoxConnection.IsInitialized)
        {
            sfConnection = SmartFoxConnection.Connection;
            initHandeler();
        }
        else
        {
            Debug.Log("tinhvc Connection lost");
        }
    }

    bool isViewer;
	void Start () {

        if (!VariableApplication.isOffline)
        {
            level = 1;
            nextNumber = -1;
            isStartGame = false;

            StartCoroutine(WaittingStart());
            playerTimeDown.SetActive(false);
        }
        boxExit.SetActive(false);
        btnCloseBoxExit.SetActive(false);
        animationObj.SetActive(false);
	}


    IEnumerator WaittingStart()
    {
        yield return new WaitForSeconds(0.5f);
        if (isStartGame)
        {
            StopCoroutine(WaittingStart());
            startGame();
        }
        else
        {
            StartCoroutine(WaittingStart());
        }
    }

    void startGame() {
        StartCoroutine(TimeDownStart());

       
        Hashtable args1 = new Hashtable();
        args1.Add("position", new Vector3(0, 3, 0));
        args1.Add("time", 0.1f);
        iTween.MoveTo(WaitingTwoPlayerPanel, args1);
       
        Hashtable args2 = new Hashtable();
        args2.Add("position", new Vector3(0, 3, 0));
        args2.Add("time", 0.1f);
        iTween.MoveTo(WaitingFourPlayerPanel, args2);
    }



    int timeDowns = 5;
    IEnumerator TimeDownStart()
    {
        yield return new WaitForSeconds(1);

        TimedownLabel.GetComponent<UILabel>().text = "Trận đấu sẽ bắt đầu trong " + timeDowns + "s";
        timeDowns--;
        if (timeDowns == -1)
        {
            StopCoroutine(TimeDownStart());
            timeDowns = 5;
            TimedownLabel.GetComponent<UILabel>().text = "";

            isViewer = VariableApplication.isViewer;

            CloneNumber(VariableApplication.listNumbers);

            //if (isViewer)
            //{

            //}
            //else
            //{
            //    if (VariableApplication.isHost)
            //    {
            //        int[] texts = new int[99];
            //        for (int i = 0; i < 99; i++)
            //        {
            //            texts[i] = i + 1;
            //        }
            //        texts = Utilities.reshuffle(texts);
            //        sendListNumbers(texts);
            //    }
            //}
        }
        else {
            StartCoroutine(TimeDownStart());
        }
    }

    void initPlayGame() { 
    
    }
    void FixedUpdate()
    {
        if (null != sfConnection)
        {
            sfConnection.ProcessEvents();
        }
    }

    void sendListNumbers(int[] texts) {
        //initHandeler();
        //Debug.Log("tinhvc list number send");
        SFSObject mParamsRequest = new SFSObject();
       // mParamsRequest.PutUtfString(SFCommands.HEADER_SEND, SFCommands.HEADER_RECEIVE_NUMBER);
        mParamsRequest.PutIntArray(SFCommands.LIST_NUMBER, texts);
        SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_LIST_NUMBER);
    }

 

    void initHandeler()
    {
        // Register callback delegate
        sfConnection.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfConnection.AddEventListener(SFSEvent.OBJECT_MESSAGE, OnObjectMesssage);
        sfConnection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }
    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        Debug.Log("tinhvc OnConnectionLost");
        UnregisterSFSSceneCallbacks();
        sfConnection = null;
        SmartFoxConnection.Connection = sfConnection;
    }

    void OnObjectMesssage(Sfs2X.Core.BaseEvent evt)
    {
        //Debug.Log("tinhvc User: object message arrived");
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;
        string header = dataObject.GetUtfString(SFCommands.HEADER_SEND);
        switch (header)
        {
            case SFCommands.HEADER_RECEIVE_NUMBER:

                //int[] listNumbers = dataObject.GetIntArray(SFCommands.LIST_NUMBER);
                //CloneNumber(listNumbers);

                break;
        }
    }

    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string sCmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;

       // Debug.Log("tinhvc CloneNumber extension response: " + sCmd);

        StartCoroutine(eventSendNumer(sCmd, dataObject));
    }

    IEnumerator eventSendNumer(string sCmd, ISFSObject dataObject)
    {
        yield return new WaitForSeconds(0);
        switch (sCmd)
        {
            case SFCommands.CM_LIST_NUMBER:

                int[] listNumbers = dataObject.GetIntArray(SFCommands.LIST_NUMBER);

               // Debug.Log("tinhvc list size: " + listNumbers.Length);

                CloneNumber(listNumbers);

                break;

            case SFCommands.CM_SEND_NUMBER:

                SFSObject dataSender = dataObject.GetSFSObject(SFCommands.DATA_SEND_NUMBER) as SFSObject;

                int idUserSender = dataSender.GetInt(SFCommands.ID_SENDER_NUMBER);
                int levelSend = dataSender.GetInt(SFCommands.LEVEL_SEND_NUMBER);
                int nextLocationNumber = dataSender.GetInt(SFCommands.NEXT_LOCATION_SEND_NUMBER);
               // Debug.Log("tinhvc nextLocationNumber: " + nextLocationNumber);
                UISprite spriteNumber = listButtonClone[nextLocationNumber].GetComponentInChildren<UISprite>();
                spriteNumber.alpha = 255;

                if (!VariableApplication.isViewer)
                {
                    if (idUserSender == VariableApplication.iIdUserSFS || idUserSender == VariableApplication.iIdYourPartnerSFS)
                    {
                       // Debug.Log("tinhvc  O game: " + listButtonClone[nextLocationNumber].name);
                        spriteNumber.spriteName = "number_selected";
                        levelTeamO.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_SENDER);
                        levelTeamX.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_COMPETITOR);
                        audio.clip = clickCorrect;
                        audio.Play();
                    }
                    else
                    {
                       // Debug.Log("tinhvc value X game: " + listButtonClone[nextLocationNumber].name);
                        spriteNumber.spriteName = "number_unselected";
                        levelTeamX.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_SENDER);
                        levelTeamO.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_COMPETITOR);
                        audio.clip = clickWrong;
                        audio.Play();
                    }
                }
                else
                {
                    if (idUserSender == VariableApplication.iIdUserSFS || idUserSender == VariableApplication.iIdYourPartnerSFS || idUserSender == VariableApplication.iIdYourPartnerSFS1)
                    {
                       // Debug.Log("tinhvc  O game: " + listButtonClone[nextLocationNumber].name);
                        spriteNumber.spriteName = "number_selected";
                        levelTeamO.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_SENDER);
                        levelTeamX.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_COMPETITOR);
                        audio.clip = clickCorrect;
                        audio.Play();
                    }
                    else
                    {
                        //Debug.Log("tinhvc value X game: " + listButtonClone[nextLocationNumber].name);
                        spriteNumber.spriteName = "number_unselected";
                        levelTeamX.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_SENDER);
                        levelTeamO.GetComponent<UILabel>().text = "" + dataObject.GetInt(SFCommands.LEVEL_USER_COMPETITOR);
                        audio.clip = clickWrong;
                        audio.Play();
                    }

                }

                if (levelSend<99)
                {
                    nextNumberFind.GetComponentInChildren<UILabel>().text = "" + (levelSend + 1);
                   
                }
                 spriteNumber.MarkAsChanged();
                 level = levelSend + 1;

                break;

            case SFCommands.CM_GAME_OVER:
                bool isSenderWin = dataObject.GetBool(SFCommands.IS_WIN_OF_SENDER);
                int idSender = dataObject.GetInt(SFCommands.ID_SENDER_NUMBER);

                if (!VariableApplication.isViewer)
                {
                    if (VariableApplication.iIdUserSFS == idSender || VariableApplication.iIdYourPartnerSFS == idSender)
                    {

                        if (isSenderWin)
                        {
                            //WinLabel.GetComponent<UILabel>().text = "YOU WIN";
                            setWinAnimation(true);
                            audio.clip = audioWin;
                            audio.Play();
                        }
                        else
                        {
                           // WinLabel.GetComponent<UILabel>().text = "YOU CLOSE";
                            setWinAnimation(false);
                            audio.clip = audioClose;
                            audio.Play();
                        }

                    }
                    else
                    {
                        if (isSenderWin)
                        {
                           // WinLabel.GetComponent<UILabel>().text = "YOU CLOSE";
                            setWinAnimation(false);
                            audio.clip = audioClose;
                            audio.Play();
                        }
                        else
                        {
                           // WinLabel.GetComponent<UILabel>().text = "YOU WIN";
                            setWinAnimation(true);
                            audio.clip = audioWin;
                            audio.Play();
                        }

                    }
                }

                break;
        }

    }

    private int[] listNumbers = new int[99];
    void CloneNumber(int[] texts)
    {
        //Debug.Log("size list number: "+texts.Length);
        listNumbers = texts;
        float countY = -128;
		int countX = 0;

        float xFinal = 0;
        float yFinal = 0;
		
        int countNumber = 1;
		for (int i =1; i<=120; i++) {
            countX++;

            if (i % 17 == 1)
            {
                countY = countY + 28;
                countX = 1;
            }

            if (i == 5 || i == 9 || i == 19 || i == 31 || i == 41 || i == 71 || i == 79)
            {
                GameObject clone = Instantiate(buttonNumber, transform.position, transform.rotation) as GameObject;
                clone.transform.parent = transform;

                float x = -270 + (31) * countX;
                float y = countY + 10;

                if (texts[countNumber - 1] < 10)
                {
                    if (i == 79)
                    {
                        setPosition(clone, x-15, y-20, 30, 32, UIWidget.Pivot.Top);
                    }
                    else
                    {
                        setPosition(clone, x, y, 36, 34, UIWidget.Pivot.Center);
                    }
                }
                else {
                    if (i == 79)
                    {
                        setPosition(clone, x -13, y-17, 30, 32, UIWidget.Pivot.Top);
                    }
                    else {
                        setPosition(clone, x, y, 30, 32, UIWidget.Pivot.Top);
                    }
                }

                if (i == 79)
                {
                    xFinal = x;
                    yFinal = y;

                    UISprite spriteNumber = clone.GetComponentInChildren<UISprite>();
                    spriteNumber.width = 25;
                    spriteNumber.height = 25;
                }
                else
                {
                    UISprite spriteNumber = clone.GetComponentInChildren<UISprite>();
                    spriteNumber.width = 50;
                    spriteNumber.height = 50;
                }
                setProperty(clone, texts[countNumber - 1],true);
                countNumber++;
            }
            else
            {
                if (i != 5 && i != 6 && i != 9 && i != 10 && i != 19 && i != 20 && i != 22 && i != 23 && i != 26 && i != 27 && i != 31 && i != 32 && i != 36 && i != 37 && i != 41 && i != 42 && i != 48 && i != 49 && i != 58 && i != 59 && i != 71 && i != 72 && i != 79 && i != 80 && i != 88 && i != 89 && i != 96 && i != 97 && i != 120)
                {
                    GameObject clone = Instantiate(buttonNumber, transform.position, transform.rotation) as GameObject;
                    clone.transform.parent = transform;
                    clone.transform.localPosition = new Vector3(-280 + (31) * countX, countY, 0);
                    UILabel cloneLabel = clone.GetComponentInChildren<UILabel>();
                    cloneLabel.pivot = UIWidget.Pivot.Center;

                    setProperty(clone, texts[countNumber - 1],false);

                    countNumber++;
                }
            }

            if (i == 120)
            {
                GameObject clone = Instantiate(buttonNumber, transform.position, transform.rotation) as GameObject;
                clone.transform.parent = transform;

                if (texts[countNumber - 1] < 10)
                {
                    setPosition(clone, xFinal + 17, yFinal + 13, 36, 34, UIWidget.Pivot.Center);
                }
                else
                {
                    setPosition(clone, xFinal + 17, yFinal + 13, 30, 32, UIWidget.Pivot.Top);
                }
                setProperty(clone, texts[countNumber - 1], true);
            }

        }
    }

    void setProperty(GameObject clone, int number, bool isSetBg) {

        clone.GetComponentInChildren<UILabel>().text = ""+ number;
        clone.transform.localScale = new Vector3(1, 1, 1);
        clone.name = "" + number;

        GameObject bgNumber = clone.transform.GetChild(0).gameObject;
        if(isSetBg){
            bgNumber.transform.localPosition = new Vector3(9, 3, 0);
        }
        bgNumber.GetComponent<UIWidget>().depth = 45;

        listButtonClone[number] = clone;
    }

    void setPosition(GameObject clone, float x, float y, int w, int h, UIWidget.Pivot pivot)
    {
        UILabel cloneLabel = clone.GetComponentInChildren<UILabel>();
        clone.transform.localPosition = new Vector3(x, y, 0);
        cloneLabel.width = w;
        cloneLabel.height = h;
        cloneLabel.pivot = pivot;

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
        catch
        {
        }
    }

   public static int nextNumber = -1;
   public static int nextLocation = -1;

   void SendNumberMsg()
   {
       //Debug.Log("tinhvc id sender: " + VariableApplication.iIdUserSFS);
       //Debug.Log("tinhvc level: " + level);
       //Debug.Log("tinhvc location: " + nextLocation);

       SFSObject mParamsRequest = new SFSObject();
       mParamsRequest.PutInt(SFCommands.ID_SENDER_NUMBER, VariableApplication.iIdUserSFS);
       mParamsRequest.PutInt(SFCommands.LEVEL_SEND_NUMBER, level);
       mParamsRequest.PutInt(SFCommands.NEXT_LOCATION_SEND_NUMBER, nextLocation);
       SmartFoxConnection.SendReadyMsg(mParamsRequest, SFCommands.CM_SEND_NUMBER);
   }

   void Update()
   {
       if (Input.GetKeyDown(KeyCode.Escape))
       {
           try
           {
               UnregisterSFSSceneCallbacks();
               SmartFoxConnection.Connection.Disconnect();
           }
           catch
           {

           }
           Application.LoadLevel("Home");
           return;
       }
   }
   //IEnumerator PingServer()
   //{
   //    //Debug.Log("ping");
   //    yield return new WaitForSeconds(5);
   //    if (sfConnection != null && sfConnection.IsConnected)
   //    {
   //        sfConnection.Send(new ExtensionRequest(SFCommands.USER_PING_SERVER, new SFSObject()));
   //    }
   //    StartCoroutine(PingServer());
   //}

   public GameObject btnStandUp;
   public void OnClickBack()
   {
       if (!VariableApplication.isOffline)
       {
           gameObject.GetComponent<UIPanel>().alpha = 0.3f;
           boxExit.SetActive(true);
           btnCloseBoxExit.SetActive(true);
           if (VariableApplication.iPlayerCount == 2)
           {
               btnStandUp.GetComponent<UIButton>().enabled = false;
               btnStandUp.GetComponentInChildren<UISprite>().alpha = 0.5f;
           }
       }
       else
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

      
   }

   public void OnClickCloseBoxExit()
   {
       SendCloseBoxExitMsg();
   }

   void SendCloseBoxExitMsg()
   {
       gameObject.GetComponent<UIPanel>().alpha = 1;
       boxExit.SetActive(false);
       btnCloseBoxExit.SetActive(false);
   }

   public void QuickRoom()
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

       SendCloseBoxExitMsg();
   }

   public void CreateNewRoom()
   {
       SendCloseBoxExitMsg();
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

   void setWinAnimation(bool isWin)
   {
       UISprite _spriteWin = spriteWin.GetComponent<UISprite>();
       if(isWin){
           Debug.Log("tinhvc thang");
           _spriteWin.spriteName = "win_sprite";
           _spriteWin.MarkAsChanged();
       }
       else
       {
           Debug.Log("tinhvc thua");
           _spriteWin.spriteName = "close_sprite";
           _spriteWin.MarkAsChanged();
       }
       panelWin.GetComponent<UIPanel>().alpha = 1;
       animationObj.SetActive(true);
   }
}
