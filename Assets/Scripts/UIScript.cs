using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour, IPointerExitHandler
{
    bool firstTick = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && transform.parent.transform.parent.GetComponent<SquareScript>().canvasBase.activeInHierarchy)
        {
            if (firstTick) {
                firstTick = false;
                return;
            }
            SelfDestruct();
            return;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SelfDestruct();
    }

    void SelfDestruct()
    {
        firstTick = true;
        var square = transform.parent.transform.parent;
        square.GetComponent<SquareScript>().canvasBase.SetActive(false);
        Destroy(square.GetComponent<SquareScript>().panelBase);
        square.parent.GetComponent<SquareManagementScript>().squareUIisActive = false;
    }
}
