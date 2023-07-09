using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandOnTile : MonoBehaviour
{
    public BoardManager boardManager;
    public ChanceCardManager chanceManager;
    [SerializeField] string tileBehavior = "happiness";
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
            // I feel luck
            chanceManager.DrawCard();
        }
        SquareScript sq = transform.GetComponent<SquareScript>();
        if (string.IsNullOrEmpty(sq.owner))
        {
            if (player.WantToBuy(sq.cost))
            {
                // Yoink
                player.money -= sq.cost;
                player.happiness += 5;
                sq.owner = boardManager.activePlayer.ToString();
            }
            else
            {
                // Too expensive, fuck this
            }
        }
        else
        {
            int nth = ((Int32.Parse(sq.owner) - boardManager.activePlayer) + 4) % 4;
            if (nth != 0)
            {
                PlayerScript owner;
                switch(nth)
                {
                    case 1:
                        owner = boardManager.NextPlayer();
                        break;
                    case 2:
                        owner = boardManager.OppositePlayer();
                        break;
                    default:
                        owner = boardManager.PreviousPlayer();
                        break;
                }
                player.payRent(sq.rent, owner, nth, sq.transform.position);
            }
        }
        boardManager.NextTurn();
    }

    public void ChangeTileBehavior(string behavior, int change = 0)
    {
        tileBehavior = behavior.ToLower();
        tileChange = change;
    }
}
