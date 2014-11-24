using UnityEngine;
using System.Collections;

public class DialogButtonControl : MonoBehaviour {

    public GameObject dialogForgetPass;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OffDialog()
    {
        iTween.MoveTo(dialogForgetPass, new Vector3(0, 7.5f, -3), 1);
    }
}
