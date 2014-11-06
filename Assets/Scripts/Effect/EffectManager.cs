using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EffectManager : MonoBehaviour {

    public Effect firstBig;
    public Effect firstSmall;
    public Effect secondBig;
    public Effect secondSmall;
    public Effect thirdBig;
    public Effect thirdSmall;
    public Effect fourthBig;
    public Effect fourthSmall;

    public Effect superWin;

    public GameObject imgFirst;
    public GameObject imgSecond;
    public GameObject imgThird;
    public GameObject imgFourth;
    public GameObject imgFirst1;
    public GameObject imgSecond1;
    public GameObject imgThird1;
    public GameObject imgFourth1;


    public static List<Effect> listEffect;
    public static List<GameObject> listImg;
	// Use this for initialization
	void Start () {
        listEffect = new List<Effect>();
        listEffect.Add(firstBig);
        listEffect.Add(firstSmall);
        listEffect.Add(secondBig);
        listEffect.Add(secondSmall);
        listEffect.Add(thirdBig);
        listEffect.Add(thirdSmall);
        listEffect.Add(fourthBig);
        listEffect.Add(fourthSmall);
        listEffect.Add(superWin);

        listImg = new List<GameObject>();
        listImg.Add(imgFirst1);
        listImg.Add(imgFirst);
        listImg.Add(imgSecond1);
        listImg.Add(imgSecond);
        listImg.Add(imgThird1);
        listImg.Add(imgThird);
        listImg.Add(imgFourth1);
        listImg.Add(imgFourth);
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static Effect ShowEffect(int effectIndex, Vector3 position, float time)
    {
        Effect effectInstance = Instantiate(listEffect[effectIndex], position, Quaternion.identity) as Effect;
        effectInstance.TimeDisplay = time;
        return effectInstance;
    }

    public static GameObject ShowImage(int imgIndex, Vector3 position)
    {
        GameObject imgInstance = Instantiate(listImg[imgIndex], position, Quaternion.identity) as GameObject;
        return imgInstance;
    }
}
