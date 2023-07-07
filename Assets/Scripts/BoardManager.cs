using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int activePlayer = 0;

    public Color[] playerColors;
    [SerializeField] private GameObject playerPiece;
    [SerializeField] private Transform boardParent;
    private Transform[] playerPieces;
    private int[] playerPositions;
    [SerializeField] private Animator diceAnim;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        playerPositions = new int[4];
        playerPieces = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            playerPieces[i] = Instantiate(playerPiece, boardParent.GetChild(0).position, Quaternion.identity, transform).transform;
            playerPieces[i].GetComponent<MeshRenderer>().material.color = playerColors[i];
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
        line.SetPosition(1, boardParent.GetChild((playerPositions[activePlayer] + count) % (boardParent.childCount - 1)).transform.position);

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
            playerPositions[activePlayer]++;
            if(playerPositions[activePlayer] >= boardParent.childCount)
            {
                playerPositions[activePlayer] = 0;
            }
            Vector3 targetPosition = boardParent.GetChild(playerPositions[activePlayer]).transform.position;
            float timer = 0;
            while(timer < 1)
            {
                playerPieces[activePlayer].transform.position = Vector3.Lerp(startPosition, targetPosition, timer) + Vector3.up * Mathf.Sin(timer * Mathf.PI);

                timer += Time.deltaTime * 2;
                yield return null;
            }   
        }
        boardParent.GetChild(playerPositions[activePlayer]).GetComponent<SquareScript>().Landed();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
