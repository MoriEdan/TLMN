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
using Sfs2X.Util;
using Assets.Scripts;
public class SFSGameRoom : MonoBehaviour
{

    public SceneData sceneData;
    public HumanPlayer humanPlayerPrefab;
    public AIPlayer aiPlayerPrefab;
    public Card cardPrefab;
    public List<Player> players;
    public List<int> playerIdInGame;

    public tk2dUIItem btnSuperWin;

    private int numberPlayer;
    private GameState gameState;
    private List<Card> allCards;

    private Player currentPlayer;
    private int currentPlayerIndex;
    private Hand currentHand;
    private bool isFirstRound;

    private int currentSortLayer;
    private int currentPlaceToPutCard;

    //Player who has currentHand in one round
    private Player roundPlayer;
    private int winPlayerNumber;

    private List<Card> cardOnTable;

    // Test for rearrange card, not the best method
    public static int reArrangeCardValue;
    public static Vector3 reArrangeCardPosition;

    // Define variables for smarfoxserver
    // SMARTFOX COMPONENT
    private SmartFox sfConnectObj;

    private SFSObject receivedData;
    // Text in game
    private GameObject sfStatus;
    private GameObject sfStatusNumber;
    // indicate this player is host or not
    public static bool isHost;
    // start or not
    public static bool isStarted;

    // Order 
    public static int order;
    public static int winPlayer;

    // UI
    private GameObject imgLama;
    private GameObject imgDeck;

    // Effect
    private List<Effect> effects;
    private List<GameObject> imgs;

    //NOtice
    public tk2dTextMesh txtNotice;

    // User Info
    public GameObject userInfo;

    // CHange put card position Place;
    private List<Card> previousCards;
    private List<Card> currentCards;

    

