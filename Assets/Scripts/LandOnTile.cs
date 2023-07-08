using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandOnTile : MonoBehaviour
{
    public BoardManager boardManager;
    public ChanceCardManager chanceManager;
    string tileBehavior = "happiness";
    int tileChange = 1;
    // Start is called before the first frame update
    void Start()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();
        chanceManager = GameObject.FindAnyObjectByType<ChanceCardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        var player = boardManager.CurrentPlayer();
        if (tileBehavior == "happiness")
        {
            player.happiness += tileChange;
        }
        else if (tileBehavior == "luck")
        {
            player.luck += tileChange;
        }
        else if (tileBehavior == "chance")
        {
            chanceManager.DrawCard();
        }
        boardManager.NextTurn();
    }

    public void ChangeTileBehavior(string behavior, int change = 0)
    {
        tileBehavior = behavior.ToLower();
        tileChange = change;
    }
}
