using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceCard3D : MonoBehaviour
{
    public void SetTexture(Texture2D texture) {
        transform.GetChild(1).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
    }
}
