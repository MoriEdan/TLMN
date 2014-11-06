using UnityEngine;
using System.Collections.Generic;
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

    public static void SendReadyMsg(SFSObject mParams)
    {
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_REQUEST_QUICK_GAME, mParams));
        //Debug.Log("ready msg sent");
    }

    // Player-is-host send 4 array of cards to server :D :D (after shuffle and deal)

    public static void SendFourListCardsToServer(List<Card> allCards)
    {
        ISFSObject mParams = new SFSObject();
        //for(int i = 0 ; i < Constants.MAX_NUMBER_PLAYER; i++)
        //{
        //    ByteArray byteArray = new ByteArray();
        //    for(int j = 0; j < Constants.CARD_AMOUNT_FOR_EACH_PLAYER; j ++)
        //    {
        //        byteArray.Bytes[j] = (byte)players[i].Deck.Cards[j].Index;
        //    }
        //    mParams.PutByteArray("Player" + i + "Cards", byteArray);
        //}

        byte[] bArray = new byte[52];
        ByteArray byteArray = new ByteArray(bArray);
        for(int i = 0 ; i< Constants.CARD_AMOUNT; i++)
        {
            byteArray.Bytes[i] = (byte)allCards[i].Index;
        }
        mParams.PutByteArray("AllCards", byteArray);
       
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_HOST_DEAL_CARD, mParams));
    }


    // When a player put CARD!!!!!!
    public static void SendHandToServer(Hand hand, int order)
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
        for(int i = 0; i < Constants.HAND_TYPE.Length; i++)
        {
            if(Constants.HAND_TYPE[i].Equals(hand.Type))
            {
                mParams.PutByte("Type", (byte)i);
            }
        }
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_PLAYER_PUT_HAND, mParams));
    }

    public static void SendPassToServer(int order)
    {
        ISFSObject mParams = new SFSObject();
        mParams.PutByte("PlayerIndex", (byte)order);
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_PLAYER_PASS, mParams));
    }
    public static void SendNewRoundToServer(int order)
    {
        ISFSObject mParams = new SFSObject();
        mParams.PutByte("PlayerIndex", (byte)order);
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_NEW_ROUND, mParams));
    }

    public static void SendNewRoundMessage(int order)
    {
        ISFSObject mParams = new SFSObject();
        mParams.PutByte("head", 0);
        mParams.PutByte("PlayerIndex", (byte)order);
        smartFox.Send(new ObjectMessageRequest(mParams));
    }

    //Send Win to server
    public static void SendWinToServer(int order, int winOrder)
    {
        ISFSObject mParams = new SFSObject();
        mParams.PutByte("PlayerIndex", (byte)order);
        mParams.PutByte("WinOrder", (byte)winOrder);
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_WIN, mParams));
    }

    // Send start (real start :))) to server
    public static void SendStartCommandToServer()
    {
        ISFSObject mParams = new SFSObject();
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_START, mParams));
    }

    public static void SendRestartToServer(int winPlayerOrder)
    {
        ISFSObject mParams = new SFSObject();
        mParams.PutByte("WinPlayerOrder", (byte)winPlayerOrder);
        smartFox.Send(new ExtensionRequest(SFCommands.CMD_RESTART, mParams));
    }

    // Handle disconnection automagically
    void OnApplicationQuit()
    {
        if (smartFox.IsConnected)
        {
            smartFox.Disconnect();
        }
    }
}
