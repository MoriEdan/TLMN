using UnityEngine;
using System.Collections;

public class SMSModel {

    private string sms = "";
    private string xen = "";
    private string chip = "";
    private string sendNUmber = "";

    public string SMS
    {
        get { return sms; }
        set { sms = value; }
    }

    public string Xen
    {
        get { return xen; }
        set { xen = value; }
    }

    public string Chip
    {
        get { return chip; }
        set { chip = value; }
    }

    public string SendNUmber
    {
        get { return sendNUmber; }
        set { sendNUmber = value; }
    }

}
