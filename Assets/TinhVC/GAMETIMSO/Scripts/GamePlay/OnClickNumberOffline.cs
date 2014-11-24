using UnityEngine;
using System.Collections;

public class OnClickNumberOffline : MonoBehaviour {

    public AudioClip clickCorrect;
    public AudioClip clickWrong;
     GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        int number = int.Parse(transform.GetComponentInChildren<UILabel>().text);
        if (number == OfflineController.level)
        {
            Debug.Log("tinhvc name: " + transform.gameObject.name);
            transform.parent.gameObject.SendMessage("SendNumberMsgOffline");
        }
        else
        {
            audio.clip = clickWrong;
            audio.Play();
        }
    }
}
