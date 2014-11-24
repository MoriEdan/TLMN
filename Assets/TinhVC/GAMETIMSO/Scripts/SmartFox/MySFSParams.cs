using UnityEngine;
using System.Collections;

/// <summary>
/// list of params may receive from server in convention
/// </summary>
public class MySFSParams
{

    #region Params for handle server response
    /// <summary>
    /// get commnad from server
    /// e/g: game_started, game_end, won, lose, tie
    /// </summary>
    public const string PARAM_CMD = "cmd";

    /// <summary>
    /// get data sent from server
    /// </summary>
    public const string PARAM_DATA = "params";
    #endregion

    #region Params for possible event

    /// <summary>
    /// e/g: SFSEvent.CONNECTION: indicate whether user connect successful or not
    /// </summary>
    public const string PARAM_SUCCESS = "success";

    /// <summary>
    /// get possible matter/error occured
    /// </summary>
    public const string PARAM_ERR_MGS = "errorMessage";

    /// <summary>
    /// get possible reason
    /// </summary>
    public const string PARAM_REASON = "reason";

    /// <summary>
    /// get user has fired the event
    /// e/g: SFSEvent.USER_ENTER_ROOM | SFSEvent.USER_EXIT_ROOM | SFSEvent.USER_COUNT_CHANGE
    /// </summary>
    public const string PARAM_USER = "user";

    /// <summary>
    /// get room related to event
    /// e/g: SFSEvent.ROOM_JOIN
    /// </summary>
    public const string PARAM_ROOM = "room";
    #endregion
}
