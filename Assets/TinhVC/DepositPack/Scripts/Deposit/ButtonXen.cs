using UnityEngine;
using System.Collections;

public class ButtonXen : MonoBehaviour {

    GameObject CardTabContentScrollView;
    TabController xenTabControll;
    // Use this for initialization
    void Start()
    {
        xenTabControll = GameObject.Find("XenTabController").GetComponent<TabController>();
        //CardTabContentScrollView = GameObject.Find("XenTabContent/Card/XenCardTabContentScrollView");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //GameObject tvChip1;
    //GameObject tvChip2;
    //GameObject tvXen1;
    //GameObject tvXen2;
    //tinhvc click send sms
    void ButtonClick()
    {
        switch (xenTabControll.tabIndex)
        {
            case 1:
                CardTabContentScrollView = GameObject.Find("XenTabContent/Card/XenCardTabContentScrollView");
                CardTabContentScrollView.SetActive(false);
                Debug.Log(gameObject.transform.parent.FindChild("moneyXen").GetComponent<tk2dTextMesh>().text);
                Debug.Log(gameObject.transform.parent.FindChild("prizeXen").GetComponent<tk2dTextMesh>().text);
                break;

            case 2:

                string nameParent = gameObject.transform.parent.name;
                string index = nameParent.Substring(4, nameParent.Length -4);
                XenUpdateActive.indexListSMS = int.Parse(index);

                CardTabContentScrollView = GameObject.Find("XenTabContent/SMS/XenSMSTabContentScrollView");
                CardTabContentScrollView.SetActive(false);
                Debug.Log(gameObject.transform.parent.FindChild("moneyXen").GetComponent<tk2dTextMesh>().text);
                Debug.Log(gameObject.transform.parent.FindChild("prizeXen").GetComponent<tk2dTextMesh>().text);

                //string  xenValue= gameObject.transform.parent.FindChild("moneyXen").GetComponent<tk2dTextMesh>().text;

                //tvXen1 = GameObject.FindGameObjectWithTag("tvXen1") as GameObject;
                //tvXen1.GetComponent<tk2dTextMesh>().text = "Bạn chọn nạp " + xenValue + " XEN bằng SMS. Chọn \"đồng ý\" và gửi tin nhắn mua XEN.";

                break;

        }
        //if (CardTabContentScrollView != null)
        //{
        //    CardTabContentScrollView.SetActive(false);
        //    //CardTabContent.SetActive(true);
        //    //Debug.Log(CardTabContent.activeSelf);
        //    Debug.Log(gameObject.transform.parent.FindChild("money").GetComponent<tk2dTextMesh>().text);
        //}

    }
}
