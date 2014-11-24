using UnityEngine;
using System.Collections;

public class SettingClick : MonoBehaviour {

    public GameObject controlpanel;
    public bool isActive;

	// Use this for initialization
	void Start () {
        //controlpanel = GameObject.Find("controlpanel");
        isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp (){
        if (!isActive)
        {
            controlpanel.SetActive(true);
            isActive = true;
        }
        else {
            controlpanel.SetActive(false);
            isActive = false;
        }
    }
}
