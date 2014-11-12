using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {

    private float rotationSpeed;
    public tk2dTextMesh txtTime;
    public float time;

    private bool isRotate;

	// Use this for initialization
	void Awake () {
        time = Constants.COUNT_DOWN_TIME_PER_TURN;
        rotationSpeed = 360 / Constants.COUNT_DOWN_TIME_PER_TURN;
        isRotate = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(isRotate)
        {
            if (time <= 0f)
            {
                time = Constants.COUNT_DOWN_TIME_PER_TURN;
                //SmartFoxConnection.SendPassToServer(SFSGameRoom.order);
            }
            else
            {
                transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
                time = time - Time.deltaTime;
            }
            //txtTime.text = Mathf.RoundToInt(time).ToString();
        }
	}
    public bool IsRotate
    {
        get { return isRotate; }
        set { isRotate = value; }
    }
}
