﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public static GameController GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        }
        return instance;
    }
    static private GameController instance;

    static public readonly string CheckerTag = "Checker";

    static public readonly string FieldTag = "Field";

    static public readonly string Player1Name = "Player1";

    static public readonly string Player2Name = "Player2";

    public Player CurrentPlayer
    {
        get
        {
            return Players[CurrentPlayerIdx];
        }
    }
    
    public Player[] Players = new Player[2];

    public int CurrentPlayerIdx = 0;

    // Start is called before the first frame update
    public void Start()
    {
        Players[0] = GameObject.Find("Player1").GetComponent<Player>();
        Players[0].Direction = 1;

        Players[1] = GameObject.Find("Player2").GetComponent<Player>();
        Players[1].Direction = -1;

        for (int y = 0; y < GameModel.GetInstance().BoardSize; y++)
        {
            for (int x = 0; x < GameModel.GetInstance().BoardSize; x++)
            {
                
                if ((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1))
                {
                    
                    if (y < GameModel.GetInstance().NumCheckersRows)
                    {
                        GameModel.GetInstance().Board[x, y].Owner = Players[0];
                    }
                    else if (y >= GameModel.GetInstance().BoardSize - GameModel.GetInstance().NumCheckersRows)
                    {
                        GameModel.GetInstance().Board[x, y].Owner = Players[1];
                    }

                }
            }
        }
        GameModel.GetInstance().UpdatePossbileMoves();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPlayer.ProcessTurn();
    }

    public void NextTurn()
    {
        CurrentPlayerIdx = 1 - CurrentPlayerIdx;
        GameModel.GetInstance().UpdatePossbileMoves();

        int winner = GameModel.GetInstance().CheckVictory();

        if(winner!=-1)
        {
            Debug.Log("Wygrywa gracz " + winner);
            GameView.GetInstance().GameEndUI.SetActive(true);
            GameView.GetInstance().GameEndUI.GetComponent<GameEndView>().SetWinnerText(winner);
        }
    }
}
