using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    float tiemr = 0f;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tiemr += Time.deltaTime*4;
        transform.position = originalPos + new Vector3(Mathf.Sin(0.4534f*tiemr)*0.1f,Mathf.Sin(0.5376f*tiemr)*0.1f,Mathf.Sin(0.67234f*tiemr)*0.1f);
    }
}
