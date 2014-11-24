using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
using System.Collections.Generic;

public class InitViewPlayGame : MonoBehaviour {

    private SmartFox sfConnection;
    private SFSObject receivedData;

    float x1 = -0.35f;
    float x11 = -0.46f;
    float x2 = 0.75f;
    float x22 = 0.6f;
    float yMoved = 0.85f;
    public GameObject labelMoneyAdd;

    public GameObject player1;
    public GameObject player11;
    public GameObject player2;
    public GameObject player22;

    public float xOld1;
    public float yOld1;
    public float xOld2;
    public float yOld2;

    public GameObject tvMoneyChip;
    public GameObject tvLevel;
    public GameObject tvPercentLevel;
    public GameObject pgLevel;


    private List<PositionUserModel> listPositionModel = new List<PositionUserModel>();
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
	void Start () {

        if (VariableApplication.iPlayerCount == 2)
        {
            setPositionPlayer();
        }
        addListPosition();
        getInfoProfile();
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

        if (VariableApplication.iPlayerCount == 2)
        {
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

    void setPositionPlayer() {
        player11.SetActive(false);
        player22.SetActive(false);

        xOld1 = player1.transform.localPosition.x;
        yOld1 = player1.transform.localPosition.y;
        xOld2 = player2.transform.localPosition.x;
        yOld2 = player2.transform.localPosition.y;

        player1.transform.localPosition = new Vector3(xOld1 + 40, yOld1, 0);
        player2.transform.localPosition = new Vector3(xOld2 + 20, yOld2, 0);
    }

    void getInfoProfile()
    {
        LamaControllib.getInstance().getAllMoneyUserService(LamaControllib.getInstance().getUserModel().IdUser, getMoneyUserCallback, this);
        LamaControllib.getInstance().getXPModelService(LamaControllib.getInstance().getUserModel().IdUser, getXPUserCallback, this);
    }

    void getMoneyUserCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            tvMoneyChip.GetComponent<UILabel>().text = "$ " + Utilities.getVNCurrency(int.Parse(LamaControllib.getInstance().getMoneyUserModel().MoneyChip)) + " Đ";
        }
    }

    void getXPUserCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            tvLevel.GetComponent<UILabel>().text = "Lv: " + LamaControllib.getInstance().getXPModel().Level;
            pgLevel.GetComponent<UISlider>().value = float.Parse(LamaControllib.getInstance().getXPModel().Persent);
            tvPercentLevel.GetComponent<UILabel>().text = float.Parse(LamaControllib.getInstance().getXPModel().Persent) * 100 + "%";
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
    }

	// Update is called once per frame
	void Update () {
	
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
        
    }

    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string sCmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        SFSObject dataObject = evt.Params[MySFSParams.PARAM_DATA] as SFSObject;

        StartCoroutine(eventSendNumer(sCmd, dataObject));
    }

    IEnumerator eventSendNumer(string sCmd, ISFSObject dataObject)
    {
        yield return new WaitForSeconds(0);
        switch (sCmd)
        {
            case SFCommands.CM_MONEY_ADD:

                string jsonResult = dataObject.GetUtfString(SFCommands.DATA_MONEY_ADD);
                JSON jss = new JSON();
                jss.serialized = jsonResult;

                bool isAdd = jss.ToBoolean("is_add");
                string username = jss.ToString("username");
                int moneyAdd = jss.ToInt("money_add");

                if (isAdd)
                {
                    Debug.Log("Ban đã được cộng: " + moneyAdd);
                }
                else
                {
                    Debug.Log("Ban đã bị trừ: " + moneyAdd);
                }


                GameObject[] listMoneyCardA = new GameObject[VariableApplication.iPlayerCount/2];
                GameObject[] listMoneyCardB = new GameObject[VariableApplication.iPlayerCount / 2];
                for (int i = 0; i < listMoneyCardA.Length; i++)
                {
                    GameObject cloneCard = Instantiate(labelMoneyAdd, transform.position, transform.rotation) as GameObject;
                    cloneCard.transform.parent = transform;
                    cloneCard.transform.localScale = new Vector3(1, 1, 1);
                    cloneCard.transform.localPosition = new Vector3(0, 0, 0);
                    if (isAdd)
                    {
                        cloneCard.GetComponent<UILabel>().text = "+" + moneyAdd;
                    }
                    else
                    {
                        cloneCard.GetComponent<UILabel>().text = "-" + moneyAdd;
                    }
                    listMoneyCardA[i] = cloneCard;
                }

                for (int i = 0; i < listMoneyCardB.Length; i++)
                {
                    GameObject cloneCard = Instantiate(labelMoneyAdd, transform.position, transform.rotation) as GameObject;
                    cloneCard.transform.parent = transform;
                    cloneCard.transform.localScale = new Vector3(1, 1, 1);
                    cloneCard.transform.localPosition = new Vector3(0, 0, 0);
                    if (isAdd)
                    {
                        cloneCard.GetComponent<UILabel>().text = "-" + moneyAdd;
                    }
                    else
                    {
                        cloneCard.GetComponent<UILabel>().text = "+" + moneyAdd;
                    }
                    listMoneyCardB[i] = cloneCard;
                }

                if (VariableApplication.iPlayerCount == 2)
                {

                    StartCoroutine(Utilities.AnimationMove(listMoneyCardA[0], x1, yMoved, 5, movetoCallback));
                    StartCoroutine(Utilities.AnimationMove(listMoneyCardB[0], x2, yMoved, 5, movetoCallback));
                }
                else
                {
                    StartCoroutine(Utilities.AnimationMove(listMoneyCardA[0], x1 -0.1f, yMoved, 5, movetoCallback));
                    StartCoroutine(Utilities.AnimationMove(listMoneyCardA[1], x11 + 0.4f, yMoved, 5, movetoCallback));
                    StartCoroutine(Utilities.AnimationMove(listMoneyCardB[0], x2 - 0.1f, yMoved, 5, movetoCallback));
                    StartCoroutine(Utilities.AnimationMove(listMoneyCardB[1], x22 + 0.5f, yMoved, 5, movetoCallback));

                }
               
                StartCoroutine(getMoneyUser());


                break;
        }

    }

    void jsonCallback(JSON jsonResult)
    {
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
        else
        {
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

        yield return new WaitForSeconds(5);
        Application.LoadLevel("PlayGame");
    }

    void movetoCallback(GameObject gameobj)
    {
            Destroy(gameobj);
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
}
