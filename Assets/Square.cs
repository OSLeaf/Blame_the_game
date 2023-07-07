using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public Square[] connections;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var neighbor in connections) {
            Debug.DrawLine(transform.position, neighbor.transform.position, Color.red, 10.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
