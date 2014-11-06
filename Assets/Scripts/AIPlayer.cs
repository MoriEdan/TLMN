using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class AIPlayer : Player {

    // Use this for initialization
    private bool hasPut;
    private float putCardTimer;
    
    void Start()
    {
        hasPut = false;
        deck = Instantiate(deckPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Deck2;
        putCardTimer = Constants.AI_DELAY_TIME_TO_PUT_CARD;
    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        {
            case PlayerState.Play:
                {
                    if(putCardTimer < 0.0f)
                    {
                        if (GameManager.RoundPlayer == GameManager.CurrentPlayer)
                        {
                            GameManager.CurrentHand = new Hand(new List<Card>());
                            GameManager.NewRound(GameManager.CurrentPlayer, GameManager);
                        }
                        Hand hand = GetMove();
                        if (hand != null)
                        {
                            PutCard(hand);
                            GameManager.RoundPlayer = GameManager.CurrentPlayer;
                            GameManager.SetIdle(GameManager.CurrentPlayer);
                            GameManager.CheckWin(GameManager.CurrentPlayer);
                        }
                        else
                        {
                            GameManager.SetPass(GameManager.CurrentPlayer);
                        }
                        putCardTimer = Constants.AI_DELAY_TIME_TO_PUT_CARD;
                        GameManager.SetPlay();    
                    }
                    else
                    {
                        putCardTimer -= Time.deltaTime;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

   

    // Find moves suitable
    public override Hand GetMove()
    {
        List<Hand> hands = LegalMoves();
        if(hands.Count > 0)
        {
            Hand selectedHand = hands[0];
            if (!selectedHand.Type.Equals("INVALID")
                && this == GameManager.CurrentPlayer)
            {
                return selectedHand;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    // Get & Set
    public bool HasPut
    {
        get { return hasPut; }
        set { hasPut = value; }
    }
}