    // Test 
    private GameObject txtOrder;
    private GameObject txtCurrentPlayerOrder;
    private GameObject txtCurrentHandType;
    private int test;
    void Awake()
    {
        txtOrder = GameObject.Find("Order");
        txtCurrentPlayerOrder = GameObject.Find("CurrentPlayerOrder");
        txtCurrentHandType = GameObject.Find("CurrentHandType");

        if (SmartFoxConnection.IsInitialized)
        {
            sfConnectObj = SmartFoxConnection.Connection;
        }
        else
        {
            Application.LoadLevel("TLMN_Menu");
        }

        // Register callback delegate
        sfConnectObj.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfConnectObj.AddEventListener(SFSEvent.OBJECT_MESSAGE, OnObjectMesssage);
        sfConnectObj.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    // Use this for initialization
    void Start()
    {
        gameState = GameState.Idle;
        numberPlayer = 1;
        players = new List<Player>();
        playerIdInGame = new List<int>();
        // Disable this for playing online :D :D
        //CreatePlayers(numberPlayer);
        currentPlayerIndex = 0;

        allCards = new List<Card>();

        // Create cards and shuffle them
        CreateAllCards();
        previousCards = new List<Card>();
        currentCards = new List<Card>();

        // Disable this for playing online :D :D
        //this.allCards = Shuffle(this.allCards);

        // set is first round or not, for player who has 3-spade will go first
        isFirstRound = true;

        // Set first player, hand
        currentHand = new Hand(new List<Card>());

        // Set Info on table to put card
        currentSortLayer = 1;
        currentPlaceToPutCard = 0;

        winPlayerNumber = 0;
        cardOnTable = new List<Card>();

        // For drag cards
        reArrangeCardValue = -1;

        //------------------Smartfox Server--------------------------//
        SmartFoxConnection.Connection = sfConnectObj;
        receivedData = SmartFoxConnection.PersistentData;
        sfStatus = GameObject.Find("TxtSfStatus");
        sfStatusNumber = GameObject.Find("TxtSfStatusNumber");

        isStarted = false;
        isHost = receivedData.GetBool("isHost");
        sfStatus.GetComponent<tk2dTextMesh>().text = "";

        // UI
        imgLama = GameObject.Find("ImgLama");
        imgDeck = GameObject.Find("ImgFakeDeck");
        imgDeck.SetActive(false);

        // Effect
        effects = new List<Effect>();
        imgs = new List<GameObject>();

        btnSuperWin.gameObject.SetActive(false);

        GetUserInfoMoney();
        GetUserInfoXP();
    }
   
    // Update is called once per frame
    void Update()
    {
        if(order!=null)
        {
            txtOrder.GetComponent<tk2dTextMesh>().text = order.ToString();
        }
        if(currentPlayer !=null)
        {
            txtCurrentPlayerOrder.GetComponent<tk2dTextMesh>().text = currentPlayer.Name.ToString();
        }
        if(currentHand!=null)
        {
            txtCurrentHandType.GetComponent<tk2dTextMesh>().text = currentHand.Type.ToString();
        }

        switch (gameState)
        {
            case GameState.Idle:
                {
                    break;
                }
            case GameState.Deal:
                {
                    break;
                }
            case GameState.Play:
                {
                    if (reArrangeCardValue >= 0)
                    {
                        ReArrange(reArrangeCardValue);
                        reArrangeCardValue = -1;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    // For send and receive message :D
    void FixedUpdate()
    {
        sfConnectObj.ProcessEvents();
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
            sfStatus.GetComponent<tk2dTextMesh>().text = "Không tìm thấy đối thủ...";
            sfStatus.SetActive(true);
            //iTween.MoveTo(noOpponentDialog, iTween.Hash("y", 0, "time", 1f));
        }
    }

    void OnConnectionLost(Sfs2X.Core.BaseEvent evt)
    {
        StopAllCoroutines();
        sfStatus.GetComponent<tk2dTextMesh>().text = "Connection was lost, reason: " + (string)evt.Params[MySFSParams.PARAM_REASON];
    }

    void OnObjectMesssage(Sfs2X.Core.BaseEvent evt)
    {
        //User sender = evt.Params["sender"] as User;
        ISFSObject dataObj = evt.Params["message"] as SFSObject;
        byte header = dataObj.GetByte("head");
        switch (header)
        {
            // next number
            case 0:
                {
                    int tempPlayerIndex = dataObj.GetByte("PlayerIndex");
                    this.CurrentHand = new Hand(new List<Card>());
                    //this.NewRound(this.players[tempPlayerIndex], this);
                    break;
                }
            default:
                break;
        }
    }


    void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        //ISFSObject mParams = new SFSObject();
        ISFSObject dataObj = evt.Params["params"] as SFSObject;
        switch (cmd)
        {
            case "Start":
                {
                    //if(!isStarted)
                    //{
                    //    order = dataObj.GetByte("Order") - 1; // -1 because order on server start with 1 , then 2 3 4...
                    //    numberPlayer = dataObj.GetByte("NumberPlayer");
                    //    sceneData.CreatePlayerPosition();
                    //    sceneData.CreatePlayerCardPosition();
                    //    SwapCardPosition(order);
                    //    CreatePlayers(numberPlayer, order);
                    //    currentPlayerIndex = dataObj.GetByte("GoFirstPlayer") - 1;
                    //    //currentPlayerIndex = (Constants.MAX_NUMBER_PLAYER + currentPlayerIndex - order) % 4;
                    //    currentPlayer = players[currentPlayerIndex];
                    //    roundPlayer = players[currentPlayerIndex];
                    //    isStarted = true;
                    //}
                    break;
                }
            case "CountDown":
                {
                    //StopAllCoroutines();
                    //StartCoroutine(CountDown(Constants.COUNT_DOWN_TIME_FOR_STARTING));
                    imgLama.SetActive(false);
                    sfStatus.SetActive(true);
                    sfStatus.GetComponent<tk2dTextMesh>().text = "Chuẩn bị vào trận đấu";
                    sfStatusNumber.SetActive(true);
                    order = dataObj.GetByte("Order"); // -1 because order on server start with 1 , then 2 3 4...
                    numberPlayer = dataObj.GetByte("NumberPlayer");
                    sceneData.CreatePlayerPosition();
                    sceneData.CreatePlayerCardPosition();
                    VariableApplication.tlmnIdUser[0] = dataObj.GetUtfString("UserName0");
                    VariableApplication.tlmnIdUser[1] = dataObj.GetUtfString("UserName1");
                    VariableApplication.tlmnIdUser[2] = dataObj.GetUtfString("UserName2");
                    VariableApplication.tlmnIdUser[3] = dataObj.GetUtfString("UserName3");

                    SwapCardPosition(order);
                    int[] tempListPlayer = new int[4];
                    tempListPlayer = dataObj.GetIntArray("ListPlayer");
                    for (int i = 0; i < players.Count; i++)
                    {
                        if(players[i]!=null)
                        {
                            if (players[i].gameObject != null)
                            {
                                Destroy(players[i].gameObject);
                            }
                        }
                    }
                    players.Clear();
                    playerIdInGame.Clear();
                    for (int i = 0; i < tempListPlayer.Length; i++ )
                    {
                        CreatePlayer(i, dataObj.GetUtfString("UserName"+i));
                        playerIdInGame.Add(tempListPlayer[i]);
                    }
                    SetNotice("[" + dataObj.GetUtfString("JoinUserName") + "] " + Constants.TXT_NEW_PLAYER);

                    break;
                }
            case "UpdateTime":
                {
                    sfStatusNumber.GetComponent<tk2dTextMesh>().text = dataObj.GetByte("Second").ToString();
                    break;
                }
            case "DealCards":
                {
                    sfStatus.SetActive(false);
                    sfStatus.GetComponent<tk2dTextMesh>().text = "Chuẩn bị vào trận đấu";
                    sfStatusNumber.SetActive(false);

                    if (!isStarted)
                    {
                        //sceneData.CreatePlayerPosition();
                        //sceneData.CreatePlayerCardPosition();
                        //SwapCardPosition(order);
                        //CreatePlayers(numberPlayer, order);
                        currentPlayerIndex = dataObj.GetByte("GoFirstPlayer");
                        currentPlayer = players[currentPlayerIndex];
                        roundPlayer = players[currentPlayerIndex];
                        isStarted = true;
                    }

                    int[] array = new int[52];
                    array = dataObj.GetIntArray("AllCards");
                    allCards = ByteArrayToCards(array);
                    DealCards(allCards, order);

                    //imgDeck.SetActive(true);
                    for (int i = 0; i < numberPlayer; i++)
                    {
                        //int playerIndex = (Constants.MAX_NUMBER_PLAYER- order + i) % Constants.MAX_NUMBER_PLAYER;

                        StartCoroutine(DealCards_Move(players[i].Deck.Cards, i));
                        players[i].IsHasSuperCut = players[i].CheckSuperCut();
                    }
                    currentPlayer.State = PlayerState.Play;

                    if(players[order].CheckSuperWin())
                    {
                        btnSuperWin.gameObject.SetActive(true);
                    }

                    break;
                }
            case "PlayerPutHand":
                {
                    byte[] bArray = new byte[13];
                    ByteArray byteArray = new ByteArray(bArray);
                    byteArray = dataObj.GetByteArray("Hand");
                    //int tempPlayerIndex = (Constants.MAX_NUMBER_PLAYER - order + dataObj.GetByte("PlayerIndex")) % Constants.MAX_NUMBER_PLAYER;
                    int tempPlayerIndex = dataObj.GetByte("PlayerIndex");
                    Hand putHand = new Hand(ByteArrayToCards_Player(byteArray, players[tempPlayerIndex]));
                    putHand.Type = Constants.HAND_TYPE[dataObj.GetByte("Type")];
                    players[tempPlayerIndex].PutCard(putHand);
                    players[tempPlayerIndex].IsNewRoundPlayer = false;
                    // SuperWin
                    if(this.Order == tempPlayerIndex)
                    {
                        this.btnSuperWin.gameObject.SetActive(false);
                    }

                    // Auto Sort After put cards
                    this.SortWithoutBubble(this.players[this.Order].Deck.Cards, this.Order);

                    ISFSObject tempWinParams = dataObj.GetSFSObject("winParams");
                    if(tempWinParams!=null)
                    {
                        SetWinEffect(tempWinParams.GetByte("PlayerWin"), 
                            tempWinParams.GetByte("WinOrder"), 
                            tempWinParams.GetByte("PlayerLose"),
                            tempWinParams.GetByte("LeftPlayerNumber"));
                        if(tempWinParams.GetByte("PlayerLose")!=10)
                        {
                            ISFSObject tempMoneyParams = tempWinParams.GetSFSObject("MoneyObject");
                            for (int i = 0; i < players.Count; i++)
                            {
                                double value = tempMoneyParams.GetDouble((i + 1) + "");
                                if (value != null && players[i].IsActive)
                                {
                                    players[i].GetComponent<HumanPlayer>().txtMoney.GetComponent<tk2dTextMesh>().text = value.ToString();
                                }
                            }
                            StartCoroutine(WaitToResetGame(4.0f));
                        }
                    }

                    this.RoundPlayer = this.players[tempPlayerIndex];
                    this.SetIdle(this.players[tempPlayerIndex]);
                    this.CurrentPlayer = players[dataObj.GetByte("CurrentPlayer")];
                    this.CurrentPlayer.State = PlayerState.Play;

                    //if(putHand.CardCount() < 12)
                    //{
                    //    this.CheckWin(this.players[tempPlayerIndex]);
                    //}
                    //for (int i = 0; i < Constants.MAX_NUMBER_PLAYER; i++ )
                    //{
                    //    if(players[i].IsActive && players[i].IsHasSuperCut)
                    //    {
                    //        players[i].State = PlayerState.Idle;
                    //    }
                    //}
                    //this.SetPlay();
                    break;
                }
            case "PlayerPass":
                {
                    int tempPlayerIndex = dataObj.GetByte("PlayerIndex");                    
                    this.SetPass(this.players[tempPlayerIndex]);
                    if(dataObj.GetByte("CurrentPlayer") != 10)
                    {
                        test = dataObj.GetByte("CurrentPlayer");
                        this.CurrentPlayer = players[dataObj.GetByte("CurrentPlayer")];
                        this.CurrentPlayer.State = PlayerState.Play;
                    }
                    else
                    {
                        this.CurrentHand = new Hand(new List<Card>());
                        NewRound();
                        this.CurrentPlayer = players[dataObj.GetByte("NewPlayer")];
                        this.CurrentPlayer.State = PlayerState.Play;
                        this.CurrentPlayer.IsNewRoundPlayer = true;
                        ISFSObject tempMoneyParams = dataObj.GetSFSObject("MoneyObject");
                        if(tempMoneyParams!=null)
                        {
                            for (int i = 0; i < players.Count; i++)
                            {
                                double value = tempMoneyParams.GetDouble((i + 1) + "");
                                if (value != null && players[i].IsActive)
                                {
                                    players[i].GetComponent<HumanPlayer>().txtMoney.GetComponent<tk2dTextMesh>().text = value.ToString();
                                }
                            }
                        }
                    }
                    break;
                }
            case "NewRound":
                {
                    int tempPlayerIndex = dataObj.GetByte("PlayerIndex");
                    this.CurrentHand = new Hand(new List<Card>());
                    //this.NewRound(this.players[tempPlayerIndex], this);
                    break;
                }
            case "Win":
                {
                    int tempPlayerIndex = dataObj.GetByte("PlayerIndex");
                    int winIndex = dataObj.GetByte("WinOrder");
                    //EffectManager.ShowEffect(winIndex, sceneData.PlayerPositions[tempPlayerIndex]);
                    break;
                }
            case "UserLeft":
                {
                    int tempPlayerIndex = dataObj.GetByte("LeftUserOrder");
                    int tempIsStarted = dataObj.GetByte("IsStarted");
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].Name == tempPlayerIndex)
                        {
                            if (tempIsStarted == 0)
                            {
                                Destroy(players[i].gameObject);
                                players.RemoveAt(i);
                                numberPlayer--;
                                if(order > tempPlayerIndex)
                                {
                                    order--;
                                    sceneData.CreatePlayerPosition();
                                    sceneData.CreatePlayerCardPosition();
                                    SwapCardPosition(order);
                                }
                            }
                            else
                            {
                                players[i].IsActive = false;
                            }
                        }
                    }

                    SetNotice("[" + dataObj.GetUtfString("LeaveUserName") + "] " + Constants.TXT_LEAVE_ROOM_PLAYER);
                    
                    break;
                }
            case "GetMoneyAllPlayer":
                {
                    for (int i = 0; i < players.Count; i++ )
                    {
                        double value = dataObj.GetDouble((i + 1) + "");
                        if(value!=null && players[i].IsActive)
                        {
                            players[i].GetComponent<HumanPlayer>().txtMoney.GetComponent<tk2dTextMesh>().text = value.ToString();
                        }
                    }
                    break;
                }
            case "UpdateTimeToThink":
                {
                    int playerIndex = dataObj.GetByte("PlayerIndex");
                    int timeToThink = dataObj.GetByte("TimeToThink");
                    this.currentPlayer.GetComponent<HumanPlayer>().txtTime.GetComponent<tk2dTextMesh>().text = timeToThink.ToString();
                    float temp = timeToThink * (360 / Constants.COUNT_DOWN_TIME_PER_TURN);
                    this.currentPlayer.GetComponent<HumanPlayer>().imgGlowCircle.transform.rotation = Quaternion.Euler(new Vector3(0,0,temp));
                    break;
                }
            case "AutoPutCard":
                {
                    int playerIndex = dataObj.GetByte("PlayerIndex");
                    if(playerIndex == this.Order)
                    {
                        if (this.players[playerIndex].State != PlayerState.Win)
                        {
                            Hand putHand = this.players[playerIndex].QuickFindHand();
                            if (putHand != null)
                            {
                                SmartFoxConnection.SendHandToServer(putHand, this.Order, false);
                            }
                        }
                    }
                    break;
                }
            case "SendSuperWin":
                {
                    byte[] bArray = new byte[13];
                    ByteArray byteArray = new ByteArray(bArray);
                    byteArray = dataObj.GetByteArray("Hand");
                    //int tempPlayerIndex = (Constants.MAX_NUMBER_PLAYER - order + dataObj.GetByte("PlayerIndex")) % Constants.MAX_NUMBER_PLAYER;
                    int tempPlayerIndex = dataObj.GetByte("PlayerIndex");
                    Hand putHand = new Hand(ByteArrayToCards_Player(byteArray, players[tempPlayerIndex]));
                    putHand.Type = Constants.HAND_TYPE[dataObj.GetByte("Type")];
                    players[tempPlayerIndex].PutCard(putHand);

                    ISFSObject tempMoneyParams = dataObj.GetSFSObject("MoneyObject");
                    if (tempMoneyParams != null)
                    {
                        for (int i = 0; i < players.Count; i++)
                        {
                            double value = tempMoneyParams.GetDouble((i + 1) + "");
                            if (value != null && players[i].IsActive)
                            {
                                players[i].GetComponent<HumanPlayer>().txtMoney.GetComponent<tk2dTextMesh>().text = value.ToString();
                            }
                        }
                    }

                    // Effect
                    Effect tempEffect;
                    if (this.Order == tempPlayerIndex)
                    {
                        tempEffect = EffectManager.ShowEffect(8, sceneData.ScreenCenter, 1.0f);
                        effects.Add(tempEffect);
                    }
                    else
                    {
                        tempEffect = EffectManager.ShowEffect(6, sceneData.PlayerPositions[this.Order], 1.0f);
                        effects.Add(tempEffect);
                    }
                    StartCoroutine(WaitToResetGame(4.0f));
                    break;
                }
            case "NotEnoughPlayer":
                {
                    ResetGame(false);
                    sfStatus.SetActive(false);
                    sfStatusNumber.SetActive(false);
                    SetNotice("[" + dataObj.GetUtfString("LeaveUserName") + "] " + Constants.TXT_LEAVE_ROOM_PLAYER);
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    #region Swap position to suit with order :)))
    // Swap player's card position suit with player @@
    private void SwapCardPosition(int order)
    {
        Vector3[] tempPosition = new Vector3[Constants.CARD_AMOUNT_FOR_EACH_PLAYER];
        Vector3 tempPlayerPosition = new Vector3(0, 0, 0);
        switch(order)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    tempPosition = sceneData.PlayerCardPosition[0];
                    sceneData.PlayerCardPosition[0] = sceneData.PlayerCardPosition[3];
                    sceneData.PlayerCardPosition[3] = sceneData.PlayerCardPosition[2];
                    sceneData.PlayerCardPosition[2] = sceneData.PlayerCardPosition[1];
                    sceneData.PlayerCardPosition[1] = tempPosition;

                    tempPlayerPosition = sceneData.PlayerPositions[0];
                    sceneData.PlayerPositions[0] = sceneData.PlayerPositions[3];
                    sceneData.PlayerPositions[3] = sceneData.PlayerPositions[2];
                    sceneData.PlayerPositions[2] = sceneData.PlayerPositions[1];
                    sceneData.PlayerPositions[1] = tempPlayerPosition;  

                    break;
                }
            case 2:
                {
                    tempPosition = sceneData.PlayerCardPosition[0];
                    sceneData.PlayerCardPosition[0] = sceneData.PlayerCardPosition[2];
                    sceneData.PlayerCardPosition[2] = tempPosition;
                    tempPosition = sceneData.PlayerCardPosition[1];
                    sceneData.PlayerCardPosition[1] = sceneData.PlayerCardPosition[3];
                    sceneData.PlayerCardPosition[3] = tempPosition;

                    tempPlayerPosition = sceneData.PlayerPositions[0];
                    sceneData.PlayerPositions[0] = sceneData.PlayerPositions[2];
                    sceneData.PlayerPositions[2] = tempPlayerPosition;
                    tempPlayerPosition = sceneData.PlayerPositions[1];
                    sceneData.PlayerPositions[1] = sceneData.PlayerPositions[3];
                    sceneData.PlayerPositions[3] = tempPlayerPosition;

                    break;
                }
            case 3:
                {
                    tempPosition = sceneData.PlayerCardPosition[0];
                    sceneData.PlayerCardPosition[0] = sceneData.PlayerCardPosition[1];
                    sceneData.PlayerCardPosition[1] = sceneData.PlayerCardPosition[2];
                    sceneData.PlayerCardPosition[2] = sceneData.PlayerCardPosition[3];
                    sceneData.PlayerCardPosition[3] = tempPosition;

                    tempPlayerPosition = sceneData.PlayerPositions[0];
                    sceneData.PlayerPositions[0] = sceneData.PlayerPositions[1];
                    sceneData.PlayerPositions[1] = sceneData.PlayerPositions[2];
                    sceneData.PlayerPositions[2] = sceneData.PlayerPositions[3];
                    sceneData.PlayerPositions[3] = tempPlayerPosition;
                    
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    #endregion

    private List<Card> ByteArrayToCards(int[] byteArray)
    {
        List<Card> tempListCard = new List<Card>();
        for(int i = 0 ; i < Constants.CARD_AMOUNT; i++)
        {
            tempListCard.Add(allCards[byteArray[i]]);
        }
        return tempListCard;
    }

    private List<Card> ByteArrayToCards_Player(ByteArray byteArray, Player player)
    {
        List<Card> tempListCard = new List<Card>();
        for (int i = 0; i < byteArray.Bytes.Length; i++)
        {
            for (int j = 0; j < player.Deck.Cards.Count; j++ )
            {
                if(player.Deck.Cards[j].Index == byteArray.Bytes[i])
                {
                    tempListCard.Add(player.Deck.Cards[j]);
                }
            }
                
        }
        return tempListCard;
    }

    private void UnregisterSFSSceneCallbacks()
    {
        // This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
        sfConnectObj.RemoveAllEventListeners();
    }
    void OnDestroy()
    {
        UnregisterSFSSceneCallbacks();
    }

    // Create players
    public void CreatePlayer(int i, string userName)
    {
        int tempPosition = (Constants.MAX_NUMBER_PLAYER - order + i) % Constants.MAX_NUMBER_PLAYER;
        HumanPlayer humanPlayerInstance = Instantiate(sceneData.HumanPlayerPrefabs[tempPosition],
            sceneData.PlayerPositions[i], Quaternion.identity) as HumanPlayer;
        humanPlayerInstance.GameManager = this;
        humanPlayerInstance.Name = i;
        humanPlayerInstance.IsActive = true;
        humanPlayerInstance.UserName = userName;
        players.Add(humanPlayerInstance);   
    }
    public void CreatePlayers(int numberPlayer, int order)
    {
        players.Clear();
        for (int i = 0; i < Constants.MAX_NUMBER_PLAYER; i++)
        {
            //int tempPosition = (Constants.MAX_NUMBER_PLAYER - order + i) % Constants.MAX_NUMBER_PLAYER ;
            //HumanPlayer humanPlayerInstance = Instantiate(humanPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as HumanPlayer;
            int tempPosition = (Constants.MAX_NUMBER_PLAYER - order + i) % Constants.MAX_NUMBER_PLAYER;
            HumanPlayer humanPlayerInstance = Instantiate(sceneData.HumanPlayerPrefabs[tempPosition],
                sceneData.PlayerPositions[i], Quaternion.identity) as HumanPlayer;
            humanPlayerInstance.GameManager = this;
            humanPlayerInstance.Name = i;
            players.Add(humanPlayerInstance);
        }

        for(int i = 0 ; i < numberPlayer; i++)
        {
            players[i].IsActive = true;
        }
    }

    // Create 52 cards
    public void CreateAllCards()
    {
        for (int i = 0; i < Constants.SUITS.Length; i++)
        {
            for (int j = 0; j < Constants.VALUES.Length; j++)
            {
                Card cardInstance = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Card;
                //cardInstance.GetComponent<tk2dSprite>().SetSprite("card_" + (j + 1) + "_" + i);
                cardInstance.GetComponent<tk2dSprite>().SetSprite("CENT CARD Back");
                cardInstance.GetComponent<Card>().Suit = Constants.SUITS[i];
                cardInstance.GetComponent<Card>().Value = Constants.VALUES[j];
                cardInstance.NumberValue = Constants.NUMBER_VALUES[j];
                cardInstance.NumberSuit = Constants.NUMBER_SUITS[i];
                cardInstance.CardValue = Constants.NUMBER_VALUES[j] * 10 + Constants.NUMBER_SUITS[i];
                cardInstance.Index = allCards.Count;
                cardInstance.gameObject.SetActive(false);
                allCards.Add(cardInstance);
            }
        }
    }

    // Shuffle cards
    public List<Card> Shuffle(List<Card> cards)
    {
        //ArrayList tempCards = ArrayToList(cards);
        List<Card> shuffleCards = new List<Card>();
        int randomIndex = 0;
        while (cards.Count > 0)
        {
            randomIndex = Random.Range(0, cards.Count);
            shuffleCards.Add(cards[randomIndex]);
            cards.RemoveAt(randomIndex);
        }
        return shuffleCards;
    }

    // Delay for deal card
    IEnumerator WaitToDealCard()
    {
        yield return new WaitForSeconds(3f);
    }

    public bool isPlayerInGame(int order)
    {
        bool inGame = false;
        for(int i = 0 ; i < playerIdInGame.Count; i++)
        {
            if(order == playerIdInGame[i])
            {
                inGame = true;
            }
        }
        return inGame;
    }

    // Move cards to each player's position
    public void DealCards(List<Card> cards, int order)
    {
     
        for (int i = 0; i < Constants.CARD_AMOUNT; i++)
        {
            int temp = i % 4;
            if (temp < numberPlayer)
            {
                if (players[temp].IsActive)
                {
                    players[temp].Deck.Cards.Add(cards[i]);
                }
            }
        }

        string temptemp = "";
        for (int i = 0; i < 13; i++)
        {
            temptemp = temptemp + " " + players[0].Deck.Cards[i].Index;
        }
        Debug.Log(temptemp);
        temptemp = "";
        for (int i = 0; i < 13; i++)
        {
            temptemp = temptemp + " " + players[1].Deck.Cards[i].Index;
        }
        Debug.Log(temptemp);

        for (int i = 0; i < numberPlayer; i++ )
        {
            Utility.BubbleSort(players[i].Deck.Cards);
        }
        
        gameState = GameState.Play;
    }

    public IEnumerator DealCards_Move(List<Card> cards, int playerIndex)
    {      
        int temp = 0;
        
        for (int i = 0; i < players[playerIndex].Deck.Cards.Count; i++)
        {
            cards[i].gameObject.SetActive(true);
            cards[i].Move(sceneData.PlayerCardPosition[playerIndex][i].x,
                     sceneData.PlayerCardPosition[playerIndex][i].y, Constants.TIME_TO_DEAL_ONE_CARD);
            cards[i].renderer.sortingLayerName = "Card";
            cards[i].renderer.sortingOrder = temp;
            temp++;
            yield return new WaitForSeconds(0.05f);
            cards[i].GetComponent<tk2dSprite>().SetSprite("CENT CART BIG-" + cards[i].Index);
            if(playerIndex != order)
            {
                cards[i].gameObject.SetActive(false);
            }
        }
    }

    // Sort card ( onhand)
    public void Sort(List<Card> cards, int order)
    {
        int temp = (Constants.CARD_AMOUNT_FOR_EACH_PLAYER - cards.Count) / 2;
        Utility.BubbleSort(cards);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Move(sceneData.PlayerCardPosition[order][temp].x, sceneData.PlayerCardPosition[order][temp].y, Constants.TIME_TO_SORT_ONE_CARD);
            cards[i].renderer.sortingOrder = i;
            temp++;
        }
    }

    // Sort without bubble sort :))
    public void SortWithoutBubble(List<Card> cards, int order)
    {
        int temp = (Constants.CARD_AMOUNT_FOR_EACH_PLAYER - cards.Count) / 2;
        
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Move(sceneData.PlayerCardPosition[order][temp].x, sceneData.PlayerCardPosition[order][temp].y, Constants.TIME_TO_SORT_ONE_CARD);
            cards[i].renderer.sortingOrder = i;
            temp++;
        }
    }

    // Drag card handle
    public void ReArrange(int cardValue)
    {
        int tempDragCardIndex = 0;
        Card tempCard = null;
        bool isMoveToLeft = false;
        for (int i = 0; i < players[order].Deck.Cards.Count; i++)
        {
            if (players[order].Deck.Cards[i].CardValue == cardValue)
            {
                tempDragCardIndex = i;
                tempCard = players[order].Deck.Cards[i];
                if (tempCard.transform.position.x >= reArrangeCardPosition.x)
                {
                    isMoveToLeft = false;
                }
                else
                {
                    isMoveToLeft = true;
                }
                break;
            }
        }
        for (int i = 0; i < players[order].Deck.Cards.Count; i++)
        {
            if (tempCard.transform.position.x < players[order].Deck.Cards[i].transform.position.x)
            {
                players[order].Deck.Cards.Insert(i, tempCard);
                if (isMoveToLeft)
                {
                    players[order].Deck.Cards.RemoveAt(tempDragCardIndex + 1);
                }
                else
                {
                    players[order].Deck.Cards.RemoveAt(tempDragCardIndex);
                }

                break;
            }
        }
        SortWithoutBubble(players[order].Deck.Cards, order);
    }

    // Find card is onhand
    public List<Card> FindCardOnHand()
    {
        List<Card> tempCardList = new List<Card>();
        for (int i = 0; i < players[order].Deck.Cards.Count; i++)
        {
            if (players[order].Deck.Cards[i].State == CardState.OnHand)
            {
                tempCardList.Add(players[order].Deck.Cards[i]);
            }
        }
        return tempCardList;
    }

    public void NewRound()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].State != PlayerState.Win)
            {
                players[i].State = PlayerState.Idle;
            }
        }
        for (int i = 0; i < cardOnTable.Count; i++)
        {
            cardOnTable[i].GetComponent<tk2dSprite>().SetSprite("CENT CARD Back");
        }
    }

    public void SetWinEffect(int playerId, int winOrder, int lastPlayerId, int leftPlayerNumber)
    {
        Effect tempEffect;
        GameObject tempImg;
        if (this.Order == playerId)
        {
            tempEffect = EffectManager.ShowEffect(((winOrder-1) * 2), sceneData.ScreenCenter, 1.0f);
            effects.Add(tempEffect);
        }
        else
        {
            tempEffect = EffectManager.ShowEffect(((winOrder - 1) * 2) + 1, sceneData.PlayerPositions[playerId], 1.0f);
            effects.Add(tempEffect);
        }
        tempImg = EffectManager.ShowImage(((winOrder - 1) * 2) + 1, sceneData.PlayerPositions[playerId]);
        imgs.Add(tempImg);
        if (lastPlayerId != 10)
        {
            if (lastPlayerId == this.Order)
            {
                tempEffect = EffectManager.ShowEffect(6 - (leftPlayerNumber *2), sceneData.ScreenCenter, 1.0f);
                effects.Add(tempEffect);
            }
            else
            {
                tempEffect = EffectManager.ShowEffect(7 - (leftPlayerNumber * 2), sceneData.PlayerPositions[lastPlayerId], 1.0f);
                effects.Add(tempEffect);
            }
            tempImg = EffectManager.ShowImage(7 - (leftPlayerNumber * 2), sceneData.PlayerPositions[lastPlayerId]);
            imgs.Add(tempImg);
        }
    }

    void ShowEXP(string level, double EXP, double expThis_Level, double expNext_Level)
    {
        tk2dTextMesh lv = GameObject.Find("User/LV").GetComponent<tk2dTextMesh>();
        Debug.Log("Level" + level);
        lv.text = "Lv: " + level;

        tk2dUIProgressBar perLV = GameObject.Find("User/ProgressBar").GetComponent<tk2dUIProgressBar>();
        float value = (float)((EXP - expThis_Level) / (expNext_Level - expThis_Level));
        perLV.Value = value;
        Debug.Log("Value" + value);
    }

    void GetUserInfoMoney()
    {
        LamaControllib.getInstance().getAllMoneyUserService(LamaControllib.getInstance().getUserId(), GetUserInfoMoneyCallback, this);
    }

    void GetUserInfoMoneyCallback(bool success, JSON json)
    {
        MoneyUserModel moneyUserModel = LamaControllib.getInstance().getMoneyUserModel();
        userInfo.transform.FindChild("Money").GetComponent<tk2dTextMesh>().text = "$ " + moneyUserModel.MoneyChip + " Đ";
    }
    void GetUserInfoXP()
    {
        LamaControllib.getInstance().getXPModelService(LamaControllib.getInstance().getUserId(), GetUserInfoXPCallback, this);
    }
    void GetUserInfoXPCallback(bool success, JSON json)
    {
        XPModel XPModel = LamaControllib.getInstance().getXPModel();
        Debug.Log("test" + XPModel.Level);
        ShowEXP(XPModel.Level, double.Parse(XPModel.Point), double.Parse(XPModel.CurrentLevel), double.Parse(XPModel.NextLevel));

    }



    IEnumerator WaitToResetGame(float time)
    {
        yield return new WaitForSeconds(time);
        ResetGame(true);
    }

    public void ResetGame(bool isEnoughPlayer)
    {
        gameState = Assets.Scripts.GameState.Idle;
        isStarted = false;
        for (int i = 0; i < players.Count; i++)
        {
            //players[i].deck.Cards.Clear();
            //players[i].State = PlayerState.Idle;
            if(players[i].gameObject!=null)
            {
                Destroy(players[i].gameObject);
            }
        }
        cardOnTable.Clear();
        for (int i = 0; i < allCards.Count; i++)
        {
            //allCards[i].State = CardState.Destroy;
            allCards[i].State = CardState.Idle;
            allCards[i].transform.position = sceneData.ScreenCenter;
            allCards[i].gameObject.SetActive(false);
        }

        // destroy all effect
        for (int i = 0; i < effects.Count; i++)
        {
            Destroy(effects[i].gameObject);
        }

        for (int i = 0; i < imgs.Count; i++)
        {
            Destroy(imgs[i].gameObject);
        }

        effects.Clear();
        imgs.Clear();
        Utility.BubbleSortByIndex(allCards);
        for (int i = 0; i < allCards.Count; i++ )
        {
            allCards[i].transform.localScale = new Vector3(0.7f, 0.7f, 0);
        }
        currentHand = new Hand(new List<Card>());
        currentSortLayer = 1;
        currentPlaceToPutCard = 0;
        winPlayerNumber = 0;
        this.btnSuperWin.gameObject.SetActive(false);
        imgLama.SetActive(true);
        if(isEnoughPlayer)
        {
            SmartFoxConnection.SendRestartToServer();
        }
    }

    public void SetPositionToStart()
    {
        int temp = 0;
        for (int i = 0; i < Constants.SUITS.Length; i++)
        {
            for (int j = 0; j < Constants.VALUES.Length; j++)
            {
                allCards[temp].transform.position = new Vector3(0, 0, 0);
                allCards[temp].GetComponent<tk2dSprite>().SetSprite("card_" + (j + 1) + "_" + i);
                //cardInstance.GetComponent<tk2dSprite>().SetSprite("Card_Back");
                allCards[temp].GetComponent<Card>().Suit = Constants.SUITS[i];
                allCards[temp].GetComponent<Card>().Value = Constants.VALUES[j];
                allCards[temp].NumberValue = Constants.NUMBER_VALUES[j];
                allCards[temp].NumberSuit = Constants.NUMBER_SUITS[i];
                allCards[temp].CardValue = Constants.NUMBER_VALUES[j] * 10 + Constants.NUMBER_SUITS[i];
                allCards[temp].Index = i * 4 + j;
                allCards.Add(allCards[temp]);
            }
        }
    }

    // Set state for all players
    public void SetPass(Player player)
    {
        player.State = PlayerState.Pass;
    }
    public void SetIdle(Player player)
    {
        player.State = PlayerState.Idle;
    }

    public Player NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        currentPlayer = players[currentPlayerIndex];
        if (currentPlayer.State == PlayerState.Win)
        {
            NextPlayer();
        }
        return currentPlayer;
    }

    public Player LastPlayer()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].State != PlayerState.Win && players[i].IsActive)
            {
                return players[i];
            }
        }
        return null;
    }

    // NOtice
    public void SetNotice(string notice)
    {
        txtNotice.gameObject.SetActive(true);
        txtNotice.text = notice;
        StartCoroutine(WaitForNotice(Constants.NOTICE_TIME));
    }

    IEnumerator WaitForNotice(float time)
    {
        yield return new WaitForSeconds(time);
        txtNotice.gameObject.SetActive(false);
    }

    // Get & Set
    public GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }
    public List<Card> AllCards
    {
        get { return allCards; }
        set { allCards = value; }
    }
    public Hand CurrentHand
    {
        get { return currentHand; }
        set { currentHand = value; }
    }
    public Player CurrentPlayer
    {
        get { return currentPlayer; }
        set { currentPlayer = value; }
    }
    public int CurrentSortLayer
    {
        get { return currentSortLayer; }
        set { currentSortLayer = value; }
    }
    public int CurrentPlaceToPutCard
    {
        get { return currentPlaceToPutCard; }
        set { currentPlaceToPutCard = value; }
    }
    public Player RoundPlayer
    {
        get { return roundPlayer; }
        set { roundPlayer = value; }
    }
    public List<Card> CardOnTable
    {
        get { return cardOnTable; }
        set { cardOnTable = value; }
    }

    public int Order
    {
        get { return order; }
        set { order = value; }
    }
    public SmartFox SfConnectObj
    {
        get { return sfConnectObj; }
        set { sfConnectObj = value; }
    }
    public List<Card> PreviousCards
    {
        get { return previousCards; }
        set { previousCards = value; }
    }
    public List<Card> CurrentCards
    {
        get { return currentCards; }
        set { currentCards = value; }
    }
}
