using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TabController : MonoBehaviour
{
    public tk2dTextMesh CardTextMesh, SMSTextMesh, XenTextMesh;
    public GameObject CardBtn, SMSBtn, XenBtn;
    public GameObject CardTabContent, CardTabScrollView, SMSTabContent, XenTabContent, SMSPopup;
    //public List<CardModel> listCard;
    //public List<SMSModel> listSMS;
    public int tabIndex;
    public string tabName;
	// Use this for initialization
	void Start () {
        //LamaControllib.getInstance().getListCardService("2", getListCardCallback, this);
        //LamaControllib.getInstance().getListSMSService("2", getListSmsCallback, this);
        CardClick();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CardClick()
    {
        CardBtn.SetActive(true);
        XenBtn.SetActive(false);
        SMSBtn.SetActive(false);
        tabIndex = 1;
        CardTextMesh.color = new Color32(133, 17, 17, 255);
        SMSTextMesh.color = new Color(255, 255, 255);
        XenTextMesh.color = new Color(255, 255, 255);
        tabName = "Card";
        //CardTabContent.SetActive(true);
        CardTabScrollView.SetActive(true);
        XenTabContent.SetActive(false);
        SMSTabContent.SetActive(false);
        SMSPopup.SetActive(false);
        //LamaControllib.getInstance().getListCardService("2", callbackCardList, this);
    }

    //void getListCardCallback(bool isSuccess, JSON jsonResult)
    //{
    //    if (isSuccess)
    //    {
    //        Debug.Log("getlistcard sucess");
    //        int i = 1;
    //        List<CardModel> card = LamaControllib.getInstance().getListCard();
    //        foreach (CardModel temp in card)
    //        {
    //            listCard[i].Card = temp.Card;
    //            listCard[i].Chip = temp.Chip;
    //            listCard[i].Xen = temp.Xen;
    //            i++;
    //        }
    //    }
    //    Debug.Log("card: " + listCard[1].Card);
    //}

    //void getListSmsCallback(bool isSuccess, JSON jsonResult)
    //{
    //    if (isSuccess)
    //    {
    //        Debug.Log("getlistsms sucess");
    //        int i = 1;
    //        List<SMSModel> sms = LamaControllib.getInstance().getListSMS();
    //        foreach (SMSModel temp in sms)
    //        {
    //            listSMS[i].SMS = temp.SMS;
    //            listSMS[i].Chip = temp.Chip;
    //            listSMS[i].Xen = temp.Xen;
    //            i++;
    //        }
    //    }
    //    Debug.Log("card: " + listSMS[1].SMS);
    //}

    void SMSClick()
    {
        CardBtn.SetActive(false);
        XenBtn.SetActive(false);
        SMSBtn.SetActive(true);
        tabIndex = 2;
        SMSTextMesh.color = new Color32(133, 17, 17, 255);
        CardTextMesh.color = new Color(255, 255, 255);
        XenTextMesh.color = new Color(255, 255, 255);
        tabName = "SMS";
        CardTabContent.SetActive(false);
        CardTabScrollView.SetActive(false);
        XenTabContent.SetActive(false);
        SMSTabContent.SetActive(true);
    }

    void XenClick()
    {
        CardBtn.SetActive(false);
        XenBtn.SetActive(true);
        SMSBtn.SetActive(false);
        tabIndex = 3;
        XenTextMesh.color = new Color32(133, 17, 17, 255);
        SMSTextMesh.color = new Color(255, 255, 255);
        CardTextMesh.color = new Color(255, 255, 255);
        tabName = "Xen";
        //CardTabContent.SetActive(false);
        CardTabScrollView.SetActive(false);
        XenTabContent.SetActive(true);
        SMSTabContent.SetActive(false);
        SMSPopup.SetActive(false);
    }
}
