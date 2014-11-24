using UnityEngine;
using System.Collections;

/// <summary>
/// list of command help user communicate with server
/// NOTE: this is our convention
/// </summary>
public class SFCommands1 {

    /// <summary>
    /// notify to server that user has ready to play, also ask SmartFox server find another player to play game
    /// </summary>
    public const string CMD_READY = "ready";

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
