using UnityEngine;
using System.Collections;

public class OfflineController : MonoBehaviour {

    public GameObject playerObj;

    public GameObject player11;

    public GameObject player2;
    public GameObject player22;
    public GameObject playerTimeDown;
    public GameObject nextNumberFind;

    public GameObject buttonNumber;

    public GameObject playerPoint;

    public GameObject WinLabel;

    private GameObject[] listButtonClone = new GameObject[119];

    public static int level = 1;

    public AudioClip clickCorrect;
    public AudioClip clickWrong;
    public AudioClip audioWin;
    public AudioClip audioClose;
    public AudioClip timeDowns;

	// Use this for initialization
	void Start () {
        if (VariableApplication.isOffline)
        {
            int[] texts = new int[99];
            for (int i = 0; i < 99; i++)
            {
                texts[i] = i + 1;
            }
            texts = Utilities.reshuffle(texts);
            CloneNumber(texts);
            initShow();
            level = 1;
        }
	}
    public float xOld1;
    public float yOld1;
    public float xOld2;
    public float yOld2;
    void initShow()
    {
        player11.SetActive(false);
        player2.SetActive(false);
        player22.SetActive(false);
        playerTimeDown.SetActive(true);

        xOld1 = playerObj.transform.localPosition.x;
        yOld1 = playerObj.transform.localPosition.y;
        xOld2 = playerTimeDown.transform.localPosition.x;
        yOld2 = playerTimeDown.transform.localPosition.y;
        playerObj.transform.localPosition = new Vector3(xOld1 + 25, yOld1, 0);
        playerTimeDown.transform.localPosition = new Vector3(xOld2 + 20, yOld2, 0);
    }

    private int[] listNumbers = new int[99];
    void CloneNumber(int[] texts)
    {
        //Debug.Log("size list number: "+texts.Length);
        listNumbers = texts;
        float countY = -128;
        int countX = 0;

        float xFinal = 0;
        float yFinal = 0;

        int countNumber = 1;
        for (int i = 1; i <= 120; i++)
        {
            countX++;

            if (i % 17 == 1)
            {
                countY = countY + 28;
                countX = 1;
            }

            if (i == 5 || i == 9 || i == 19 || i == 31 || i == 41 || i == 71 || i == 79)
            {
                GameObject clone = Instantiate(buttonNumber, transform.position, transform.rotation) as GameObject;
                clone.transform.parent = transform;

                float x = -270 + (31) * countX;
                float y = countY + 10;

                if (texts[countNumber - 1] < 10)
                {
                    if (i == 79)
                    {
                        setPosition(clone, x - 15, y - 20, 30, 32, UIWidget.Pivot.Top);
                    }
                    else
                    {
                        setPosition(clone, x, y, 36, 34, UIWidget.Pivot.Center);
                    }
                }
                else
                {
                    if (i == 79)
                    {
                        setPosition(clone, x - 13, y - 17, 30, 32, UIWidget.Pivot.Top);
                    }
                    else
                    {
                        setPosition(clone, x, y, 30, 32, UIWidget.Pivot.Top);
                    }
                }

                if (i == 79)
                {
                    xFinal = x;
                    yFinal = y;

                    UISprite spriteNumber = clone.GetComponentInChildren<UISprite>();
                    spriteNumber.width = 25;
                    spriteNumber.height = 25;
                }
                else
                {
                    UISprite spriteNumber = clone.GetComponentInChildren<UISprite>();
                    spriteNumber.width = 50;
                    spriteNumber.height = 50;
                }
                setProperty(clone, texts[countNumber - 1], true);
                countNumber++;
            }
            else
            {
                if (i != 5 && i != 6 && i != 9 && i != 10 && i != 19 && i != 20 && i != 22 && i != 23 && i != 26 && i != 27 && i != 31 && i != 32 && i != 36 && i != 37 && i != 41 && i != 42 && i != 48 && i != 49 && i != 58 && i != 59 && i != 71 && i != 72 && i != 79 && i != 80 && i != 88 && i != 89 && i != 96 && i != 97 && i != 120)
                {
                    GameObject clone = Instantiate(buttonNumber, transform.position, transform.rotation) as GameObject;
                    clone.transform.parent = transform;
                    clone.transform.localPosition = new Vector3(-280 + (31) * countX, countY, 0);
                    UILabel cloneLabel = clone.GetComponentInChildren<UILabel>();
                    cloneLabel.pivot = UIWidget.Pivot.Center;

                    setProperty(clone, texts[countNumber - 1], false);

                    countNumber++;
                }
            }

            if (i == 120)
            {
                GameObject clone = Instantiate(buttonNumber, transform.position, transform.rotation) as GameObject;
                clone.transform.parent = transform;

                if (texts[countNumber - 1] < 10)
                {
                    setPosition(clone, xFinal + 17, yFinal + 13, 36, 34, UIWidget.Pivot.Center);
                }
                else
                {
                    setPosition(clone, xFinal + 17, yFinal + 13, 30, 32, UIWidget.Pivot.Top);
                }
                setProperty(clone, texts[countNumber - 1], true);
            }

        }
        startTimeDown();
    }

