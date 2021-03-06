﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    public static GameView GetInstance()
    {
        if (instance==null)
        {
            instance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameView>();
        }
        return instance;
    }
    private static GameView instance;

    public GameObject FieldPrefab;
    public GameObject CheckerPrefab;
    public GameObject GameEndUI;

    private Vector3 GetFieldPosition(int x, int y, bool isChecker)
    {
        SpriteRenderer renderer = FieldPrefab.GetComponent<SpriteRenderer>();
        float fieldSize = renderer.sprite.bounds.size.x * FieldPrefab.transform.localScale.x;
        float startFieldPosition = -(fieldSize * GameModel.GetInstance().BoardSize / 2);
        return new Vector3(startFieldPosition + x * fieldSize, startFieldPosition + y * fieldSize,(isChecker ? -1.0f : 0.0f));
    }
    // Start is called before the first frame update
    public void Start()
    {
        for(int y=0;y<GameModel.GetInstance().BoardSize;y++)
        {
            for(int x=0;x<GameModel.GetInstance().BoardSize;x++)
            {
                SpriteRenderer newField = (Instantiate(FieldPrefab, GetFieldPosition(x, y, false), Quaternion.identity) as GameObject).GetComponent<SpriteRenderer>();
                newField.GetComponent<FieldData>().X = x;
                newField.GetComponent<FieldData>().Y = y;
                if((x%2==0 && y%2==0) || ( x % 2 == 1 && y % 2 ==1))
                { 
                    newField.color = Color.grey;
                    if (y < GameModel.GetInstance().NumCheckersRows)
                    {
                        GameObject checker = Instantiate(CheckerPrefab, GetFieldPosition(x, y, true), Quaternion.identity) as GameObject;
                        GameModel.GetInstance().RegiesterCheckerData(checker.GetComponent<CheckerData>(), new Vec2(x, y));
                    }
                    else if(y>=GameModel.GetInstance().BoardSize-GameModel.GetInstance().NumCheckersRows)
                    {
                        GameObject checker = Instantiate(CheckerPrefab, GetFieldPosition(x, y, true), Quaternion.identity) as GameObject;
                        checker.GetComponent<SpriteRenderer>().color = Color.black;
                        GameModel.GetInstance().RegiesterCheckerData(checker.GetComponent<CheckerData>(), new Vec2(x, y));
                    }
        
                }
            }
        }
    }

    public void HighlightChecker(CheckerData toHighlight, CheckerData previouslyHighlited=null)
    {
        if(previouslyHighlited)
        {
            previouslyHighlited.gameObject.GetComponent<SpriteRenderer>().color = (previouslyHighlited.Owner == GameController.GetInstance().Players[0] ? Color.white : Color.black);
        }
        if(toHighlight)
        {
            toHighlight.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyChecker(CheckerData checker)
    {
        Destroy(checker.gameObject);
    }

    public void MoveChecker(CheckerData checker, Vec2 target)
    {
        checker.gameObject.transform.position = GetFieldPosition(target.x, target.y, true);
    }
}
