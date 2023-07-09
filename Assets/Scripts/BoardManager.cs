using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public int activePlayer = 0;
    public Transform pieceParent;
    public Color[] playerColors;
    [SerializeField] private GameObject playerPiece;
    [SerializeField] private Transform boardParent;
    [SerializeField] private SquareScript startSquare;
    private Transform[] playerPieces;
    private SquareScript[] playerPositions;
    public PlayerScript[] playerValues;
    [SerializeField] private Animator diceAnim;
    private LineRenderer line;

    private Vector3[] possibleOffsets = {Vector3.zero, new Vector3(0.5f,0,0.5f), new Vector3(0.5f,0,-0.5f), new Vector3(-0.5f, 0, 0.5f)};

    public GameObject objectiveText;
    public GameObject currentTurnText;
    int currentTurn = 1;
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

    public PlayerScript nthPlayer(int n)
    {
        return playerValues[(activePlayer + n) % 4];
    }

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        playerPositions = new SquareScript[4];
        playerPieces = new Transform[4];
        playerValues = new PlayerScript[4];
        objectiveText.GetComponent<TextMeshProUGUI>().text = "No objective... Yet";
        currentTurnText.GetComponent<TextMeshProUGUI>().text = "Current turn: 1";

        for (int i = 0; i < 4; i++)
        {
            playerPositions[i] = startSquare;
            playerPieces[i] = Instantiate(playerPiece, startSquare.transform.position + possibleOffsets[i], Quaternion.identity, pieceParent).transform;
            playerPieces[i].GetComponent<MeshRenderer>().material.color = playerColors[i];
            playerValues[i] = GameObject.FindObjectOfType<PlayerScript>();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) {
                SceneManager.LoadScene("Titlescreen");
            } else {
                Application.Quit();
            }
        }
    }

    public void NextTurn()
    {
        try
        {
            UpdatePlayerStats();
        }
        catch (System.Exception e) 
        {
            
        }   



        activePlayer++;
        if (activePlayer > 3)
        {
            activePlayer = 0;
            currentTurn += 1;
            currentTurnText.GetComponent<TextMeshProUGUI>().text = "Current turn: " + currentTurn.ToString();
            if (currentTurn == 5)
            {
                startObjective();
            }
        }
        diceAnim.SetTrigger("DiceRoll");
    }

    private void UpdatePlayerStats()
    {
        int i;
        int[] money = (int[])playerValues.Select(p => p.money);
        int suurin = money.Max();
        int suurinInd = money.ToList().IndexOf(suurin);
        List<int> vihattavat = new List<int>();
        List<int> saalittavat = new List<int>();


        for (i = 0; i < 4; i++)
        {
            if (money[i] >= suurin * 0.75)
            {
                playerValues[i].happiness += 5;
                playerValues[i].vitutus -= 3;
                if (i == suurinInd)
                {
                    playerValues[i].happiness += 5;
                }
                vihattavat.Add(i);
            } 
            else if (money[i] <= suurin * 0.25)
            {
                foreach (int saa in saalittavat)
                {
                    playerValues[saa].relationships["" + ((i - saa + 4) % 4)] += 5;
                }
                playerValues[i].happiness -= 2;
                playerValues[i].vitutus += 5;
                foreach (int vih in vihattavat)
                {
                    string id = "" + ((vih - i + 234632) % 4);
                    playerValues[i].relationships[id] -= 5;
                }
                saalittavat.Add(i);
                foreach (int saa in saalittavat)
                {
                    playerValues[i].relationships["" + ((saa - i + 4) % 4)] += 5;
                }
            } 
            else
            {
                foreach (int vih in vihattavat)
                {
                    string id = "" + ((vih - i + 497524) % 4);
                    playerValues[i].relationships[id] -= 6;
                }
                int j;
                for ( j = 0; j<4; j++)
                {
                    string id = "" + ((j - i + 497524) % 4);
                    playerValues[i].relationships[id] += 2;
                }
            }
        }
    }


    private void startObjective()
    {
        objectiveText.GetComponent<TextMeshProUGUI>().text = "Current objective: Achieve Communism\nEvery player must have the same amount of money";
    }
    public void DiceHower(int count)
    {
        line.startColor = playerColors[activePlayer];
        line.endColor = line.startColor;
        line.enabled = true;
        var pos1 = playerPieces[activePlayer].transform.position;
        var pos2 = GetNSquaresForwardPrimary(playerPositions[activePlayer], count).transform.position;
        line.SetPosition(0, pos1+0.5f*Vector3.up);
        line.SetPosition(1, (pos1+pos2)*0.5f+1.5f*Vector3.up);
        line.SetPosition(2, pos2+0.5f*Vector3.up);

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

            for (int j = 0; j < 4; j++) {
                var collision = false;
                foreach (var player in playerPieces) {
                    if (player!=playerPieces[activePlayer] && (player.transform.position-(targetPosition+possibleOffsets[j])).magnitude < 0.2) {
                        collision = true;
                        break;
                    }
                }
                if (!collision) {
                    targetPosition += possibleOffsets[j];
                    break;
                }
            }

            float timer = 0;
            while(timer < 1)
            {
                playerPieces[activePlayer].transform.position = Vector3.Lerp(startPosition, targetPosition, timer) + Vector3.up * Mathf.Sin(timer * Mathf.PI)*3f;
                timer += Time.deltaTime * 5;
                yield return null;
            }
            playerPieces[activePlayer].transform.position = targetPosition;
        }

        playerPositions[activePlayer].Landed();
    }
}