    void setProperty(GameObject clone, int number, bool isSetBg)
    {

        clone.GetComponentInChildren<UILabel>().text = "" + number;
        clone.transform.localScale = new Vector3(1, 1, 1);
        clone.name = "" + number;

        GameObject bgNumber = clone.transform.GetChild(0).gameObject;
        if (isSetBg)
        {
            bgNumber.transform.localPosition = new Vector3(9, 3, 0);
        }
        bgNumber.GetComponent<UIWidget>().depth = 45;

        listButtonClone[number] = clone;
    }

    void setPosition(GameObject clone, float x, float y, int w, int h, UIWidget.Pivot pivot)
    {
        UILabel cloneLabel = clone.GetComponentInChildren<UILabel>();
        clone.transform.localPosition = new Vector3(x, y, 0);
        cloneLabel.width = w;
        cloneLabel.height = h;
        cloneLabel.pivot = pivot;

    }

	// Update is called once per frame
	void Update () {
	
	}

    bool enableCLick = true;

    void SendNumberMsgOffline()
    {
        if (enableCLick) 
        {

            UISprite spriteNumber = listButtonClone[level].GetComponentInChildren<UISprite>();
            spriteNumber.alpha = 255;
            spriteNumber.spriteName = "number_selected";
            playerPoint.GetComponent<UILabel>().text = "" + level * 18;
            nextNumberFind.GetComponent<UILabel>().text = ""+(level + 1);
            audio.Stop();
            audio.clip = clickCorrect;
            audio.Play();
            secondDown = 10;
            if (level == 99)
            {
                audio.clip = audioWin;
                audio.Play();
                stopTimeDown();
                //WinLabel.GetComponent<UILabel>().text = "THẮNG RẦU!";
                setWinAnimation(true);
            }

            if (level < 99)
            {
                level++;
            }
        }
        
    }

    public void startTimeDown()
    {
        StartCoroutine(TimeDownMethod());
    }

    public void stopTimeDown()
    {
        StopCoroutine(TimeDownMethod());
    }

    public static bool isCountDown = true;
    public static int secondDown = 10;
    public IEnumerator TimeDownMethod()
    {
        if (secondDown > 0)
        {
            yield return new WaitForSeconds(1);
            playerTimeDown.GetComponentInChildren<UILabel>().text = ("" + secondDown);

            if (isCountDown)
            {
                StartCoroutine(TimeDownMethod());
                secondDown--;
            }

            if (secondDown == 3)
            {
                audio.clip = timeDowns;
                audio.Play();
            }

            if (secondDown == 9)
            {
                
            }
        }
        else
        {
            enableCLick = false;
            audio.clip = audioClose;
            audio.Play();
            stopTimeDown();
            secondDown = 10;
            //WinLabel.GetComponent<UILabel>().text = "THUA RẦU!";
            setWinAnimation(false);
            int pointCur = int.Parse(playerPoint.GetComponent<UILabel>().text);
            PlayerPrefs.SetInt("yourScore", pointCur);

            int hightCore = PlayerPrefs.GetInt("hightCore", 0);
            if (pointCur > hightCore)
            {
                PlayerPrefs.SetInt("hightCore", (pointCur));
            }
            PlayerPrefs.SetInt("is_win", 0);

            PlayerPrefs.SetInt("level_finish", level - 1);

            level = 1;
        }
    }

    public GameObject panelWin;
    public GameObject animationObj;
    public GameObject spriteWin;
    void setWinAnimation(bool isWin)
    {
        UISprite _spriteWin = spriteWin.GetComponent<UISprite>();
        if (isWin)
        {
            Debug.Log("tinhvc thang");
            _spriteWin.spriteName = "win_sprite";
            _spriteWin.MarkAsChanged();
        }
        else
        {
            Debug.Log("tinhvc thua");
            _spriteWin.spriteName = "close_sprite";
            _spriteWin.MarkAsChanged();
        }
        panelWin.GetComponent<UIPanel>().alpha = 1;
        animationObj.SetActive(true);
    }
}
