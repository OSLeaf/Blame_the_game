using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandOnTile : MonoBehaviour
{
    public BoardManager boardManager;
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
        player.happiness += 1;
        boardManager.NextTurn();
    }
}
