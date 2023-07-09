using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChanceCardManager : MonoBehaviour
{
    private Stack<ChanceBase> deck = new Stack<ChanceBase>();
    [SerializeField] private ChanceCard chanceCardUIAsset;
    [SerializeField] private ChanceCard3D chanceCard3DAsset;
    [SerializeField] private GameObject deckBase;
    private bool isPicking = false;
    private GameObject confirmButton;
    private const float cardDrawDuration = 0.5f;
    private const float cardShowDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        confirmButton = transform.GetChild(0).gameObject;
        confirmButton.SetActive(isPicking);
        foreach (var c in GetComponents<ChanceBase>()) {
            deck.Push(c);
        }
        UpdateDeckVisual();
    }

    public void DrawCard() {
        if (isPicking) {
            Debug.Log("Shouldn't happen!");
            return;
        }
        if (deck.Count > 0) {
            var drawn = deck.Pop();
            drawn.Affect();
            UpdateDeckVisual();
            StartCoroutine(DrawCoroutine(drawn));
        } else {
            Debug.Log("Tried to draw from empty deck.");
            TopUpDeck();
        }
    }

    private void UpdateDeckVisual() {
        if (deck.Count > 0) {
            deckBase.GetComponentInChildren<TextMeshPro>().text = deck.Count.ToString();
            deckBase.transform.GetChild(0).gameObject.SetActive(true);
        } else  {
            deckBase.GetComponentInChildren<TextMeshPro>().text = "";
            deckBase.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void TopUpDeck() {
        if (isPicking) {return;}
        isPicking = true;
        confirmButton.SetActive(isPicking);
        foreach (var c in GetComponents<ChanceBase>()) {
            var pos = new Vector3(Random.Range(-400,400),Random.Range(-300,300),0);
            var card = Instantiate(chanceCardUIAsset, pos, Quaternion.identity);
            card.card = c;
            card.SetPicture(c.texture);
            card.transform.SetParent(transform, false);
        }
    }

    public void ConfirmDeck() {
        var selection = new List<ChanceCard>();
        foreach (var c in GetComponentsInChildren<ChanceCard>()) {
            if (c.selected) {
                selection.Add(c);
            }
        }
        var selectedCount = selection.Count;
        if (selectedCount == 0) {
            Debug.Log("No cards picked!");
            return;
        }
        for (int i = 0; i < selectedCount; i++) {
            int randomIndex = Random.Range(0,selectedCount-i-1);
            var toput = selection[randomIndex];
            selection.RemoveAt(randomIndex);
            deck.Push(toput.card);
        }
        foreach (var c in GetComponentsInChildren<ChanceCard>()) {
            Destroy(c.gameObject);
        }
        UpdateDeckVisual();
        isPicking = false;
        confirmButton.SetActive(isPicking);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            DrawCard();
        }
    }

    IEnumerator DrawCoroutine(ChanceBase cardtype) {
        Quaternion sourceRotation = Quaternion.Euler(0,0,180);
        Vector3 source = deckBase.transform.position;
        ChanceCard3D card = Instantiate(chanceCard3DAsset, source, sourceRotation);
        Vector3 target = Camera.main.transform.position + Camera.main.transform.forward*6f;
        Quaternion targetRotation = Camera.main.transform.rotation * Quaternion.Euler(-90,180,180);
        card.SetTexture(cardtype.texture);
        float timer = 0;
        while (timer < 1 + cardShowDuration/cardDrawDuration) {
            timer += Time.deltaTime / cardDrawDuration;
            card.transform.position = Vector3.Lerp(source, target, timer);
            card.transform.localRotation = Quaternion.Slerp(sourceRotation, targetRotation, timer);
            yield return null;
        }
        Destroy(card.gameObject);
        if (deck.Count == 0) {
            TopUpDeck();
        }
    }
}
