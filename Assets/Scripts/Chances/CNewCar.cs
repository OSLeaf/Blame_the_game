using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNewCar : MonoBehaviour, ChanceBase
{
    [SerializeField]
    private string _description;
    public string description {get {return _description;}}
    [SerializeField] private Texture2D _texture;
    public Texture2D texture {get {return _texture;}}
    private BoardManager bm;public void Start() {bm = FindObjectOfType<BoardManager>();}
    public void Affect() {
        Debug.Log("New car!");
        bm.CurrentPlayer().money -= 300;
        bm.CurrentPlayer().happiness -= 20;
    }
}
