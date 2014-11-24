using UnityEngine;
using System.Collections;

public class demoaaa : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LamaControllib.getInstance().getListCardService("2", callBackGetAllMoney, this);
	}

   void callBackGetAllMoney(bool isSuccess, JSON jsonResult)
{
    Debug.Log("tinhvc dsdasdksak: "+jsonResult.serialized);

}
	
	// Update is called once per frame
	void Update () {
	
	}
}
