using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour {

    public SFSGameRoom gameManager;
    public Deck2 deck;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Btn Deal Card Click
    void DealCard()
    {
        //gameManager.GameState = GameState.Deal;
        //StartCoroutine(gameManager.DealCards(gameManager.AllCards));
    }

    void Sort()
    {
        //gameManager.GameState = GameState.Sort;
        //gameManager.Sort(gameManager.players[gameManager.Order].Deck.Cards, gameManager.Order);
        Hand hand = gameManager.players[gameManager.Order].QuickFindHand();
        for(int i = 0; i < hand.CardCount(); i++)
        {
            hand.Cards[i].MouseUpHandler();
        }
    }

    void SendHandToServer()
    {
        //if ((gameManager.RoundPlayer.Name.Equals(gameManager.CurrentPlayer.Name)) && gameManager.CurrentHand != null)
        //{
        //    gameManager.CurrentHand = new Hand(new List<Card>());
        //    gameManager.NewRound(gameManager.players[gameManager.Order], gameManager);
        //    SmartFoxConnection.SendNewRoundMessage(gameManager.Order);
        //}
        if (gameManager.players[gameManager.Order].State != PlayerState.Win)
        {
            Hand putHand = gameManager.players[gameManager.Order].GetMove();
            if (putHand != null)
            {
                SmartFoxConnection.SendHandToServer(putHand, gameManager.Order);
            }
        }
    }

    void SendPassToServer()
    {
        if (gameManager.players[gameManager.Order].State != PlayerState.Win)
        {
            if (gameManager.players[gameManager.Order].IsCurrentPlayer())
            {
                SmartFoxConnection.SendPassToServer(gameManager.Order);
            }
        }
    }


    void Quit()
    {
        SmartFoxConnection.Connection.Disconnect();
        Application.LoadLevel("TLMN_Menu");
    }

    void Put()
    {
        if(gameManager.RoundPlayer == gameManager.CurrentPlayer)
        { 
            gameManager.CurrentHand = new Hand(new List<Card>());
            gameManager.NewRound(gameManager.CurrentPlayer, gameManager);            
        }
        //gameManager.PutCard(gameManager.FindCardOnHand());
        Hand putHand = gameManager.CurrentPlayer.GetMove();
        if(putHand != null)
        {
            gameManager.CurrentPlayer.PutCard(putHand);
            gameManager.RoundPlayer = gameManager.CurrentPlayer;
            gameManager.SetIdle(gameManager.CurrentPlayer);
            gameManager.CheckWin(gameManager.CurrentPlayer);
            gameManager.SetPlay();
        }
    }

    void Pass()
    {
        gameManager.SetPass(gameManager.CurrentPlayer);
        gameManager.SetPlay();
    }

}
