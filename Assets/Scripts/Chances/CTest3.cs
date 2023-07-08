using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTest3 : MonoBehaviour, ChanceBase
{
    [SerializeField]
    private string _description;
    public string description {get {return _description;}}
    [SerializeField] private Texture2D _texture;
    public Texture2D texture {get {return _texture;}}
    public void Affect() {
        Debug.Log("Hello4");
    }
}
