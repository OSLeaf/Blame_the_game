using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandOnTile : MonoBehaviour
{
    public BoardManager boardManager;
    string tileBehavior = "happiness";
    int tileChange = 1;
    // Start is called before the first frame update
    void Start()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();
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
        boardManager.NextTurn();
    }

    public void ChangeTileBehavior(string behavior, int change = 0)
    {
        tileBehavior = behavior.ToLower();
        tileChange = change;
    }
}
