using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanceCardManager : MonoBehaviour
{
    private Stack<ChanceBase> deck = new Stack<ChanceBase>();
    [SerializeField] private ChanceCard chanceCardUIAsset;
    [SerializeField] private ChanceCard chanceCard3DAsset;
    [SerializeField] private GameObject deckBase;
    private bool isPicking = false;
    private GameObject confirmButton;
    // Start is called before the first frame update
    void Start()
    {
        confirmButton = transform.GetChild(0).gameObject;
        confirmButton.SetActive(isPicking);
        foreach (var c in GetComponents<ChanceBase>()) {
            deck.Push(c);
        }
    }

    public void DrawCard() {
        if (isPicking) {
            Debug.Log("Shouldn't happen!");
            return;
        }
        if (deck.Count > 0) {
            var drawn = deck.Pop();
            drawn.Affect();
        } else {
            Debug.Log("Tried to draw from empty deck.");
        }
        if (deck.Count == 0) {
            TopUpDeck();
        }
    }

    public void TopUpDeck() {
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
        isPicking = false;
        confirmButton.SetActive(isPicking);
        // foreach () {

        // }
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
}
