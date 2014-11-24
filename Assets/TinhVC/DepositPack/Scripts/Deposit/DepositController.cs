using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DepositController : MonoBehaviour {
    public tk2dTextMesh ChipTextMesh, XenTextMesh, AvatarTextMesh, ItemsTextMesh, BadgetTextMesh;
    public GameObject ChipBtn, XenBtn, AvatarBtn, ItemsBtn, BadgetBtn;
    public GameObject ChipTabContent, XenTabContent, AvatarTabContent, ItemsTabContent, BadgetTabContent;
    public List<CardModel> listCard;
    public List<SMSModel> listSMS;

    public static int indexTab = 1;
	// Use this for initialization

    void Awake()
    {
        listCard = new List<CardModel>();
        listSMS = new List<SMSModel>();
        LamaControllib.getInstance().getListCardService(LamaControllib.getInstance().getUserModel().IdUser, getListCardCallback, this);
        LamaControllib.getInstance().getListSMSService(LamaControllib.getInstance().getUserModel().IdUser, getListSmsCallback, this);
    }

	void Start () {
        indexTab = 1;
        ChipTab();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChipTab()
    {
        indexTab = 1;
        ChipBtn.SetActive(true);
        XenBtn.SetActive(false);
        AvatarBtn.SetActive(false);
        ItemsBtn.SetActive(false);
        BadgetBtn.SetActive(false);

        ChipTabContent.SetActive(true);
        XenTabContent.SetActive(false);
        AvatarTabContent.SetActive(false);
        ItemsTabContent.SetActive(false);
        BadgetTabContent.SetActive(false);

        ChipTextMesh.color = new Color32(133,17,17,255);
        XenTextMesh.color = new Color(255, 255, 255);
        AvatarTextMesh.color = new Color(255, 255, 255);
        ItemsTextMesh.color = new Color(255, 255, 255);
        BadgetTextMesh.color = new Color(255, 255, 255);
    }

    void XenTab()
    {
        indexTab = 2;
        ChipBtn.SetActive(false);
        XenBtn.SetActive(true);
        AvatarBtn.SetActive(false);
        ItemsBtn.SetActive(false);
        BadgetBtn.SetActive(false);

        ChipTabContent.SetActive(false);
        XenTabContent.SetActive(true);
        AvatarTabContent.SetActive(false);
        ItemsTabContent.SetActive(false);
        BadgetTabContent.SetActive(false);

        XenTextMesh.color = new Color32(133, 17, 17, 255);
        ChipTextMesh.color = new Color(255, 255, 255);
        AvatarTextMesh.color = new Color(255, 255, 255);
        ItemsTextMesh.color = new Color(255, 255, 255);
        BadgetTextMesh.color = new Color(255, 255, 255);
    }

    void AvatarTab()
    {
        indexTab = 3;
        ChipBtn.SetActive(false);
        XenBtn.SetActive(false);
        AvatarBtn.SetActive(true);
        ItemsBtn.SetActive(false);
        BadgetBtn.SetActive(false);

        ChipTabContent.SetActive(false);
        XenTabContent.SetActive(false);
        AvatarTabContent.SetActive(true);
        ItemsTabContent.SetActive(false);
        BadgetTabContent.SetActive(false);

        AvatarTextMesh.color = new Color32(133, 17, 17, 255);
        ChipTextMesh.color = new Color(255, 255, 255);
        XenTextMesh.color = new Color(255, 255, 255);
        ItemsTextMesh.color = new Color(255, 255, 255);
        BadgetTextMesh.color = new Color(255, 255, 255);
    }

    void ItemsTab()
    {
        indexTab = 4;
        ChipBtn.SetActive(false);
        XenBtn.SetActive(false);
        AvatarBtn.SetActive(false);
        ItemsBtn.SetActive(true);
        BadgetBtn.SetActive(false);

        ChipTabContent.SetActive(false);
        XenTabContent.SetActive(false);
        AvatarTabContent.SetActive(false);
        ItemsTabContent.SetActive(true);
        BadgetTabContent.SetActive(false);

        ItemsTextMesh.color = new Color32(133, 17, 17, 255);
        ChipTextMesh.color = new Color(255, 255, 255);
        AvatarTextMesh.color = new Color(255, 255, 255);
        XenTextMesh.color = new Color(255, 255, 255);
        BadgetTextMesh.color = new Color(255, 255, 255);
    }

    void BadgetTab()
    {
        indexTab = 5;
        ChipBtn.SetActive(false);
        XenBtn.SetActive(false);
        AvatarBtn.SetActive(false);
        ItemsBtn.SetActive(false);
        BadgetBtn.SetActive(true);

        ChipTabContent.SetActive(false);
        XenTabContent.SetActive(false);
        AvatarTabContent.SetActive(false);
        ItemsTabContent.SetActive(false);
        BadgetTabContent.SetActive(true);

        BadgetTextMesh.color = new Color32(133, 17, 17, 255);
        ChipTextMesh.color = new Color(255, 255, 255);
        AvatarTextMesh.color = new Color(255, 255, 255);
        ItemsTextMesh.color = new Color(255, 255, 255);
        XenTextMesh.color = new Color(255, 255, 255);
    }

    void getListCardCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            int i = 0;
            List<CardModel> card = LamaControllib.getInstance().getListCard();
            listCard = card;
            //foreach (CardModel temp in card)
            //{
            //    listCard[i].Card = temp.Card;
            //    listCard[i].Chip = temp.Chip;
            //    listCard[i].Xen = temp.Xen;
            //    i++;
            //}
        }
    }

    void getListSmsCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            int i = 0;

            List<SMSModel> sms = LamaControllib.getInstance().getListSMS();
            listSMS = sms;
            //foreach (SMSModel temp in sms)
            //{
            //    listSMS[i].SMS = temp.SMS;
            //    listSMS[i].Chip = temp.Chip;
            //    listSMS[i].Xen = temp.Xen;
            //    i++;
            //}
        }
    }
    public void CloseDeposit()
    {
        transform.parent.gameObject.transform.parent.gameObject.SendMessage("CloseDeposit");
        //GameObject.Destroy(gameObject.transform.parent.gameObject);
    }
}
