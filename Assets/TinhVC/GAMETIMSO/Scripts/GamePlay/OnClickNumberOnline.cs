using UnityEngine;
using System.Collections;

public class OnClickNumberOnline : MonoBehaviour {

    public AudioClip clickCorrect;
    public AudioClip clickWrong;
    GameObject player;
    //public GameObject btnNumber;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("mPlayer") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick() {
       // Debug.Log("tinhvc click");
        player.SendMessage("SendCloseBoxExitMsg", SendMessageOptions.RequireReceiver);
        if(!VariableApplication.isViewer){
            int number = int.Parse(transform.GetComponentInChildren<UILabel>().text);
            if (number == CloneNumberController.level)
            {
                CloneNumberController.nextLocation = int.Parse(gameObject.name);
                Debug.Log("tinhvc: "+player.name);
                player.SendMessage("SendNumberMsg", SendMessageOptions.RequireReceiver);
            }
            else
            {
                audio.clip = clickWrong;
                audio.Play();
            }
        }
        
    }
}
