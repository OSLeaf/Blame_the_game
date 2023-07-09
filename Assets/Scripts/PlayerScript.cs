using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject rentAnimation;
    private float _happiness = 50;
    public float happiness
    {
        get { return _happiness; }
        set {
            _happiness = value;
            if (_happiness < 0) { _happiness = 0; }
            if (_happiness > 100) { _happiness = 100; }
        }
    }


    private float _vitutus = 35;
    public float vitutus
    {
        get { return _vitutus; }
        set
        {
            _vitutus = value;
            if (_vitutus < 0) { _vitutus = 0; }
            if (_vitutus > 100) { _vitutus = 100; }
        }
    }


    private float _luck = 35;
    public float luck
    {
        get { return _luck; }
        set
        {
            _luck = value;
            if (_luck < 0) { _luck = 0; }
            if (_luck > 100) { _luck = 100; }
        }
    }


    private int _money = 2000;
    public int money
    {
        get { return _money; }
        set
        {
            _money = value;
            if (_money < 0) { money = 0; }
        }
    }



    private Dictionary<string, float> _relationships = new Dictionary<string, float>(){
        {"1", 50},
        {"2", 50},
        {"3", 50}
    };
    public Dictionary<string, float> relationships
    {
        get { return _relationships; }
        set
        {
            _relationships = value;
            int i;
            for (i = 1; i <= 3; i++ )
            {
                string id = "" + i;
                if (relationships[id] > 100) { _relationships[id] = 100; }
                if (relationships[id] < 0 ) { _relationships[id] = 0; }
            }
        }
     }


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

    public void payRent(int rent, PlayerScript owner, int nth, Vector3 pos)
    {
        relationships[nth.ToString()] -= (float)rent/(float)money * vitutus*0.25f / (happiness*0.5f) * 100f;
        money -= rent; owner.money += rent;
        Instantiate(rentAnimation, pos, Quaternion.identity);
    }

}
