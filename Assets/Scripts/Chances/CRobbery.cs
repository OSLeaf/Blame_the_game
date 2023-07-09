using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRobbery : MonoBehaviour, ChanceBase
{
    [SerializeField]
    private string _description;
    public string description {get {return _description;}}
    [SerializeField] private Texture2D _texture;
    public Texture2D texture {get {return _texture;}}
    private BoardManager bm;void start() {bm = FindObjectOfType<BoardManager>();}
    public void Affect() {
        Debug.Log("Robbing");
        for (int i = 0; i < 4; i++) {
            bm.playerValues[i].money -= 500;
            bm.playerValues[i].happiness -= 20;
            if (i != bm.activePlayer) {
                bm.playerValues[i].relationships[((bm.activePlayer-i+16)%4).ToString()] -= 30;
            }
        }
    }
}
