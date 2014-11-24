using UnityEngine;
using System.Collections;
using WyrmTale;
using Assets.Script;
using System.Collections.Generic;
public class Gift : MonoBehaviour {

    public int sizeWidth = 8;
    public int sizeHeight = 3;
   // public int sizeGift = 50;

    public GameObject gift;
    public Vector2 FirstPos = new Vector2(-0.7f, -0.1f);
    public float BottomPos = -0.6f;

    public string url = "";
    private Texture[] imgGift;
    //json
    private IDictionary results;
    WWW www;

  //  public List<InfoGift> listGift = new List<InfoGift>();

    private int[] arrID;
    private string[] arrName;
    private int[] arrMoney;
    private Sprite[] arrImg;
    private float[] arrScale;
    private int sizeGift;
    //
    public float sizeIma;
	void Start () {
       
        
        StartCoroutine(LoadData());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void CloneObj()
    {
        GameObject content = GameObject.Find("ScrollingList/ScrollableArea/Content");
        for (int i = 0; i < sizeGift; i++)
        {
           GameObject tempObj = getItem(i);
           tempObj.transform.parent = content.transform;
           float posX = (i % sizeWidth - (float)(sizeWidth/2)+0.5f ) * (2 * FirstPos.x /(sizeWidth-1));
           float posY = FirstPos.y + ((int)i/sizeWidth)*(BottomPos-FirstPos.y)/sizeHeight;
           tempObj.transform.localPosition = new Vector2(posX,posY);
        }
    }

    GameObject getItem(int index)
    {
        GameObject tempGift = Instantiate(gift) as GameObject;
        tk2dTextMesh txtMoney = (tempGift.transform.Find("txtMoney").gameObject).GetComponent<tk2dTextMesh>();
        txtMoney.text = "$"+arrMoney[index];
        tk2dTextMesh txtName = (tempGift.transform.Find("NameGift").gameObject).GetComponent<tk2dTextMesh>();
        txtMoney.text = arrName[index];
        SpriteRenderer IconGift = (tempGift.transform.Find("Icon").gameObject).GetComponent<SpriteRenderer>();
        IconGift.sprite = arrImg[index];
        IconGift.transform.localScale = new Vector3(arrScale[index], arrScale[index], arrScale[index]); 
        return tempGift;
    }
    void loadIconGift()
    {

    }

    IEnumerator LoadData()
    {
        TextAsset txt = (TextAsset)Resources.Load("json", typeof(TextAsset));
        string txtJson = txt.text;     
        JSON json = new JSON();
        json.serialized = txtJson;
        JSON[] arrJson = json.ToArray<JSON>("item");
        sizeGift= arrJson.Length;
        arrID = new int[sizeGift];
        arrName = new string[sizeGift];
        arrImg = new Sprite[sizeGift];
        arrMoney = new int[sizeGift];
         arrScale= new float[sizeGift];
        for (int i = 0; i < arrJson.Length; i++)
        {
           // arrID[i] =   int.Parse( arrJson[i].ToString("ID"));
            int.TryParse(arrJson[i].ToString("ID"), out  arrID[i]);

            arrName[i] = arrJson[i].ToString("name");

           // arrMoney[i] = int.Parse(arrJson[i].ToString("money"));

            int.TryParse(arrJson[i].ToString("money"), out  arrMoney[i]);

            string urlTexture = arrJson[i].ToString("image");
            Debug.Log("log Nhuan json: name: " + arrName[i] + " - id: " + arrID[i] + " - money: " + arrMoney[i] + " - url: " + urlTexture);
            Texture2D temp;

            www = new WWW(urlTexture);
                yield return www;
                temp = www.texture;
                arrImg[i] = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f), 10f);
                arrScale[i] = sizeIma / temp.width;

                CloneObj();
     
       //     arrImg[i] = loadImage(urlTexture);
            
            
        }
         
        
    }

    Sprite loadImage(string url)
    {
        Sprite sprTemp;
        Texture2D imgGift;
        try
        {
            www = new WWW(url);
            imgGift = www.texture;
            if (imgGift != null)
                sprTemp = Sprite.Create(imgGift, new Rect(0, 0, imgGift.width, imgGift.height), new Vector2(0.5f, 0.5f), 10f);
            else
                return null;
        }
        catch
        {
            return null;
        }

        return sprTemp;
    }
}
