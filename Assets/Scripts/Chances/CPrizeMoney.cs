using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPrizeMoney : MonoBehaviour, ChanceBase
{
    [SerializeField]
    private string _description;
    public string description {get {return _description;}}
    [SerializeField] private Texture2D _texture;
    public Texture2D texture {get {return _texture;}}
    private BoardManager bm;public void start() {bm = FindObjectOfType<BoardManager>();}
    public void Affect() {
        Debug.Log("Prize money!");
        bm.CurrentPlayer().money += 300;
        bm.CurrentPlayer().happiness += 15;
    }
}
