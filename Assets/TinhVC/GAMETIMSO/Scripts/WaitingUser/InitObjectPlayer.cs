using UnityEngine;
using System.Collections;

public class InitObjectPlayer : MonoBehaviour {


   public GameObject waitingTwoPlayout;
   public GameObject waitingFourPlayout;

	// Use this for initialization
	void Start () {

        if (!VariableApplication.isOffline)
        {
            if (VariableApplication.iPlayerCount == 2)
            {
                Hashtable args1 = new Hashtable();
                args1.Add("position", new Vector3(0, 0, 0));
                args1.Add("time", 0.5);
                iTween.MoveTo(waitingTwoPlayout, args1);
            }
            else if (VariableApplication.iPlayerCount == 4)
            {
                Hashtable args2 = new Hashtable();
                args2.Add("position", new Vector3(0, 0, 0));
                args2.Add("time", 0.5);
                iTween.MoveTo(waitingFourPlayout, args2);
            }
        }
	}
	
    

	// Update is called once per frame
	void Update () {
	
	}



}
