using UnityEngine;
using System.Collections;

/// <summary>
/// list of command help user communicate with server
/// NOTE: this is our convention
/// </summary>
public class SFCommands
{

    /// <summary>
    /// notify to server that user has ready to play, also ask SmartFox server find another player to play game
    /// </summary>
    public const string CMD_READY = "ready";

    /// <summary>
    /// indicate to server that user make a move
    /// </summary>
    public const string CMD_FINISH = "finish";

    public const string NAME_ROOM = "NAME_ROOM";
    public const string IS_HOST_ROOM = "IS_HOST_ROOM";
    public const string IS_CREATE_ROOM_DYNAMIC = "IS_CREATE_ROOM_DYNAMIC";
    public const string USER_NAME_KEY = "USER_NAME_KEY";

    public const string KIND_GAME = "3";

    public const string KIND_GAME_MESS = "GameKind";

    public const string KIND_GAME_FIGHT = "GameKindFight";

    public const string REQUEST_QUICK_GAME = "RequestQuickGame";


    /**
	 * VARIABLE CONNECT ROOM
	 */
    public const string IS_CREATE_ROOM_SUCCESS = "CreateRomSuccess";

    public const string IS_JOIN_ROOM_SUCCESS = "JoinRoomSuccess";

    public const string IS_HOST = "JoinSuccess";

    public const string CM_CREATE_ROOM = "CommandCreateRoom";

    public const string CM_JOIN_ROOM = "CommandJoinRoom";

    public const string MONEY_GAME  = "MONEY_GAME"; 
	
	public const string PRIVATE_GAME  = "PRIVATE_GAME";

    public const string SPEED_GAME = "SPEED_GAME";

    public const string IS_CREATE_ROOM_NEW = "IS_CREATE_ROOM_NEW";

    public const string PASSWORD_ROOM_JOIN = "PASSWORD_ROOM_JOIN"; 
    /**
     * 
     */

    /**
	 * USER READY GAME
	 */
    public const string CM_READY_FIGHT_ROOM = "CommandReadyFight";

    public const string READY_FIGHT_ROOM_SUCCESS = "ReadyFightRoomSuccess";

    public const string READY_FIGHT_ROOM_LOCATION = "ReadyFightRoomLocation";

    public const string CM_READY_START_GAME = "CommandReadyStartGame";

    public const string USERNAME_1 = "Username1";

    public const string USERNAME_2 = "Username2";

    public const string USERNAME_3 = "Username3";

    public const string USERNAME_4 = "Username4";

    public const string ID_USER_1 = "ID_USER_1";

    public const string ID_USER_2 = "ID_USER_2";

    public const string ID_USER_3 = "ID_USER_3";

    public const string ID_USER_4 = "ID_USER_4";

    public const string USERNAME = "Username";

    public const string ID_USER_HOST = "IdUserHost";

    /**
     * 
     */

    /**
   * COMMAND DEBUG GAME
   */
    public const string CM_DEBUG_GAME = "CommandDebugGame";
    /**
     * 
     */

    /**
    * USER DISCONNECT GAME
    */
    public const string CM_USER_DISCONNECT = "CommandUserDisconnect";

    public const string ID_USER_DISCONNECT = "IdUserDisconnect";

    public const string USERNAME_USER_DISCONNECT = "UsernameUserDisconnect";

    public const string LOCATION_USER_DISCONNECT = "LocationUserDisconnect";
    /**
     * 
     */

    /**
     * USER KEY
     */
    public const string USERNAME_KEY = "UsernameOfUser";

    public const string ID_USER_KEY = "IdOfUser";
    /**
     * 
     */

    /**
     * 
     */
    public const string CM_GET_USER_READY = "CommandGetUserReady";

    public const string LOCATION_1 = "LOCATION_1";

    public const string LOCATION_2 = "LOCATION_2";

    public const string LOCATION_3 = "LOCATION_3";

    public const string LOCATION_4 = "LOCATION_4";

    public const string LOCATION = "LOCATION";

    public const string SIZE_TEAM_A = "SIZE_TEAM_A";

    public const string SIZE_TEAM_B = "SIZE_TEAM_B";

    /**
     * 
     */

    /**
     * COMMANDS OF NUMBER
     */
    public const string CM_LIST_NUMBER = "CM_LIST_NUMBER";

    public const string LIST_NUMBER = "LIST_NUMBER";

    public const string HEADER_SEND = "HEADER_SEND";

    public const string HEADER_RECEIVE_NUMBER = "HEADER_RECEIVE_NUMBER";

    public const string CM_SEND_NUMBER = "CM_SEND_NUMBER";

    public const string ID_SENDER_NUMBER = "ID_SENDER_NUMBER";

    public const string LEVEL_SEND_NUMBER = "LEVEL_SEND_NUMBER";

    public const string NEXT_LOCATION_SEND_NUMBER = "NEXT_LOCATION_SEND_NUMBER";

    public const string DATA_SEND_NUMBER = "DATA_SEND_NUMBER";

    public const string CM_GAME_OVER = "CM_GAME_OVER";

    public const string ID_USER_WIN = "ID_USER_WIN";

    public const string ID_USER_CLOSE = "ID_USER_CLOSE";

    public const string LEVEL_USER_SENDER = "LEVEL_USER_SENDER";

    public const string LEVEL_USER_COMPETITOR = "LEVEL_USER_COMPETITOR";

    public const string IS_WIN_OF_SENDER = "IS_WIN_OF_SENDER";
    /**
     * 
     */

    public const string USER_PING_SERVER = "USER_PING_SERVER";

    public const string USER_GIVEUP_KEY = "USER_GIVEUP_KEY";

    /**
     * MONEY ADD
     */
    public const string CM_MONEY_ADD = "CM_MONEY_ADD";

    public const string DATA_MONEY_ADD = "DATA_MONEY_ADD";

    public const string DATA_JSON = "DATA_JSON";

    // TLMN

    /// <summary>
    /// indicate to server that user make a move
    /// </summary>
    public const string CMD_MOVE = "move";

    //----------------------------------------//
    // player click QUICK GAME
    public const string CMD_REQUEST_QUICK_GAME = "RequestQuickGame";

    // Player-is-host send all cards to server ( after shuffle and deal)
    public const string CMD_HOST_DEAL_CARD = "HostDealCard";

    // Any player put a hand
    public const string CMD_PLAYER_PUT_HAND = "PlayerPutHand";

    // Any player Super Win
    public const string CMD_SEND_SUPER_WIN = "SendSuperWin";

    // Any player pass round
    public const string CMD_PLAYER_PASS = "PlayerPass";

    // Start new round
    public const string CMD_NEW_ROUND = "NewRound";

    // Win
    public const string CMD_WIN = "Win";

    // Start
    public const string CMD_START = "Start";

    // Restart
    public const string CMD_RESTART = "Restart";

    // List Friend Request
    public const string CMD_GET_LIST_FRIEND = "GetListFriend";
}