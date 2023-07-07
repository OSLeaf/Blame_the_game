using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Dictionary<string, float> stats = new Dictionary<string, float>(){
        {"happiness", 50},
        {"vitutus", 25},
        {"luck", 35}
    };
    public Dictionary<string, float> relationships = new Dictionary<string, float>(){
        {"1", 50},
        {"2", 50},
        {"3", 50}
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    } 

    public void ChangeAStat(string statToChange, float change)
    {
        stats[statToChange] += change;
    }

    public void ChangeRelationshipWithPlayer(string player, float change)
    {
        if (!relationships.ContainsKey(player))
        {
            return;
        }
        relationships[player] += change;
    }
}
