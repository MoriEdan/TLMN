using UnityEngine;
using System.Collections;

public class SettingOff : MonoBehaviour {

	// Use this for initialization
    public GameObject panel;
    public SettingClick setting;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp()
    {
        panel.SetActive(false);
        setting.isActive = false;
    }
}
