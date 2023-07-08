using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.

public class ChanceCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler // required interface when using the OnPointerEnter method.
{
    private static Vector3 hoverDistance = new Vector3(10,20f,0);
    private static float hoverRotation = 8f;
    private bool hovered = false;
    private bool selected = false;
    private float animationState = 0.0f;
    private static float animationSpeed = 15.0f;
    private Rigidbody2D rb;
    public ChanceBase card;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var actualCard = transform.GetChild(0);
        animationState += (hovered ? 1 : -1)*animationSpeed*Time.deltaTime;
        actualCard.localPosition = Vector3.Lerp(Vector3.zero,hoverDistance,animationState);
        actualCard.localEulerAngles = Vector3.Lerp(Vector3.zero,new Vector3(0,0,hoverRotation),animationState);
        actualCard.GetChild(0).gameObject.SetActive(!selected);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hovered = true;
        animationState = 0.0f;
    }
    public void OnPointerExit(PointerEventData eventData) {
        hovered = false;
        animationState = 1.0f;
    }
    public void OnPointerDown(PointerEventData eventData) {
        selected = !selected;
    }
    void FixedUpdate() {
        var target = new Vector2(Mathf.Clamp(transform.localPosition.x,-400,400),0);
        var difference = new Vector2(transform.localPosition.x,transform.localPosition.y) - target;
        rb.AddForce(-difference*10f,ForceMode2D.Force);
    }
}
