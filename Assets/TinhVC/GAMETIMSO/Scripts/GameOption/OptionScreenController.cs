using UnityEngine;
using System.Collections;

public class OptionScreenController : MonoBehaviour {

    public tk2dUIScrollbar scrollBar;
    public tk2dTextMesh labelNotify;
    public tk2dTextMesh labelMoney1;
    public tk2dTextMesh labelMoney2;


    public GameObject friendListPrefab;
    void Start()
    {
        scrollBar.Value = 0.65f;
        //string text = "Tổng giải thưởng 12.000.099 đ (80%). Lên 100% sẽ khởi động giải vô địch!";
        //labelNotify.text = text;
        scrollBar.enabled = false;
        labelMoney1.text = "$ " + Utilities.getVNCurrency(Utilities.GetMoneyQuick() * 1000);
        labelMoney2.text = "$ " + Utilities.getVNCurrency(Utilities.GetMoneyFriend() * 1000);

        getInfoProfile();
        getAvatarUser();
    }

    void Update()
    {

    }

    void QuickGame()
    {
        VariableApplication.isQuick = true;
        Application.LoadLevel("CreateRoom");
    }
    void FriendGame()
    {
        VariableApplication.isQuick = false;
        Application.LoadLevel("CreateRoom");
    }

    void vs1(){
        VariableApplication.iPlayerCount = 2;
        VariableApplication.isOffline = false;
        Application.LoadLevel("CreateRoom");
    }

    void vs2(){
        VariableApplication.iPlayerCount = 4;
        VariableApplication.isOffline = false;
        Application.LoadLevel("CreateRoom");
    }

    void train(){
        Debug.Log("tinhvc click");
        VariableApplication.isOffline = true;
        Application.LoadLevel("PlayGame");
    }

    void OnCLickDTSD()
    {
        string url = string.Format("sms:{0}?body={1}", 9999,  "LM 63 3");
        //Application.OpenURL("sms:" + 999999 + "&body=" + "LM 32 2");
        Application.OpenURL(url);
    }

    void OnClickDeposit()
    {
        LamaControllib.getInstance().LoadLevel("Deposit");
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

    void onBack()
    {
        Application.LoadLevel("Home");
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
        LamaControllib.getInstance().getAvatarService(LamaControllib.getInstance().getUserModel().IdUser, getAvatarCallback, this, 0);
    }

    void getAvatarCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            Debug.Log("AVATAR!!!");
            StartCoroutine(Utilities.setAvatarUser(pictureObj, LamaControllib.getInstance().getUserModel().Avatar, 1f));
        }
    }
}
