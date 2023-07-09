using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBirthday : MonoBehaviour, ChanceBase
{
    [SerializeField] private string _description;
    public string description {get {return _description;}}
    [SerializeField] private Texture2D _texture;
    public Texture2D texture {get {return _texture;}}
    private BoardManager bm;public void start() {bm = FindObjectOfType<BoardManager>();}
    public void Affect() {
        Debug.Log("Happy birthday!");
        for (int i = 0; i < 4; i++) {
            if (i != bm.activePlayer) {
                var pre = bm.playerValues[i].money;
                bm.playerValues[i].money -= 100;
                bm.playerValues[i].relationships[((bm.activePlayer-i+16)%4).ToString()] -= 10;
                var change = pre - bm.playerValues[i].money;
                bm.CurrentPlayer().money += change;
                bm.CurrentPlayer().happiness += 7;
            }
        }
    }
}
