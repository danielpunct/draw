using System;
using Gamelogic.Extensions;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        Game.Instance.ShowLevel(0);
    }

    public void OnContourPassed()
    {
        if (!Game.Instance.LoadNextPiece())
        {
            Game.Instance.LoadNextLevel();
        }
    }


    public void OnContourFailed()
    {
        
    }
}