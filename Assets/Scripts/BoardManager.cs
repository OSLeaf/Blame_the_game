using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int activePlayer = 0;
    public Transform pieceParent;
    public Color[] playerColors;
    [SerializeField] private GameObject playerPiece;
    [SerializeField] private Transform boardParent;
    private Transform[] playerPieces;
    private SquareScript[] playerPositions;
    private SquareScript startSquare;
    public PlayerScript[] playerValues;
    [SerializeField] private Animator diceAnim;
    private LineRenderer line;

    SquareScript GetNSquaresForwardPrimary(SquareScript from, int N) {
        // stay in the last square
        var current = from;
        for (int i = 0; i < N; i++) {
            if (current.connections.Count > 0) {
                current = current.connections[0];
            }
        }
        return current;
    }

    private List<SquareScript> FindPath(SquareScript start, SquareScript goal) {
        // Will break horribly if goal is not reachable from start!!!
        // Should instead add nodes to unvisited via neighbors dynamically.
        var unvisited = new HashSet<SquareScript>();
        var distances = new Dictionary<SquareScript, (float, SquareScript)>();
        foreach (var square in FindObjectsOfType<SquareScript>()) {
            distances.Add(square, (Mathf.Infinity,start));
            unvisited.Add(square);
        }        
        var current = start;
        distances[current] = (0,start);
        int helper = 0;
        while (unvisited.Count > 0 && unvisited.Contains(goal)) {
            helper += 1;
            if (helper > 10) {
                break;
            }
            unvisited.Remove(current);
            foreach (var conncetion in current.connections) {
                if (distances[conncetion].Item1 > distances[current].Item1 + 1) {
                    distances[conncetion] = (distances[current].Item1+1, current);
                }
            }
            float lowest = Mathf.Infinity;
            foreach (var square in unvisited) {
                if (distances[square].Item1 < lowest) {
                    lowest = distances[square].Item1;
                    current = square;
                }
            }
        }
        var result = new List<SquareScript>();
        current = goal;
        result.Add(current);
        while (current != start) {
            current = distances[current].Item2;
            result.Add(current);
        }
        result.Reverse();
        return result;
    }

    public PlayerScript CurrentPlayer()
    {
        return playerValues[activePlayer];
    }
    public PlayerScript NextPlayer()
    {
        return playerValues[(activePlayer + 1) % 4];
    }
    public PlayerScript PreviousPlayer()
    {
        return playerValues[(activePlayer + 3) % 4];
    }
    public PlayerScript OppositePlayer()
    {
        return playerValues[(activePlayer + 2) % 4];
    }

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        playerPositions = new SquareScript[4];
        playerPieces = new Transform[4];
        playerValues = new PlayerScript[4];

        // Form a graph of the game board squares
        startSquare = boardParent.GetChild(0).GetComponent<SquareScript>();
        startSquare.connections = new List<SquareScript>{boardParent.GetChild(1).GetComponent<SquareScript>()};
        for (int i = 1; i < boardParent.childCount - 1; i++)
        {
            boardParent.GetChild(i).GetComponent<SquareScript>().connections = new List<SquareScript> {boardParent.GetChild(i+1).GetComponent<SquareScript>()};
        }
        boardParent.GetChild(boardParent.childCount - 1).GetComponent<SquareScript>().connections = new List<SquareScript> {startSquare};


        for (int i = 0; i < 4; i++)
        {
            playerPositions[i] = startSquare;
            playerPieces[i] = Instantiate(playerPiece, startSquare.transform.position + Vector3.left * 0.5f * i, Quaternion.identity, pieceParent).transform;
            playerPieces[i].GetComponent<MeshRenderer>().material.color = playerColors[i];
            playerValues[i] = GameObject.FindObjectOfType<PlayerScript>();
        }

    }

    public void NextTurn()
    {
        activePlayer++;
        if (activePlayer > 3)
            activePlayer = 0;
        diceAnim.SetTrigger("DiceRoll");
    }
    public void DiceHower(int count)
    {
        line.startColor = playerColors[activePlayer];
        line.endColor = line.startColor;
        line.enabled = true;
        line.SetPosition(0, playerPieces[activePlayer].transform.position);
        line.SetPosition(1, GetNSquaresForwardPrimary(playerPositions[activePlayer], count).transform.position);

    }
    public void DiceHowerEnd()
    {

        line.enabled = false;
    }
    public void DiceRoll(int count)
    {
        line.enabled = false;
        diceAnim.SetTrigger("RollDone");
        StartCoroutine(MoveCoroutine(count));
    }
    IEnumerator MoveCoroutine(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Vector3 startPosition = playerPieces[activePlayer].transform.position;
            playerPositions[activePlayer] = GetNSquaresForwardPrimary(playerPositions[activePlayer], 1);
            Vector3 targetPosition = playerPositions[activePlayer].transform.position;

            int playersOnSameSquare = 0;
            foreach(Transform player in playerPieces)
            {
                if((player.position - targetPosition).magnitude < 0.75f)
                {
                    playersOnSameSquare++;
                }
            }
            if(playersOnSameSquare == 1)
            {
                targetPosition += Vector3.right * 0.5f;
                targetPosition += Vector3.forward * 0.5f;
            }
            else if (playersOnSameSquare == 2)
            {
                targetPosition += Vector3.right * 0.5f;
                targetPosition += Vector3.forward * -0.5f;
            }
            else if (playersOnSameSquare == 3)
            {
                targetPosition += Vector3.right * -0.5f;
                targetPosition += Vector3.forward * -0.5f;
            }

            float timer = 0;
            while(timer < 1)
            {
                playerPieces[activePlayer].transform.position = Vector3.Lerp(startPosition, targetPosition, timer) + Vector3.up * Mathf.Sin(timer * Mathf.PI);
                timer += Time.deltaTime * 10;
                yield return null;
            }   
        }

        playerPositions[activePlayer].Landed();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
