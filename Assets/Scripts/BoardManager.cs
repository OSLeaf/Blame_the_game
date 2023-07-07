using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int activePlayer = 0;
    
    [SerializeField] private GameObject playerPiece;
    [SerializeField] private Transform boardParent;
    private Transform[] playerPieces;
    private SquareScript[] playerPositions;
    private SquareScript startSquare;
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

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        playerPositions = new SquareScript[4];
        playerPieces = new Transform[4];
        startSquare = boardParent.GetChild(0).GetComponent<SquareScript>();
        startSquare.connections = new List<SquareScript>();
        startSquare.connections.Add(boardParent.GetChild(1).GetComponent<SquareScript>());
        for (int i = 1; i < boardParent.childCount - 1; i++)
        {
            boardParent.GetChild(i).GetComponent<SquareScript>().connections = new List<SquareScript>();
            boardParent.GetChild(i).GetComponent<SquareScript>().connections.Add(boardParent.GetChild(i+1).GetComponent<SquareScript>());
        }
        for (int i = 0; i < 4; i++)
        {
            playerPositions[i] = startSquare;
            playerPieces[i] = Instantiate(playerPiece, startSquare.transform.position, Quaternion.identity, transform).transform;
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
            if (playerPositions[activePlayer].connections.Count > 0) {
                // Take primary route
                playerPositions[activePlayer] = playerPositions[activePlayer].connections[0];
            }
            Vector3 targetPosition = playerPositions[activePlayer].transform.position;
            float timer = 0;
            while(timer < 1)
            {
                playerPieces[activePlayer].transform.position = Vector3.Lerp(startPosition, targetPosition, timer) + Vector3.up * Mathf.Sin(timer * Mathf.PI);

                timer += Time.deltaTime * 2;
                yield return null;
            }
              
        }

        playerPositions[activePlayer].GetComponent<SquareScript>().Landed();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
