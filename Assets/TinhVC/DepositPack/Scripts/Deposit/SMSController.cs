using UnityEngine;
using System.Collections;

public class SMSController : MonoBehaviour {
    public tk2dTextMesh Option1, Option2, Option3;
    public GameObject PopupMessage;
    public GameObject scrollView;
    public tk2dTextMesh Header, AccountName, DeclineBtn, AcceptBtn;
    // Use this for initialization
    int type;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Option1Click()
    {
        Debug.Log("tinhvc click Option1Click");
        PopupMessage.SetActive(true);
        Header.text = "Bạn chọn nạp $500,000 Chip bằng SMS. Chọn \"đồng ý\" và gửi tin nhắn mua Chip.";
        type = 1;
    }

    void Option2Click()
    {
        Debug.Log("tinhvc click Option2Click");
        PopupMessage.SetActive(true);
        Header.text = "Bạn chọn nạp $1,000,000 Chip bằng SMS. Chọn \"đồng ý\" và gửi tin nhắn mua Chip.";
        type = 2;
    }

    void Option3Click()
    {
        Debug.Log("tinhvc click Option3Click");
        PopupMessage.SetActive(true);
        Header.text = "Bạn chọn nạp $1,500,000 Chip bằng SMS. Chọn \"đồng ý\" và gửi tin nhắn mua Chip.";
        type = 3;
    }

    void DeclineBtnClick()
    {
        Debug.Log("tinhvc click DeclineBtnClick");
        PopupMessage.SetActive(false);
        scrollView.SetActive(true);
    }

    void AcceptBtnClick()
    {

        Debug.Log("Type: " + type);
        //tinhvc send sms
         string IID = "";
        string number = "";
        if(DepositController.indexTab == 1){
            number = LamaControllib.getInstance().getListSMS()[ChipUpdateActive.indexListSMS -1].SendNUmber;
            IID = "3";
        }else{
            IID = "2";
            number = LamaControllib.getInstance().getListSMS()[XenUpdateActive.indexListSMS - 1].SendNUmber;
        }


        string message = "LM " + LamaControllib.getInstance().getUserModel().IdUser + " " + IID;

        Debug.Log("tinhvc number: "+number);

        //string url = string.Format("sms:{0}?body={1}", number, message);
        //Debug.Log("tinhvc url sms: "+url);
        //Application.OpenURL(url);


        //string url = string.Format("sms:{0}?body={1}", "subcode", "content" + message);
        //Application.OpenURL(url);

        string url = string.Format("sms:{0}?body={1}", 9999, "LM 63 3");
        Application.OpenURL(url);

        //SMS(long.Parse(number), message);

        PopupMessage.SetActive(false);
        scrollView.SetActive(true);
    }

    void SMS(long number, string message)
    {

        string separator = (Application.platform == RuntimePlatform.IPhonePlayer) ? ";" : "?";

        string url = string.Format("sms:{0}{1}body={2}", number, separator, WWW.EscapeURL(message));

        Debug.Log("tinhvc url SMS: " + url);
        Application.OpenURL(url);
    }
}
