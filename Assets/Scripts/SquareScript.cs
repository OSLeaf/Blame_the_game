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
    public TMP_Text text;
    public GameObject canvasBase;
    Canvas canvas;
    public GameObject panelBase;
    SquareManagementScript squareManager;
    public void Start()
    {
        DestroyAllBridges();
        UpdatePaths();

        if (Application.IsPlaying(gameObject))
        {
            squareManager = transform.parent.GetComponent<SquareManagementScript>();
            m_renderer = GetComponent<MeshRenderer>();
            originalColor = m_renderer.material.color;
        // Canvas
            canvasBase = new GameObject("SquareCanvas");
            canvasBase.AddComponent<Canvas>();

            canvasBase.SetActive(false);

            canvasBase.transform.SetParent(transform, false);
            canvas = canvasBase.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasBase.AddComponent<CanvasScaler>();
            canvasBase.AddComponent<GraphicRaycaster>();
            CanvasScaler c = canvas.GetComponent<CanvasScaler>();
            c.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; 
        }
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

    private void OnMouseEnter()
    {   
        if (squareManager.squareUIisActive) {
            return;
        }
        canvasBase.SetActive(true);
        panelBase = new GameObject("Panel");
        panelBase.AddComponent<CanvasRenderer>();
        Image i = panelBase.AddComponent<Image>();
        i.color = Color.cyan;
        i.rectTransform.sizeDelta = new Vector2(80, 160);

        var pos =  Camera.main.WorldToScreenPoint(transform.position);
        panelBase.transform.SetParent(canvasBase.transform, true);
        panelBase.transform.position = new Vector3(pos.x, pos.y, pos.z);
        panelBase.AddComponent<UIScript>();

        GameObject buttonBase = new GameObject("Button");
        buttonBase.AddComponent<CanvasRenderer>();
        Image buttonImage = buttonBase.AddComponent<Image>();
        buttonImage.rectTransform.sizeDelta = new Vector2(70, 30);
        var buttPos = buttonImage.rectTransform.anchoredPosition3D;
        buttonImage.rectTransform.anchoredPosition = new Vector2(buttPos.x, buttPos.y + 60);
        Button button = buttonBase.AddComponent<Button>();
        button.onClick.AddListener(delegate { transform.GetComponent<LandOnTile>().ChangeTileBehavior("Happiness", 10); } );

        buttonBase.transform.SetParent(panelBase.transform, false); 

        GameObject textComponent = new GameObject("Text");
        text = textComponent.AddComponent<TextMeshProUGUI>();
        text.text = "Happiness";
        text.fontSize = 10;
        text.color = Color.black;
        text.transform.position = new Vector3(text.transform.position.x + 75, text.transform.position.y - 17, text.transform.position.z);

        textComponent.transform.SetParent(buttonBase.transform, false);
        squareManager.squareUIisActive = true;
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
        if (Application.IsPlaying(gameObject)) {
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
