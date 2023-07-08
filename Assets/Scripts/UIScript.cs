using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour, IPointerExitHandler
{
    public bool firstTick = true;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckIfShouldSelfDestruct", 0, 0.1f);
    }

    // Update is called once per frame
    private void CheckIfShouldSelfDestruct()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && transform.parent.transform.parent.GetComponent<SquareScript>().canvasBase.activeInHierarchy)
        {
            if (firstTick) {
                firstTick = false;
                return;
            }
            SelfDestruct();
        }
    }
    void Update()
    {

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
