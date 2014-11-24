using UnityEngine;
using System.Collections;

public class pro5controller : MonoBehaviour {
    ArrayList arrBaiDepNhat;
    GameObject objAddFr, objReport, objBoxAddFr, objBoxReport;
    bool fBoxAddFr, fBoxReport;
	// Use this for initialization
	void Start () {

        objAddFr = GameObject.Find("addFriend");
        objReport = GameObject.Find("report");
        objBoxAddFr = GameObject.Find("BoxAddFriend");
        objBoxReport = GameObject.Find("BoxReport");
        fBoxAddFr = false;
        fBoxReport = false;

        arrBaiDepNhat = new ArrayList { "1", "2", "3" };
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddFriend()
    {
        Debug.Log("add friend");
        showBoxAddFr();

    }

    void Report()
    {
        Debug.Log("add report");
        showBoxReport();
    }

    void Gift()
    {
        Debug.Log("Tặng quà!");
    }

    void EditName()
    {
        Debug.Log("Edit Name User!");
    }

    void GiftQuick()
    {
        Debug.Log("Tặng quà tại bàn!");
    }

    void Change_avatar()
    {
        Debug.Log("Thay avatar!");
    }

    void Up_level()
    {
        Debug.Log("Nâng hạng!");
    }
    void showBoxAddFr()
    {
        
        if (!fBoxAddFr)
        {
            objBoxAddFr.GetComponent<SpriteRenderer>().sortingOrder = 1;
            objBoxAddFr.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
            objBoxAddFr.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2;

            objBoxAddFr.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            objBoxAddFr.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;

            fBoxAddFr = !fBoxAddFr;
        }
        else
        {
            objBoxAddFr.GetComponent<SpriteRenderer>().sortingOrder = -1;
            objBoxAddFr.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -1;
            objBoxAddFr.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;

            objBoxAddFr.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            objBoxAddFr.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;

            fBoxAddFr = !fBoxAddFr;
        }
    }

    void showBoxReport()
    {
        if (!fBoxReport)
        {
            objBoxReport.GetComponent<SpriteRenderer>().sortingOrder = 1;
            objBoxReport.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
            objBoxReport.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2;
            
            objBoxReport.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            objBoxReport.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;

            fBoxReport = !fBoxReport;
        }
        else
        {
            objBoxReport.GetComponent<SpriteRenderer>().sortingOrder = -1;
            objBoxReport.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -1;
            objBoxReport.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;

            objBoxReport.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            objBoxReport.transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;

            fBoxReport = !fBoxReport;
        }
    }
}
