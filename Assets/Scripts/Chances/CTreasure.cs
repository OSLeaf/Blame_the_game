using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTreasure : MonoBehaviour, ChanceBase
{
    [SerializeField]
    private string _description;
    public string description {get {return _description;}}
    [SerializeField] private Texture2D _texture;
    public Texture2D texture {get {return _texture;}}
    private BoardManager bm;public void Start() {bm = FindObjectOfType<BoardManager>();}
    public void Affect() {
        Debug.Log("Treasure!");
        bm.CurrentPlayer().money += 250;
        bm.CurrentPlayer().happiness += 10;
    }
}
