using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
//using Assets.Scripts.PlayScene;

/* 
public class SFGameRoom : MonoBehaviour
{
    //public AudioClip clickWrong;
    public static bool isBotFeature;
    public static byte clicked_pos;
    //
    public static byte next_number;
    // number button store in prefabs
    //public GameObject buttonNumber;
    // array store generated button
    private GameObject[] listButtonClone = new GameObject[99];

    // SMARTFOX COMPONENT
    private SmartFox sfConnectObj;
    private SFSObject receivedData;

    // UI COMPONENT
    // player point
    private GameObject point;
    // player's opponent point
    private GameObject point_opn;
    // first status when player enter room (include: waiting.., game start in...)
    private GameObject status;
    // next number
    private GameObject nextNumber;
    // number of seconds countdown before game start
    private int secondDownMulti = 5;
    //
    private int score = 0;
    private int opn_score = 0;
    // indicate this player is host or not
    public static bool isHost;
    //
    public static bool isStarted;

    //---------------------------------------//

    GameObject player1;
    GameObject player2;
    public ElephantBehaviour elephantPrefab;
    public BirdBehaviour bird;
    private float timer;
    private float playTime;
    private Rect timeRect;
    private Rect player1ScoreRect;
    private Rect player2ScoreRect;

    public ItemBehaviour iShield;
    public ItemBehaviour iCoco;
    public ItemBehaviour iBoot;
    public ItemBehaviour iSword;

    public static bool isBootPlayer1;
    public static bool isBootPlayer2;
    public static bool isSwordPlayer1;
    public static bool isSwordPlayer2;

    private GameObject gameTime;
    private GameObject player1Score;
    private GameObject player2Score;

    // modal dialog
    private GameObject modalDialog;

    // Text in game
    private GameObject text;
    public Sprite waiting;
    public Sprite leaveRoom;
    public Sprite noOpponent;
    public Sprite startIn;

    // Dialog
    private GameObject noOpponentDialog;
    private GameObject leaveOpponentDialog;

    void Awake()
    {
        if (SmartFoxConnection.IsInitialized)
        {
            sfConnectObj = SmartFoxConnection.Connection;
        }
        else
        {
            Application.LoadLevel("Setup");
        }

        // Register callback delegate
        sfConnectObj.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfConnectObj.AddEventListener(SFSEvent.OBJECT_MESSAGE, OnObjectMesssage);
        sfConnectObj.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    // Use this for initialization
    void Start()
    {
        isStarted = false;
        isHost = false;
        isBotFeature = false;
        clicked_pos = 0;
        next_number = 1;
        SmartFoxConnection.Connection = sfConnectObj;
        receivedData = SmartFoxConnection.PersistentData;
        status = GameObject.FindGameObjectWithTag("waiting_player") as GameObject;
        text = GameObject.Find("Text");
       

        isHost = receivedData.GetBool("isH");
        //status.GetComponent<UILabel>().text = "Waiting for opponent...";
        //status.GetComponent<tk2dTextMesh>().text = "Waiting for opponent...";
        text.GetComponent<SpriteRenderer>().sprite = waiting;
        

        //------------------------------// Gameplay
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        timer = Constants.TIME_SPAWN_BIRD;
        playTime = SceneData.winTime;
        timeRect = new Rect(Screen.width / 2, Screen.height / 5, 150, 60);
        player1ScoreRect = new Rect(Screen.width / 2 - 100, Screen.height / 5, 100, 40);
        player2ScoreRect = new Rect(Screen.width / 2 + 100, Screen.height / 5, 100, 40);

        isBootPlayer1 = false;
        isBootPlayer2 = false;
        isSwordPlayer1 = false;
        isSwordPlayer2 = false;

        gameTime = GameObject.Find("GameTime");
        gameTime.GetComponent<tk2dTextMesh>().text = SceneData.winTime.ToString();
        //gameTime.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - 200, 5));

        player1Score = GameObject.Find("Player1Score");
        player1Score.GetComponent<tk2dTextMesh>().text = "0";
        player1Score.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(10 * SceneData.scaleW, Screen.height - (20 * SceneData.scaleH), 5));
        
        player2Score = GameObject.Find("Player2Score");
        player2Score.GetComponent<tk2dTextMesh>().text = "0";
        player2Score.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - (50 * SceneData.scaleW), Screen.height - (20 * SceneData.scaleH), 5));

        // Modal dialog
        modalDialog = GameObject.Find("ModalDialog");

        // Wait for 45 seconds
        StartCoroutine(WaitForOpponent(45));

        // DIalog
        noOpponentDialog = GameObject.Find("NoOpponentDialog");
        leaveOpponentDialog = GameObject.Find("OpponentLeaveDialog");
    }

    

    // Update is called once per frame
    void Update()
    {
        // When user tap back key =.="
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SmartFoxConnection.Connection.Send(new LeaveRoomRequest());
            //Application.LoadLevel("Setup");
            iTween.MoveTo(modalDialog, iTween.Hash("y", 0, "time", 1f));
        }
        
        // Set time text, player's score
        gameTime.GetComponent<tk2dTextMesh>().text = Mathf.RoundToInt(playTime).ToString();
        player1Score.GetComponent<tk2dTextMesh>().text = SceneData.scorePlayer1.ToString();
        player2Score.GetComponent<tk2dTextMesh>().text = SceneData.scorePlayer2.ToString();

        if(isHost && isStarted)
        {
            if (timer < 0f)
            {
                //SpawnBird();
                int tempPosition = Random.Range(3, 17);
                //string itemName = Constants.ARRAY_ITEM_NAME[Random.Range(0, 4)];
                int itemName = Random.Range(0, 4);
                int tempLine = Random.Range(0, Constants.LINE);
                DropItem(tempPosition, Constants.ARRAY_ITEM_NAME[itemName], tempLine);
                SmartFoxConnection.SendDropItem((byte)tempPosition, (byte)itemName, (byte)tempLine);
                timer = Constants.TIME_SPAWN_BIRD;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            // Handler for player 1
            Player1Handler();   
        }
        else
        {
            // Handler for player 2
            Player2Handler();
        }
        if (isStarted)
        {
            playTime -= Time.deltaTime;
        }
        
        if (SceneData.scorePlayer1 >= SceneData.winScore)
        {
            Application.LoadLevel("Player1Win");
        }
        if (SceneData.scorePlayer2 >= SceneData.winScore)
        {
            Application.LoadLevel("Player2Win");
        }
        if (playTime < 0f)
        {
            if (SceneData.scorePlayer1 > SceneData.scorePlayer2)
            {
                if(isHost)
                {
                    Application.LoadLevel("OnlineWin");
                }
                else
                {
                    Application.LoadLevel("OnlineLose");
                }
            }
            else if (SceneData.scorePlayer1 < SceneData.scorePlayer2)
            {
                if(isHost)
                {
                    Application.LoadLevel("OnlineLose");
                }
                else
                {
                    Application.LoadLevel("OnlineWin");
                }
            }
            else
            {
                Application.LoadLevel("Draw");
            }
        }
    }

    void FixedUpdate()
    {
        sfConnectObj.ProcessEvents();
    }

    IEnumerator countDown(int time)
    {
        text.GetComponent<SpriteRenderer>().sprite = startIn;
        for (int i = time; i >= 0; i--)
        {
            //status.GetComponent<UILabel>().text = "Game will start in " + i + " seconds...";
            //status.GetComponent<tk2dTextMesh>().text = "Game will start in " + i + " seconds...";
            status.GetComponent<tk2dTextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        status.SetActive(false);
        text.SetActive(false);
        isStarted = true;
        GameManagerBehaviour.isOnlineStarted = false;
    }

    IEnumerator countDownToWin(float time)
    {
       yield return new WaitForSeconds(time);
       //if (isHost)
       //{
       //    Application.LoadLevel("Player1Win");
       //}
       //else
       //{
       //    Application.LoadLevel("Player2Win");
       //}
       Application.LoadLevel("OnlineWin");
    }

    IEnumerator WaitForOpponent(int time)
    {
        if (time > 0)
        {
            if (!isStarted)
            {
                yield return new WaitForSeconds(1);
                time--;
                StartCoroutine(WaitForOpponent(time));
            }
        }
        else
        {
            StopAllCoroutines();
            //status.GetComponent<tk2dTextMesh>().text = "Khong tim thay doi thu...";
            //status.SetActive(true);
            text.SetActive(true);
            text.GetComponent<SpriteRenderer>().sprite = noOpponent;
            iTween.MoveTo(noOpponentDialog, iTween.Hash("y", 0, "time", 1f));

        }
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        //Debug.Log("Connection was lost, Reason: " + (string)evt.Params[MySFSParams.PARAM_REASON]);
        StopAllCoroutines();
        status.GetComponent<tk2dTextMesh>().text = "Connection was lost, reason: " + (string)evt.Params[MySFSParams.PARAM_REASON];
    }

    void OnObjectMesssage(Sfs2X.Core.BaseEvent evt)
    {
        //User sender = evt.Params["sender"] as User;
        ISFSObject dataObj = evt.Params["message"] as SFSObject;
        byte header = dataObj.GetByte("head");
        switch (header) {
                // next number
            case 0:
                {
                    byte tempPlayer = dataObj.GetByte("player");
                    byte tempLine = dataObj.GetByte("line");
                    if (tempPlayer == 1)
                    {
                        Player1Behaviour.currentLine = tempLine;
                    }
                    if(tempPlayer == 2)
                    {
                        Player2Behaviour.currentLine = tempLine;
                    }

                    break;
                }
            case 1:
                {
                    Vector3 tempPosition = new Vector3(0,0,0);
                    float tempSpeed = 0.0f;
                    if(dataObj.GetByte("player") == 1)
                    {
                        Player1Behaviour.state = PlayerState.Attack;
                        tempPosition = SceneData.player1PosArray[dataObj.GetByte("line")];
                        tempSpeed = SceneData.ELEPHANT_SPEED[dataObj.GetByte("type")];
                    }
                    if (dataObj.GetByte("player") == 2)
                    {
                        Player2Behaviour.state = PlayerState.Attack;
                        tempPosition = SceneData.player2PosArray[dataObj.GetByte("line")];
                        tempSpeed = -SceneData.ELEPHANT_SPEED[dataObj.GetByte("type")];
                    }
                    if(dataObj.GetByte("type") == 6)
                    {
                       ElephantMadBehaviour.SpawnElephant("Player"+dataObj.GetByte("player"),
                       dataObj.GetByte("type"),
                       tempPosition,
                       dataObj.GetByte("line"),
                       Constants.ELEPHANT_STRENGTH[dataObj.GetByte("type")],
                       tempSpeed);
                    }
                    else
                    {
                        ElephantBehaviour.SpawnElephant("Player" + dataObj.GetByte("player"),
                       dataObj.GetByte("type"),
                       tempPosition,
                       dataObj.GetByte("line"),
                       Constants.ELEPHANT_STRENGTH[dataObj.GetByte("type")],
                       tempSpeed);
                    }
                    break;
                }
            case 2:
                {
                    int tempPlayer = dataObj.GetByte("player");
                    if(tempPlayer == 1)
                    {
                        CoconutCountBehaviour.currentAvatarTexturePlayer1 = dataObj.GetByte("type");
                    }
                    if(tempPlayer == 2)
                    {
                        CoconutCountBehaviour.currentAvatarTexturePlayer2 = dataObj.GetByte("type");
                    }
                    break;
                }
            case 3:
                {
                    int tempPlayer = dataObj.GetByte("player");
                    if(tempPlayer == 1)
                    {
                        CoconutCountBehaviour.timerPlayer1 = Constants.COCONUT_LEAF_NUMBER;
                        CoconutCountBehaviour.isReadyPlayer1 = false;
                    }
                    if(tempPlayer == 2)
                    {
                        CoconutCountBehaviour.timerPlayer2 = Constants.COCONUT_LEAF_NUMBER;
                        CoconutCountBehaviour.isReadyPlayer2 = false;
                    }
                    break;
                }
            case 4:
                {
                    int topEdgePosition = dataObj.GetByte("position");
                    int itemName = dataObj.GetByte("itemName");
                    int tempLine = dataObj.GetByte("line");
                    DropItem(topEdgePosition, Constants.ARRAY_ITEM_NAME[itemName], tempLine);
                    break;
                }
            case 5:
                {
                    Vector3 tempPosition = new Vector3(0, 0, 0) ;
                    if (dataObj.GetByte("player") == 1)
                    {
                        Player1Behaviour.state = PlayerState.Attack;
                        tempPosition = SceneData.player1PosArray[dataObj.GetByte("line")];
                        
                    }
                    if (dataObj.GetByte("player") == 2)
                    {
                        Player2Behaviour.state = PlayerState.Attack;
                        tempPosition = SceneData.player2PosArray[dataObj.GetByte("line")];
                        
                    }
                    ElephantCocoBehaviour.SpawnElephant("Player" + dataObj.GetByte("player"), dataObj.GetByte("line"), tempPosition);
                    break;
                }
            default:
                break;
        }
    }

    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {

        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        ISFSObject mParams = new SFSObject();
        
        switch (cmd)
        {

            // player move was made, change next_number & notify player's opponent
            case "move":
                ISFSObject dataObj = evt.Params["params"] as SFSObject;

                Vector3 tempPosition = new Vector3(0,0,0);
                    float tempSpeed = 0.0f;
                    if(dataObj.GetByte("player") == 1)
                    {
                        Player1Behaviour.state = PlayerState.Attack;
                        tempPosition = SceneData.player1PosArray[dataObj.GetByte("line")];
                        tempSpeed = SceneData.ELEPHANT_SPEED[dataObj.GetByte("type")];
                    }
                    if (dataObj.GetByte("player") == 2)
                    {
                        Player2Behaviour.state = PlayerState.Attack;
                        tempPosition = SceneData.player2PosArray[dataObj.GetByte("line")];
                        tempSpeed = -SceneData.ELEPHANT_SPEED[dataObj.GetByte("type")];
                    }
                    if (dataObj.GetByte("type") == 6)
                    {
                        ElephantMadBehaviour.SpawnElephant("Player" + dataObj.GetByte("player"),
                       dataObj.GetByte("type"),
                       tempPosition,
                       dataObj.GetByte("line"),
                       Constants.ELEPHANT_STRENGTH[dataObj.GetByte("type")],
                       tempSpeed);
                    }
                    else if (dataObj.GetByte("type") == 9)
                    {
                        ElephantCocoBehaviour.SpawnElephant("Player" + dataObj.GetByte("player"),
                            dataObj.GetByte("line"),
                            tempPosition);
                    }
                    else
                    {
                        ElephantBehaviour.SpawnElephant("Player" + dataObj.GetByte("player"),
                       dataObj.GetByte("type"),
                       tempPosition,
                       dataObj.GetByte("line"),
                       Constants.ELEPHANT_STRENGTH[dataObj.GetByte("type")],
                       tempSpeed);
                    }
                break;

            // game started, let's defeat your opponent
            case "go":
                StopAllCoroutines();
                StartCoroutine(countDown(secondDownMulti));
                
                //sfConnectObj.Send(new ObjectMessageRequest(mParams));
                
                break;

            case "win":
                //Application.LoadLevel(6);
                //Debug.Log("----GAME END----");
                UnregisterSFSSceneCallbacks();
                text.SetActive(true);
                //status.SetActive(true);
                StopAllCoroutines();
                //status.GetComponent<tk2dTextMesh>().text = "Your opponent had left game...";
                text.GetComponent<SpriteRenderer>().sprite = leaveRoom;
                if (isStarted)
                {
                    StartCoroutine(countDownToWin(3f));
                }
                else
                {
                    iTween.MoveTo(leaveOpponentDialog, iTween.Hash("y", 0 , "time", 1f));
                    status.SetActive(false);
                }
               
                //
                //entGameOver();
                break;

            // your opponent has left game, have fun with BOT :v
            case "bot":
                //Debug.Log("----BOT----");
                if (isStarted)
                {
                    UnregisterSFSSceneCallbacks();
                    isBotFeature = true;
                    InvokeRepeating("StartMachine", 1, 10);
                }
                else {
                    StopAllCoroutines();
                    //status.GetComponent<UILabel>().text = "Your opponent has left game. Game stoped!";
                }
                break;
        }

    }

    //------------------------// Gameplay
    //Drop Item Function
    void DropItem(int position, string itemName, int line)
    {
        ItemBehaviour itemInstance;
        switch (itemName)
        {
            case "Shield":
                {
                    itemInstance = Instantiate(iShield, SceneData.topEdgePosition[position], Quaternion.identity) as ItemBehaviour;
                    itemInstance.line = line;
                    break;
                }
            case "Coco":
                {
                    itemInstance = Instantiate(iCoco, SceneData.topEdgePosition[position], Quaternion.identity) as ItemBehaviour;
                    itemInstance.line = line;
                    break;
                }
            case "Boot":
                {
                    itemInstance = Instantiate(iBoot, SceneData.topEdgePosition[position], Quaternion.identity) as ItemBehaviour;
                    itemInstance.line = line;
                    break;
                }
            case "Sword":
                {
                    itemInstance = Instantiate(iSword, SceneData.topEdgePosition[position], Quaternion.identity) as ItemBehaviour;
                    itemInstance.line = line;
                    break;
                }
            default:
                {
                    break;
                }
        }
        

    }


    // Spawn bird fucntion
    void SpawnBird()
    {
        BirdBehaviour birdInstance;
        birdInstance = Instantiate(bird) as BirdBehaviour;
        //SmartFoxConnection.SendSpawnBird(birdInstance.isPlayer1Side, birdInstance.line, birdInstance.itemName);
    }

    // Spawn elephant of Player 1
    void SpawnPlayer1Elephant()
    {
        Player1Behaviour.state = PlayerState.Attack;
        if (CoconutCountBehaviour.currentAvatarTexturePlayer1 == 9)
        {
            //ElephantCocoBehaviour.SpawnElephant("Player1", Player1Behaviour.currentLine, SceneData.player1PosArray[Player1Behaviour.currentLine]);
            //SmartFoxConnection.SendSpawnCoconutElephant(1, (byte)Player1Behaviour.currentLine);
            SmartFoxConnection.SendSpawnEleMessageToServer(1, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer1,(byte)Player1Behaviour.currentLine);
        }
        else if (CoconutCountBehaviour.currentAvatarTexturePlayer1 == 6)
        {
            //ElephantMadBehaviour.SpawnElephant("Player1", CoconutCountBehaviour.currentAvatarTexturePlayer1,
            //                                SceneData.player1PosArray[Player1Behaviour.currentLine], Player1Behaviour.currentLine,
            //                                Constants.ELEPHANT_STRENGTH[CoconutCountBehaviour.currentAvatarTexturePlayer1],
            //                                SceneData.ELEPHANT_SPEED[CoconutCountBehaviour.currentAvatarTexturePlayer1]);
            //SmartFoxConnection.SendSpawnEleMessage("Player1", CoconutCountBehaviour.currentAvatarTexturePlayer1, Player1Behaviour.currentLine);
            SmartFoxConnection.SendSpawnEleMessageToServer(1, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer1, (byte)Player1Behaviour.currentLine);
        }
        else
        {
            //ElephantBehaviour.SpawnElephant("Player1", CoconutCountBehaviour.currentAvatarTexturePlayer1,
            //                                SceneData.player1PosArray[Player1Behaviour.currentLine], Player1Behaviour.currentLine,
            //                                Constants.ELEPHANT_STRENGTH[CoconutCountBehaviour.currentAvatarTexturePlayer1],
            //                                SceneData.ELEPHANT_SPEED[CoconutCountBehaviour.currentAvatarTexturePlayer1]);
            //SmartFoxConnection.SendSpawnEleMessage("Player1", CoconutCountBehaviour.currentAvatarTexturePlayer1, Player1Behaviour.currentLine);
            SmartFoxConnection.SendSpawnEleMessageToServer(1, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer1, (byte)Player1Behaviour.currentLine);
        }
        if (SceneData.specificEleArrayPlayer1.Count > 0)
        {
            CoconutCountBehaviour.currentAvatarTexturePlayer1 = SceneData.GetSpecialElephant(SceneData.specificEleArrayPlayer1);
            SmartFoxConnection.SendSymbolChange(1, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer1);
        }
        else
        {
            CoconutCountBehaviour.currentAvatarTexturePlayer1 = SceneData.RandomAndRemoveElephant(SceneData.eleArrayPlayer1);
            SmartFoxConnection.SendSymbolChange(1, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer1);
        }
        CoconutCountBehaviour.isReadyPlayer1 = false;
        CoconutCountBehaviour.timerPlayer1 = Constants.COCONUT_LEAF_NUMBER;
        SmartFoxConnection.SendTimeReset(1);
    }

    // Spawn elephant of Player 2
    void SpawnPlayer2Elephant()
    {
        Player2Behaviour.state = PlayerState.Attack;
        if (CoconutCountBehaviour.currentAvatarTexturePlayer2 == 9)
        {
            //ElephantCocoBehaviour.SpawnElephant("Player2", Player2Behaviour.currentLine, player2.transform.position);
            //SmartFoxConnection.SendSpawnCoconutElephant(2, (byte)Player2Behaviour.currentLine);
            SmartFoxConnection.SendSpawnEleMessageToServer(2, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer2, (byte)Player2Behaviour.currentLine);
        }
        else if (CoconutCountBehaviour.currentAvatarTexturePlayer2 == 6)
        {
            //ElephantMadBehaviour.SpawnElephant("Player2", CoconutCountBehaviour.currentAvatarTexturePlayer2,
            //                                SceneData.player2PosArray[Player2Behaviour.currentLine], Player2Behaviour.currentLine,
            //                                Constants.ELEPHANT_STRENGTH[CoconutCountBehaviour.currentAvatarTexturePlayer2],
            //                                -SceneData.ELEPHANT_SPEED[CoconutCountBehaviour.currentAvatarTexturePlayer2]);
            //SmartFoxConnection.SendSpawnEleMessage("Player2", CoconutCountBehaviour.currentAvatarTexturePlayer2, Player2Behaviour.currentLine);
            SmartFoxConnection.SendSpawnEleMessageToServer(2, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer2, (byte)Player2Behaviour.currentLine);
        }
        else
        {
            //ElephantBehaviour.SpawnElephant("Player2", CoconutCountBehaviour.currentAvatarTexturePlayer2,
            //                                SceneData.player2PosArray[Player2Behaviour.currentLine], Player2Behaviour.currentLine,
            //                                Constants.ELEPHANT_STRENGTH[CoconutCountBehaviour.currentAvatarTexturePlayer2],
            //                                -SceneData.ELEPHANT_SPEED[CoconutCountBehaviour.currentAvatarTexturePlayer2]);
            //SmartFoxConnection.SendSpawnEleMessage("Player2", CoconutCountBehaviour.currentAvatarTexturePlayer2, Player2Behaviour.currentLine);
            SmartFoxConnection.SendSpawnEleMessageToServer(2, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer2, (byte)Player2Behaviour.currentLine);
        }
        if (SceneData.specificEleArrayPlayer2.Count > 0)
        {
            CoconutCountBehaviour.currentAvatarTexturePlayer2 = SceneData.GetSpecialElephant(SceneData.specificEleArrayPlayer2);
            SmartFoxConnection.SendSymbolChange(2, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer2);
        }
        else
        {
            CoconutCountBehaviour.currentAvatarTexturePlayer2 = SceneData.RandomAndRemoveElephant(SceneData.eleArrayPlayer2);
            SmartFoxConnection.SendSymbolChange(2, (byte)CoconutCountBehaviour.currentAvatarTexturePlayer2);
        }
        CoconutCountBehaviour.isReadyPlayer2 = false;
        CoconutCountBehaviour.timerPlayer2 = Constants.COCONUT_LEAF_NUMBER;
        SmartFoxConnection.SendTimeReset(2);
    }

    void Player1Handler()
    {
        // Get input touch of user
        if (Input.GetMouseButtonUp(0))
        {
            //Vector3 abc = Input.mousePosition;
            if (Input.mousePosition.x < SceneData.leftMouseLimit)
            {
                if (Input.mousePosition.y <
                   Camera.main.WorldToScreenPoint(player1.transform.position).y - SceneData.lineHeight / 2)
                {
                    if (Player1Behaviour.currentLine > 0)
                    {
                        Player1Behaviour.currentLine--;
                        SmartFoxConnection.SendCharacterMoveMessage(1, (byte)Player1Behaviour.currentLine);
                    }
                }
                if (Input.mousePosition.y >
                   Camera.main.WorldToScreenPoint(player1.transform.position).y + SceneData.lineHeight / 2)
                {
                    if (Player1Behaviour.currentLine < Constants.LINE - 1)
                    {
                        Player1Behaviour.currentLine++;
                        SmartFoxConnection.SendCharacterMoveMessage(1, (byte)Player1Behaviour.currentLine);
                    }
                }
            }
            //else if (Input.mousePosition.x < SceneData.middleMouseLimit && Input.mousePosition.y > SceneData.bottomMouseLimit)
            else if (Input.mousePosition.y > SceneData.bottomMouseLimit)
            {
                if (CoconutCountBehaviour.isReadyPlayer1)
                {
                    SpawnPlayer1Elephant();
                }

            }
        }
    }

    // Handle Player 2 Input
    void Player2Handler()
    {
        // Get input touch of user
        if (Input.GetMouseButtonUp(0))
        {
            //Vector3 abc = Input.mousePosition;
            if (Input.mousePosition.x > SceneData.rightMouseLimit)
            {
                if (Input.mousePosition.y <
                   Camera.main.WorldToScreenPoint(player2.transform.position).y - SceneData.lineHeight / 2)
                {
                    if (Player2Behaviour.currentLine > 0)
                    {
                        Player2Behaviour.currentLine--;
                        SmartFoxConnection.SendCharacterMoveMessage(2, (byte)Player2Behaviour.currentLine);
                    }
                }
                if (Input.mousePosition.y >
                   Camera.main.WorldToScreenPoint(player2.transform.position).y + SceneData.lineHeight / 2)
                {
                    if (Player2Behaviour.currentLine < Constants.LINE - 1)
                    {
                        Player2Behaviour.currentLine++;
                        SmartFoxConnection.SendCharacterMoveMessage(2, (byte)Player2Behaviour.currentLine);
                    }
                }
            }
            //else if (Input.mousePosition.x > SceneData.middleMouseLimit && Input.mousePosition.y > SceneData.bottomMouseLimit)
            else if (Input.mousePosition.y > SceneData.bottomMouseLimit)
            {
                if (CoconutCountBehaviour.isReadyPlayer2)
                {
                    SpawnPlayer2Elephant();
                }

            }
        }
    }

    // Draw Score, Time...
    //void OnGUI()
    //{
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 50;
    //    style.normal.textColor = Color.black;
    //    GUI.Label(timeRect, Mathf.RoundToInt(playTime).ToString(), style);
    //    GUI.Label(player1ScoreRect, SceneData.scorePlayer1.ToString(), style);
    //    GUI.Label(player2ScoreRect, SceneData.scorePlayer2.ToString(), style);

    //}

    private void UnregisterSFSSceneCallbacks()
    {
        // This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
        sfConnectObj.RemoveAllEventListeners();
    }

    void OnDestroy()
    {
        UnregisterSFSSceneCallbacks();
    }
    
}
 */