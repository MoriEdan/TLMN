using UnityEngine;
using System.Collections;

public class ScrollProgressBar : MonoBehaviour {

    public tk2dUIScrollbar scrollBar;
    private tk2dUIScrollbar slider;
    public float percent;
    public long minMoneyBet;
    public long maxMoney;

    private tk2dTextMesh minBetText;
    private tk2dTextMesh maxMoneyText;

    bool isClick;

    void Awake()
    {
        Debug.Log("tinhvc var: "+VariableApplication.iPlayerCount);
        if (VariableApplication.isQuick)
        {
            minMoneyBet = Utilities.GetMoneyQuick() * 1000;
        }
        else
        {
            minMoneyBet = Utilities.GetMoneyFriend() * 1000;
        }
        float value = ((float)minMoneyBet / (float)1000000);
        if (value >= 0.6)
        {
            scrollBar.Value = (value);
        }
        else
        {
            scrollBar.Value = (value - 0.1f);
        }
    }

	// Use this for initialization
	void Start () {
        slider = GameObject.Find("Slider").GetComponent<tk2dUIScrollbar>();
        minBetText = GameObject.Find("phongchoi_text/tiencuoc_text").GetComponent<tk2dTextMesh>();
        maxMoneyText = GameObject.Find("phongchoi_text/tienvaophong_text").GetComponent<tk2dTextMesh>();

        maxMoney = minMoneyBet * 10;
        minBetText.text = Utilities.ConvertMoney(minMoneyBet);
        maxMoneyText.text = Utilities.ConvertMoney(maxMoney);
	}
	
	// Update is called once per frame
	void Update () {
       // Debug.Log("update");
        progressing();
	}

    void progressing()
    {
        percent = scrollBar.Value * 9;
        slider.Value = scrollBar.Value;
        for (int i = 0; i <= 9; i++)
        {
            if (percent > (i - 0.55) && percent <= (i + 1.1))
            {
                float per = (float)(i / 9D);
                long addBet = (long)(per * 1000000);

                minMoneyBet = 100000 + i * 100000;
                maxMoney = minMoneyBet * 10;
                minBetText.text = Utilities.ConvertMoney(minMoneyBet);
                maxMoneyText.text = Utilities.ConvertMoney(maxMoney);
                scrollBar.Value = per;

                if (VariableApplication.isQuick)
                {
                    Utilities.SetMoneyQuick((int)(minMoneyBet / 1000));
                }
                else
                {
                    Utilities.SetMoneyFriend((int)(minMoneyBet / 1000));
                }
            }
        }
    }

    void OnDestroy()
    {
        if (VariableApplication.isQuick)
        {
            Utilities.SetMoneyQuick((int)(minMoneyBet / 1000));
        }
        else
        {
            Utilities.SetMoneyFriend((int)(minMoneyBet / 1000));
        }
    }

}
