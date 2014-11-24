using UnityEngine;
using System.Collections;

public class ClosePopupError : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClickClose()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
