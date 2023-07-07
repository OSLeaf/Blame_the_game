using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> scriptsToRunWhenLanded;
    [SerializeField] public List<SquareScript> connections; // The squares that can be continued to
    // Start is called before the first frame update
    public void Landed()
    {
        if (scriptsToRunWhenLanded.Count == 0)
        {
            FindObjectOfType<BoardManager>().NextTurn();
            return;
        }
        foreach (MonoBehaviour script in scriptsToRunWhenLanded)
        {
            script.Invoke("Play", 0);
        }
    }
}
