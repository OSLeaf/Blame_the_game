using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTest1 : MonoBehaviour, ChanceBase
{
    [SerializeField]
    private string _description;
    public string description {get {return _description;}}
    public void Affect() {
        Debug.Log("Hello2");
    }
}
