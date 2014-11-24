using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChipTabController : MonoBehaviour {
    public GameObject VinaphoneIcon, MobifoneIcon, ViettelIcon;
    public Sprite Vinaphone1, Mobifone1, Viettel1;
    public Sprite Vinaphone2, Mobifone2, Viettel2;
    public tk2dTextMesh CodeNumber, SerialNumber;
    public DepositController tabController;
    public GameObject SendRequest;
    public GameObject ScrollViewContent;
    public Sprite coin1, coin2, coin3, coin4;
    public GameObject Item;
    string userID;
    List<CardModel> listCard;
    List<SMSModel> listSMS;
    int[] ID;
    long[] Money, Prize;
	// Use this for initialization
	void Start () {
        //if (gameObject.transform.parent.name.Equals("Card"))
        //{
        //    LamaControllib.getInstance().getListCardService(userID, getListCardCallback, this);
        //}
        //else
        //{
        //    LamaControllib.getInstance().getListSMSService(userID, getListSmsCallback, this);
        //}
        StartCoroutine(delay());
        pupopError.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    string typeCard = "";
    //tinhvc click type card
    void VinaphoneClick()
    {
        typeCard = "vinaphone";
        VinaphoneIcon.GetComponent<SpriteRenderer>().sprite = Vinaphone2;
        MobifoneIcon.GetComponent<SpriteRenderer>().sprite = Mobifone1;
        ViettelIcon.GetComponent<SpriteRenderer>().sprite = Viettel1;
    }

    void MobifoneClick()
    {
        typeCard = "mobiphone";
        VinaphoneIcon.GetComponent<SpriteRenderer>().sprite = Vinaphone1;
        MobifoneIcon.GetComponent<SpriteRenderer>().sprite = Mobifone2;
        ViettelIcon.GetComponent<SpriteRenderer>().sprite = Viettel1;
    }

    void ViettelClick()
    {
        typeCard = "viettel";
        VinaphoneIcon.GetComponent<SpriteRenderer>().sprite = Vinaphone1;
        MobifoneIcon.GetComponent<SpriteRenderer>().sprite = Mobifone1;
        ViettelIcon.GetComponent<SpriteRenderer>().sprite = Viettel2;
    }

    void SendRequestClick()
    {
        //tinhvc send card

        Debug.Log(CodeNumber.text);
        Debug.Log(SerialNumber.text);

        if (checkInputCard())
        {
            LamaControllib.getInstance().sendNumberCardService(LamaControllib.getInstance().getUserModel().IdUser, typeCard, CodeNumber.text, SerialNumber.text, "3", callBacksendNumberCard, this);
        }

    }

   void callBacksendNumberCard(bool isSuccess, JSON jsonResult){
       if (isSuccess)
       {
           string mess = "Bạn đã nạp thành công, tài khoản của bạn hiện có " + Utilities.getVNCurrency(long.Parse(LamaControllib.getInstance().getNumberCardModel().Amount)) + " Đ";
           showError(mess);
       }
       else
       {
           showError(LamaControllib.getInstance().getNumberCardModel().Message);
       }
    }

    public GameObject pupopError;
    public bool checkInputCard()
    {
        if (this.CodeNumber.text.Equals(""))
        {
            showError("Mã thẻ không được để trống!");
            return false;
        }

        if (this.SerialNumber.text.Equals(""))
        {
            showError("Số seri thẻ không được để trống!");
            return false;
        }

        if (this.typeCard.Equals(""))
        {
            showError("Vui lòng chọn loại thẻ muốn nạp!");
            return false;
        }

        return true;
    }

    private void showError(string text)
    {
        Debug.Log("tinhvc show error: "+text);
        pupopError.SetActive(true);
        pupopError.GetComponentInChildren<tk2dTextMesh>().text = text;
    }

    Sprite ReturnSprite(int id)
    {
        switch (id)
        {
            case 1:
                return coin1;
            case 2:
                return coin2;
            case 3:
                return coin3;
            case 4:
                return coin4;
            default:
                return null;
        }
    }

    void getListCardCallback()
    {
        if (true)
        {
            int i = 1;
            float X = -1.1f;
            float Y = 0.2f;
            foreach (CardModel card in listCard)
            {
                GameObject temp = (GameObject)Instantiate(Item);
                temp.transform.FindChild("coin").gameObject.GetComponent<SpriteRenderer>().sprite = ReturnSprite(i);
                temp.transform.FindChild("money").gameObject.GetComponent<tk2dTextMesh>().text = Utilities.getVNCurrency(long.Parse(card.Chip));
                temp.transform.FindChild("prize").gameObject.GetComponent<tk2dTextMesh>().text = Utilities.getVNCurrency(long.Parse(card.Card));
                temp.transform.parent = ScrollViewContent.transform;
                temp.transform.localPosition = new Vector3(X, Y, 0.2f);
                temp.name = "Item" + i;
                Y -= 0.9f;
                i++;
            }
        }
    }

    void getListSmsCallback()
    {
        if (true)
        {
            int i = 1;
            float X = -1.1f;
            float Y = 0.2f;
            foreach (SMSModel sms in listSMS)
            {
                GameObject temp = (GameObject)Instantiate(Item);
                temp.transform.FindChild("coin").gameObject.GetComponent<SpriteRenderer>().sprite = ReturnSprite(i);
                temp.transform.FindChild("money").gameObject.GetComponent<tk2dTextMesh>().text = Utilities.getVNCurrency(long.Parse(sms.Chip));
                temp.transform.FindChild("prize").gameObject.GetComponent<tk2dTextMesh>().text = Utilities.getVNCurrency(long.Parse(sms.SMS));
                temp.transform.parent = ScrollViewContent.transform;
                temp.transform.localPosition = new Vector3(X, Y, 0.2f);
                temp.name = "Item" + i;
                Y -= 0.9f;
                i++;
            }
        }
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
        listCard = tabController.listCard;
        listSMS = tabController.listSMS;
        if (listCard.Count > 0 && listSMS.Count >0)
        {
            if (gameObject.transform.parent.name.Equals("Card"))
            {
                getListCardCallback();
            }
            if (gameObject.transform.parent.name.Equals("SMS"))
            {
                getListSmsCallback();
            }
        }
        else
        {
            StartCoroutine(delay());
        }
    }
}
