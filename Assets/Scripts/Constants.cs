﻿using UnityEngine;
using System.Collections;

public class Constants
{

    // Number of cards
    public const int CARD_AMOUNT = 52;

    // Cards number for each player
    public const int CARD_AMOUNT_FOR_EACH_PLAYER = 13;

    // 4 kind of suit
    public static readonly string[] SUITS = { "Heart", "Diamond", "Club", "Spade" };

    // 13 value of card
    public static readonly string[] VALUES = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    // true Value =))
    public static readonly int[] NUMBER_VALUES = { 14, 15, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
    public static readonly int[] NUMBER_SUITS = { 4, 3, 2, 1 };

    // Type of hand
    public static readonly string[] HAND_TYPE = {"Single", "Pair", "ThreeOfAKind", "FourOfAKind", "Straight3", "Straight4",
                                                    "Straight5", "Straight6", "Straight7", "Straight8", "Straight9", "Straight10",
                                                    "Straight11", "Straight12", "Straight13", "Cut", "SuperCut"};

    // Time to deal one card
    public const float TIME_TO_DEAL_ONE_CARD = 0.3f;

    // Card margin horizontal;
    public const float CARD_MARGIN_HORIZONTAL = 1.0f;

    // Card margin vertical;
    public const float CARD_MARGIN_VERTICAL = 0.5f;

    // Card Move Up Distance
    public const float CARD_MOVE_UP_DISTANCE = 0.5f;

    // Time to sort one card
    public const float TIME_TO_SORT_ONE_CARD = 0.3f;

    // Time to put card
    public const float TIME_TO_PUT_CARD = 0.1f;

    // Number of position for putting card
    public const int NUMBER_POSITION_TO_PUT_CARD = 4;

    // Max player in room
    public const int MAX_NUMBER_PLAYER = 4;

    // Delay time for put card;
    public const float AI_DELAY_TIME_TO_PUT_CARD = 1.0f;

    // Delay time for drag card
    public const float DELAY_TIME_TO_DRAG_CARD = 0.3f;

    // Time countdown for start playing
    public const int COUNT_DOWN_TIME_FOR_STARTING = 5;

    // Time countdown per turn
    public const float COUNT_DOWN_TIME_PER_TURN = 15.0f;

}