using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var square = transform.parent.transform.parent;
        square.GetComponent<SquareScript>().canvasBase.SetActive(false);
        Destroy(square.GetComponent<SquareScript>().panelBase);
    }
}
