using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquareScript : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> scriptsToRunWhenLanded;
    [SerializeField] public List<SquareScript> connections; // The squares that can be continued to
    // Start is called before the first frame update
    MeshRenderer m_renderer;

    Color overColor = Color.yellow;

    Color originalColor;
    bool showgui = false;

    public TMP_Text text;
    GameObject canvasBase;
    Canvas canvas;
    GameObject panelBase;

    public void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
        originalColor = m_renderer.material.color;


        // Canvas
        canvasBase = new GameObject("SquareCanvas");
        canvasBase.AddComponent<Canvas>();

        canvasBase.transform.SetParent(transform, false);
        canvas = canvasBase.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasBase.AddComponent<CanvasScaler>();
        canvasBase.AddComponent<GraphicRaycaster>();
        CanvasScaler c = canvas.GetComponent<CanvasScaler>();
        c.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        panelBase = new GameObject("Panel");
        panelBase.AddComponent<CanvasRenderer>();
        Image i = panelBase.AddComponent<Image>();
        i.color = Color.cyan;

        var pos =  Camera.main.WorldToScreenPoint(transform.position);
        panelBase.transform.SetParent(canvasBase.transform, true);
        panelBase.transform.position = new Vector3(pos.x, pos.y, pos.z);

    }
    public void Landed()
    {
        if (scriptsToRunWhenLanded.Count == 0)
        {
            FindObjectOfType<BoardManager>().NextTurn();
            return;
        }
        foreach (MonoBehaviour script in scriptsToRunWhenLanded)
        {
            script.Invoke("Play", 0);
        }
    }

    private void OnMouseDown()
    {
        showgui = true;
    }

    private void OnMouseOver()
    {
        m_renderer.material.color = overColor;
    }

    private void OnMouseExit()
    {
        m_renderer.material.color = originalColor;
    }
}
