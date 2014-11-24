using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

    private GameObject buttonRoomType;
    private GameObject buttonRoomTime;
    Vector2 posRoomType;
    Vector2 posRoomTime;
    public tk2dTextMesh time_text;
    public tk2dTextMesh type_text;
    int time = 0;
    int type = 0;

	void Start () {
        time_text.text = "Nhanh";
        type_text.text = "Tự do";
        buttonRoomType = GameObject.Find("Alarm/button1/typeroom");
        posRoomType = buttonRoomType.transform.position;
        buttonRoomTime = GameObject.Find("Alarm/button2/timeroom");
        posRoomTime = buttonRoomTime.transform.position;
        
        setTypeDefault();
	}

    void setTypeDefault()
    {
        type = Utilities.GetTypeRoom();
        setItween(type);
        time = Utilities.GetSpeedRoom();
        setItween1(time);
    }

    void setItween(int type)
    {
        if (type == 1)
        {
            iTween.MoveTo(buttonRoomType, posRoomType - new Vector2(1.6f, 0), 1);
            type_text.text = "Riêng";
        }
        else
        {
            iTween.MoveTo(buttonRoomType, posRoomType, 1);
            type_text.text = "Tự do";
        }
    }

    void setItween1(int time)
    {
        if (time == 1)
        {
            iTween.MoveTo(buttonRoomTime, posRoomTime + new Vector2(1.6f, 0), 1);
            time_text.text = "Chậm";
        }
        else
        {
            iTween.MoveTo(buttonRoomTime, posRoomTime, 1);
            time_text.text = "Nhanh";
        }
    }

	// Update is called once per frame
	void Update () {
      
       
	}
    void ClickRoomType() {
        if (type == 0)
        {
            iTween.MoveTo(buttonRoomType, posRoomType - new Vector2(1.6f, 0), 1);
            type = 1;
            type_text.text = "Riêng";
        }
        else
        {
            iTween.MoveTo(buttonRoomType, posRoomType , 1);
            type = 0;
            type_text.text = "Tự do";
        }
        Utilities.SetTypeRoom(type);
    }

    void ClickRoomTime()
    {
        if (time == 0)
        {
            iTween.MoveTo(buttonRoomTime, posRoomTime + new Vector2(1.6f, 0), 1);
            time = 1;
            time_text.text = "Chậm";
        }
        else
        {
            iTween.MoveTo(buttonRoomTime, posRoomTime , 1);
            time = 0;
            time_text.text = "Nhanh";
        }
        Utilities.SetSpeedRoom(time);
    }
}
