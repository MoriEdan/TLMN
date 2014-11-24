using UnityEngine;
using System.Collections;

public class ButtonChip : MonoBehaviour {

    GameObject CardTabContentScrollView;
    TabController chipTabControll;
	// Use this for initialization
	void Start () {
        chipTabControll = GameObject.Find("ChipTabController").GetComponent<TabController>();
        //CardTabContentScrollView = GameObject.Find("ChipTabContent/Card/CardTabContentScrollView");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void ButtonClick()
    {
        switch (chipTabControll.tabIndex)
        {
            case 1:
                CardTabContentScrollView = GameObject.Find("ChipTabContent/Card/CardTabContentScrollView");
                CardTabContentScrollView.SetActive(false);
                break;

            case 2:

                string nameParent = gameObject.transform.parent.name;
                string index = nameParent.Substring(4, nameParent.Length -4);

                ChipUpdateActive.indexListSMS = int.Parse(index);

                CardTabContentScrollView = GameObject.Find("ChipTabContent/SMS/SMSTabContentScrollView");
                CardTabContentScrollView.SetActive(false);
                break;

        }
        //if (CardTabContentScrollView != null )
        //{
        //    CardTabContentScrollView.SetActive(false);
        //    //CardTabContent.SetActive(true);
        //    //Debug.Log(CardTabContent.activeSelf);
        //    Debug.Log(gameObject.transform.parent.FindChild("money").GetComponent<tk2dTextMesh>().text);
        //}

    }
}
