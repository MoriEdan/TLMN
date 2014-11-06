using UnityEngine;
using System.Collections;

/// <summary>
/// list of command help user communicate with server
/// NOTE: this is our convention
/// </summary>
public class SFCommands {

    /// <summary>
    /// notify to server that user has ready to play, also ask SmartFox server find another player to play game
    /// </summary>
    public const string CMD_READY = "ready";

    /// <summary>
    /// indicate to server that user make a move
    /// </summary>
    public const string CMD_MOVE = "move";

    // When user control their character
    public const string CMD_CHAR_MOVE = "charMove";

    // When elephant's symbol change
    public const string CMD_SYMBOL_CHANGE = "symbolChange";

    // When coconut leaf change
    public const string CMD_LEAF_CHANGE = "leafChange";

    // When user spawn a elephant
    public const string CMD_SPAWN_NORMAL = "spawnNormal";
    public const string CMD_SPAWN_COCONUT = "spawnCoconut";

    // when host spawn a bird
    public const string CMD_SPAWN_BIRD = "spawnBird";

    // when random drop an item
    public const string CMD_DROP_ITEM = "dropItem";


    //----------------------------------------//
    // player click QUICK GAME
    public const string CMD_REQUEST_QUICK_GAME = "RequestQuickGame";

    // Player-is-host send all cards to server ( after shuffle and deal)
    public const string CMD_HOST_DEAL_CARD = "HostDealCard";

    // Any player put a hand
    public const string CMD_PLAYER_PUT_HAND = "PlayerPutHand";

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
}
