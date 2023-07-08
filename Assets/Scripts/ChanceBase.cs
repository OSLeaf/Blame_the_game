using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ChanceBase
{
    public string description {get;}
    public virtual void Affect() {
        Debug.Log("BASE");
    }
}
