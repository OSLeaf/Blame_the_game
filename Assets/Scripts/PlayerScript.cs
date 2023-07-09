using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject rentAnimation;
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
        Debug.Log("Mmmmmm, tunnen sinut, UwU");
        pelaajaInfo info = FindObjectOfType<pelaajaInfo>(true); //
        Debug.Log("pelaajainfo löydetty");
        info.LoadPlayer(this); 
    }

    public void OnMouseExit()
    {
        pelaajaInfo info = FindObjectOfType<pelaajaInfo>(true); //
        info.Close();
        
    }

    public bool WantToBuy(int cost)
    {
        if (cost > money)
        {
            return false;
        }
        if ((float)cost/(float)money <= 0.2 || vitutus >= 70 || (vitutus < 30 && happiness >= 55) || luck >= 50)
        {
            return true;
        }
        return false;
    }

    public void payRent(int rent, PlayerScript owner, int nth)
    {
        relationships[nth.ToString()] -= (float)rent/(float)money * vitutus*0.25f / (happiness*0.5f) * 100f;
        Instantiate(rentAnimation, new Vector3(1,1,1), Quaternion.identity);
        money -= rent; owner.money += rent;
    }

}
