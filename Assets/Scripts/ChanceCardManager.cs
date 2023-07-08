using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceCardManager : MonoBehaviour
{
    private Stack<ChanceBase> deck = new Stack<ChanceBase>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var c in GetComponents<ChanceBase>()) {
            Debug.Log(c.description);
            deck.Push(c);
        }
        DrawCard();
        DrawCard();
    }

    public void DrawCard() {
        if (deck.Count > 0) {
            var drawn = deck.Pop();
            drawn.Affect();
        } else {
            Debug.Log("Tried to draw from empty deck.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
