using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;
using Sfs2X.Entities.Data;
using Sfs2X.Util;


// Statics for holding the connection to the SFS server end
// Can then be queried from the entire game to get the connection

public class SmartFoxConnection : MonoBehaviour
{

    private static SmartFoxConnection mInstance;
    private static SmartFox smartFox;
    private static SFSObject persistentData;
    public static SmartFox Connection
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
            }
            return smartFox;
        }

        set
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
            }
            smartFox = value;
        }
    }
    public static SFSObject PersistentData
    {
        get
        {
            return persistentData;
        }

        set
        {
            persistentData = value;
        }
    }

    public static bool IsInitialized
    {
        get
        {
            return (smartFox != null);
        }
    }

    public static void SendReadyMsg(SFSObject mParams ,string sCommand)
    {
        smartFox.Send(new ExtensionRequest(sCommand, mParams));
        Debug.Log("ready msg sent");
    }

    public static void SendReadyMsg(SFSObject mParams)
    {
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_REQUEST_QUICK_GAME, mParams));
        //Debug.Log("ready msg sent");
    }


    public static void Connect(SmartFox sfsConnection)
    {
        if (!sfsConnection.IsConnected)
        {
            sfsConnection.Connect(ServerDefine.SERVER_NAME, ServerDefine.SERVER_PORT);
            Debug.Log("connect success");
        }
    }

    public static void Login(SmartFox sfsConnection, string userName)
    {

       // string userName = SystemInfo.deviceName + SystemInfo.deviceUniqueIdentifier;
        //userName = "tinh";
        VariableApplication.sUserNameSFS = userName;
        sfsConnection.Send(new LoginRequest(userName, "timso_lama", ServerDefine.ZONE));
    }

    // Handle disconnection automagically
    void OnApplicationQuit()
    {
        if (smartFox != null && smartFox.IsConnected)
        {
            smartFox.Disconnect();
        }
    }

    // TLMN
    // When a player put CARD!!!!!!
    public static void SendHandToServer(Hand hand, int order, bool isSuperWin)
    {
        ISFSObject mParams = new SFSObject();
        byte[] bArray = new byte[hand.CardCount()];
        ByteArray byteArray = new ByteArray(bArray);
        for (int i = 0; i < hand.CardCount(); i++)
        {
            byteArray.Bytes[i] = (byte)hand.Cards[i].Index;
        }
        mParams.PutByteArray("Hand", byteArray);
        mParams.PutByte("PlayerIndex", (byte)order);
        for (int i = 0; i < Constants.HAND_TYPE.Length; i++)
        {
            if (Constants.HAND_TYPE[i].Equals(hand.Type))
            {
                mParams.PutByte("Type", (byte)i);
            }
        }
        if (!isSuperWin)
        {
            smartFox.Send(new ExtensionRequest(SFCommands.CMD_PLAYER_PUT_HAND, mParams));
        }
        else
        {
            smartFox.Send(new ExtensionRequest(SFCommands.CMD_SEND_SUPER_WIN, mParams));
        }

    }
    public static void SendPassToServer(int order)
    {
        ISFSObject mParams = new SFSObject();
        mParams.PutByte("PlayerIndex", (byte)order);
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_PLAYER_PASS, mParams));
    }

    public static void SendRestartToServer()
    {
        ISFSObject mParams = new SFSObject();
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_RESTART, mParams));
    }

    public static void SendGetFriendListRequest()
    {
        ISFSObject mParams = new SFSObject();
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_GET_LIST_FRIEND, mParams));
    }
}
