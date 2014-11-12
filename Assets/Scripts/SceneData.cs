using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneData : MonoBehaviour {

    private Vector3[][] playerCardPosition;
  
    private Vector3[] putCardPosition;
    private Vector3 screenCenter;

    

    private float scaleW;
    private float scaleH;

    private List<HumanPlayer> humanPlayerPrefabs;

    public HumanPlayer player1Prefab;
    public HumanPlayer player2Prefab;
    public HumanPlayer player3Prefab;
    public HumanPlayer player4Prefab;

    private List<Vector3> playerPositions;

    public tk2dCamera tk2DCmr;
    
	// Use this for initialization
	void Awake () {

        // Scale
        scaleW = (float)Screen.width / 1280;
        scaleH = (float)Screen.height / 720;

        

        // Create 13 position
        CreatePlayerCardPosition();

        // Create position to put card - 4 position (for display previous card)
        
        screenCenter = tk2DCmr.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5));
        CreatePutCardPosition();

        // Add humanplayer Prefabs to list
        humanPlayerPrefabs = new List<HumanPlayer>();
        humanPlayerPrefabs.Add(player1Prefab);
        humanPlayerPrefabs.Add(player2Prefab);
        humanPlayerPrefabs.Add(player3Prefab);
        humanPlayerPrefabs.Add(player4Prefab);

        playerPositions = new List<Vector3>();
        CreatePlayerPosition();
	}

    public void CreatePlayerPosition()
    {
        playerPositions.Clear();
        playerPositions.Add(tk2DCmr.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 4, 5)));
        playerPositions.Add(tk2DCmr.camera.ScreenToWorldPoint(new Vector3(Screen.width - (100 * scaleW), Screen.height / 2, 5)));
        playerPositions.Add(tk2DCmr.camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - (100 * scaleH), 5)));
        playerPositions.Add(tk2DCmr.camera.ScreenToWorldPoint(new Vector3((100 * scaleW), Screen.height / 2, 5)));
    }

    public void CreatePlayerCardPosition()
    {
        playerCardPosition = new Vector3[Constants.MAX_NUMBER_PLAYER][];
        for (int i = 0; i < Constants.MAX_NUMBER_PLAYER; i++)
        {
            playerCardPosition[i] = new Vector3[Constants.CARD_AMOUNT_FOR_EACH_PLAYER];
        }
        for (int i = 0; i < Constants.CARD_AMOUNT_FOR_EACH_PLAYER; i++)
        {
            playerCardPosition[0][i] = tk2DCmr.camera.ScreenToWorldPoint(new Vector3(200 * scaleW, 100 * scaleH, 5))
                + new Vector3(i * Constants.CARD_MARGIN_HORIZONTAL, 0, 0);
            playerCardPosition[1][i] = tk2DCmr.camera.ScreenToWorldPoint(new Vector3(Screen.width - 100 * scaleW, 100 * scaleH, 5))
                + new Vector3(0, i * Constants.CARD_MARGIN_VERTICAL, 0);
            playerCardPosition[2][i] = tk2DCmr.camera.ScreenToWorldPoint(new Vector3(Screen.width - 300 * scaleW, Screen.height - 100 * scaleH, 5))
                - new Vector3(i * Constants.CARD_MARGIN_HORIZONTAL, 0, 0);
            playerCardPosition[3][i] = tk2DCmr.camera.ScreenToWorldPoint(new Vector3(100 * scaleW, Screen.height - 100 * scaleH, 5))
                - new Vector3(0, i * Constants.CARD_MARGIN_VERTICAL, 0);
        }
    }

    public void CreatePutCardPosition()
    {
        putCardPosition = new Vector3[Constants.NUMBER_POSITION_TO_PUT_CARD];
        putCardPosition[0] = screenCenter + new Vector3(-0.5f, -0.5f, 0);
        putCardPosition[1] = screenCenter + new Vector3(-0.5f, 0.5f, 0);
        putCardPosition[2] = screenCenter + new Vector3(0.5f, 0.5f, 0);
        putCardPosition[3] = screenCenter + new Vector3(0.5f, -0.5f, 0);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Get & Set
    public Vector3[][] PlayerCardPosition
    {
        get { return playerCardPosition; }
        set { playerCardPosition = value; }
    }
    public float ScaleW
    {
        get { return scaleW; }
        set { scaleW = value; }
    }
    public float ScaleH
    {
        get { return scaleH; }
        set { scaleH = value; }
    }
    public Vector3[] PutCardPosition
    {
        get { return putCardPosition; }
        set { putCardPosition = value; }
    }

    public List<HumanPlayer> HumanPlayerPrefabs
    {
        get { return humanPlayerPrefabs; }
        set { humanPlayerPrefabs = value; }
    }
    public List<Vector3> PlayerPositions
    {
        get { return playerPositions; }
        set { playerPositions = value; }
    }
    public Vector3 ScreenCenter
    {
        get { return screenCenter; }
        set { screenCenter = value; }
    }
}
