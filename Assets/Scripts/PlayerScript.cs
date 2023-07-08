using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float happiness = 50;
    public float vitutus = 25;
    public float luck = 35;
    public int money = 2000;
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

    public void ChangeRelationshipWithPlayer(string player, float change)
    {
        if (!relationships.ContainsKey(player))
        {
            return;
        }
        relationships[player] += change;
    }

    public void OnMouseEnter()
    {
        Debug.Log("tunnen sinut");
        pelaajaInfo info = FindObjectOfType<pelaajaInfo>(true); //
        Debug.Log("pelaajainfo löydetty");
        info.LoadPlayer(this);
       
    }

    public void OnMouseExit()
    {
        pelaajaInfo info = FindObjectOfType<pelaajaInfo>(true); //
        info.Close();
        
    }

}
