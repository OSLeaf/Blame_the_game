using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[ExecuteAlways]
public class SquareScript : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> scriptsToRunWhenLanded;
    [SerializeField] public List<SquareScript> connections; // The squares that can be continued to
    [SerializeField] private BridgeScript bridgeAsset;
    private Dictionary<SquareScript,BridgeScript> bridges = new Dictionary<SquareScript, BridgeScript>();
    // Start is called before the first frame update
    Color overColor = Color.yellow;
    Color originalColor;
    public TMP_Text text;
    public GameObject canvasBase;
    Canvas canvas;
    public GameObject panelBase;
    SquareManagementScript squareManager;
    public bool startSelfDestruct = true;
    public GameObject textInputField;
    public GameObject addButton;
    public void Start()
    {
        DestroyAllBridges();
        UpdateBridges();

        if (Application.IsPlaying(gameObject))
        {
            squareManager = transform.parent.GetComponent<SquareManagementScript>();
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

    private void OnMouseOver()
    {  
        if (squareManager.squareUIisActive || !squareManager.allowSquareUIActivation) {
            return;
        }
        canvasBase.SetActive(true);
        panelBase = new GameObject("Panel");
        panelBase.AddComponent<CanvasRenderer>();
        Image i = panelBase.AddComponent<Image>();
        i.color = new Color32(204,199,193,255);
        i.rectTransform.sizeDelta = new Vector2(80, 160);

        var pos =  Camera.main.WorldToScreenPoint(transform.position);
        panelBase.transform.SetParent(canvasBase.transform, true);
        panelBase.transform.position = new Vector3(pos.x, pos.y, pos.z);
        panelBase.AddComponent<UIScript>();

        createButton("Happiness", 60);
        createButton("Luck", 25);

        squareManager.squareUIisActive = true;
    }

    private void createButton(string behavior, int yoffset)
    {
        GameObject buttonBase = new GameObject("Button");
        buttonBase.AddComponent<CanvasRenderer>();
        Image buttonImage = buttonBase.AddComponent<Image>();
        buttonImage.rectTransform.sizeDelta = new Vector2(70, 30);
        var buttPos = buttonImage.rectTransform.anchoredPosition3D;
        buttonImage.rectTransform.anchoredPosition = new Vector2(buttPos.x, buttPos.y + yoffset);
        Button button = buttonBase.AddComponent<Button>();
        button.onClick.AddListener(delegate {ShowPopUp(behavior);} );

        buttonBase.transform.SetParent(panelBase.transform, false); 

        GameObject textComponent = new GameObject("Text");
        text = textComponent.AddComponent<TextMeshProUGUI>();
        text.text = behavior;
        text.fontSize = 10;
        text.color = Color.black;
        text.transform.position = new Vector3(text.transform.position.x + 75, text.transform.position.y - 17, text.transform.position.z);

        textComponent.transform.SetParent(buttonBase.transform, false);
    }

    private void ShowPopUp(string behavior)
    {
        startSelfDestruct = false;
        panelBase.GetComponent<UIScript>().SelfDestruct();

        canvasBase.SetActive(true);
        panelBase = new GameObject("Panel");
        panelBase.AddComponent<CanvasRenderer>();
        Image i = panelBase.AddComponent<Image>();
        i.color = new Color32(204,199,193,255);
        i.rectTransform.sizeDelta = new Vector2(300, 100);

        panelBase.transform.SetParent(canvasBase.transform, false);
        panelBase.AddComponent<UIScript>();
        

        GameObject textComponent = new GameObject("Text");
        text = textComponent.AddComponent<TextMeshProUGUI>();
        text.text = behavior;
        text.fontSize = 20;
        text.transform.position = new Vector3(text.transform.position.x - 20, text.transform.position.y + 20, text.transform.position.z);
        text.color = Color.black;

        textComponent.transform.SetParent(panelBase.transform, false);

        Vector3 pos = panelBase.transform.position;
        GameObject textInput = Instantiate(textInputField, panelBase.transform.position, Quaternion.identity);
        textInput.transform.SetParent(panelBase.transform, true);

        GameObject changeButton = Instantiate(addButton, new Vector3(pos.x + 80,pos.y - 40, pos.y), Quaternion.identity);
        GameObject cancelButton = Instantiate(addButton, new Vector3(pos.x - 80,pos.y - 40, pos.y), Quaternion.identity);
        changeButton.GetComponent<Button>().onClick.AddListener(delegate {SubmitChangeEvent(behavior);});
        changeButton.transform.SetParent(panelBase.transform, true);
        cancelButton.GetComponent<Button>().onClick.AddListener(DestroyPopUp);
        cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";
        cancelButton.transform.SetParent(panelBase.transform, true);

        squareManager.allowSquareUIActivation = false;
    }

    void SubmitChangeEvent(string behavior)
    {
        int change = Int32.Parse(panelBase.GetComponentInChildren<TMP_InputField>().text);
        transform.GetComponent<LandOnTile>().ChangeTileBehavior(behavior, change);
        DestroyPopUp();
    }

    void DestroyPopUp()
    {
        panelBase.GetComponent<UIScript>().SelfDestruct();
        squareManager.allowSquareUIActivation = true;
        startSelfDestruct = true;
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

    public void UpdateBridges() {
        foreach (var connection in connections) {
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
            UpdateBridges();
        }
    }
}
