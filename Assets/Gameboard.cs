using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    private Square[] ruudut;
    public GameObject tester;
    public Square Rgoal;
    public Square Rstart;
    public Vector3 offset = new Vector3(0,0,-1.0f);
    // Start is called before the first frame update
    private List<Square> FindPath(Square start, Square goal) {
        var unvisited = new HashSet<Square>();
        var distances = new Dictionary<Square, (float,Square)>();
        foreach (var square in ruudut) {
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
            // Debug.Log($"Current {current.name}");
            foreach (var square in unvisited) {
                if (distances[square].Item1 < lowest) {
                    lowest = distances[square].Item1;
                    current = square;
                }
            }
        }
        var result = new List<Square>();
        current = goal;
        result.Add(current);
        while (current != start) {
            current = distances[current].Item2;
            result.Add(current);
        }
        result.Reverse();
        return result;
    }
    void Start()
    {
        ruudut = FindObjectsOfType<Square>();
        foreach (var square in ruudut)  {
            Instantiate(tester, square.transform.position + offset, Quaternion.identity);;
        }
        foreach (var square in FindPath(Rstart, Rgoal)) {
            Debug.Log(square.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
