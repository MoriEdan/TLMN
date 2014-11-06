using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{

    public enum GameState
    {
        Idle,
        Deal,
        Play,
        End
    }

    public enum CardState
    {
        Idle,
        OnHand,
        Putted,
        Fold,
        Destroy
    }

    public enum PlayerState
    {
        Idle,
        Play,
        Pass,
        Win,
        Lose,
        Pre
    }
}

