using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class SquareScript : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> scriptsToRunWhenLanded;
    [SerializeField] public List<SquareScript> connections; // The squares that can be continued to
    [SerializeField] private BridgeScript bridgeAsset;
    private Dictionary<SquareScript,BridgeScript> bridges = new Dictionary<SquareScript, BridgeScript>();
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
        DestroyAllBridges();
        UpdatePaths();
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

    private void DestroyBridge(SquareScript target) {
        if (bridges.ContainsKey(target)) {
            if(Application.IsPlaying(gameObject)) {
                Destroy(bridges[target].gameObject);
            } else {
                DestroyImmediate(bridges[target].gameObject);
            }
            bridges.Remove(target);
        }
    }
    private void DestroyAllBridges() {
        foreach (var conbridge in bridges) {
            DestroyBridge(conbridge.Key);
        }
        if(Application.IsPlaying(gameObject)) {
            foreach (var bridge in GetComponentsInChildren<BridgeScript>()) {
                Destroy(bridge.gameObject);
            }
        } else {
            foreach (var bridge in GetComponentsInChildren<BridgeScript>()) {
                DestroyImmediate(bridge.gameObject);
            }
        }
    }

    public void UpdatePaths() {
        foreach (var connection in connections) {
            // Debug.Log("Update paths");
            Vector3 difference = connection.transform.position - transform.position;
            BridgeScript bridge;
            if (bridges.ContainsKey(connection)) {
                bridge = bridges[connection];
            } else {
                bridge = Instantiate(bridgeAsset, transform.position, Quaternion.identity);
                bridges.Add(connection, bridge);
            }
            bridge.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(difference, Vector3.up));
            var globalscalefactor = bridge.transform.lossyScale.z/bridge.transform.localScale.z;
            bridge.transform.localScale = new Vector3(1,1,difference.magnitude/globalscalefactor);
            bridge.transform.parent = transform;
        }
        var allbridges = new List<SquareScript>(bridges.Keys);
        foreach (var target in allbridges) {
            if (!connections.Contains(target)) {
                DestroyBridge(target);
            }
        }
    }

    public void Update() {
        if(!Application.IsPlaying(gameObject)) {
            UpdatePaths();
        }
    }
}
