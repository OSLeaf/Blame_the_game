using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using TMPro;

public class pelaajaInfo : MonoBehaviour
{


    private BoardManager manager;

    // Start is called before the first frame update
    void Start()
    {
        //enabled = false;
        gameObject.SetActive(false);

        manager = gameObject.transform.parent.gameObject.GetComponentInChildren<BoardManager>();
        
        //line = GetComponent<LineRenderer>();
        //hearts = new Image[4];
        //playerValues = new PlayerScript[4];


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadPlayer(PlayerScript player)
    {
        gameObject.SetActive(true);

        int i;
        for (i = 1; i <= player.relationships.Count; i++)
        {
            //Instantiate()
            
            GameObject tekstiObjekti = new GameObject();
            TextMeshProUGUI t = tekstiObjekti.AddComponent<TextMeshProUGUI>();
            t.SetText(manager.nthPlayer(i).name);
            t.fontSize = 12f;
            t.color = Color.white;
            tekstiObjekti.transform.parent = gameObject.transform;
            t.transform.position = new Vector3(500, 200, 0);
           
        }






        //int wtf = 100;
        //if (gameObject.GetType == typeOf(UnityEngine.UI.Image))
        //{
        GameObject[] tama = { gameObject };
        //}
        var sydamet = new HashSet<UnityEngine.UI.Image>(GetComponentsInChildren<UnityEngine.UI.Image>(true));
        sydamet.Remove(gameObject.GetComponent<UnityEngine.UI.Image>());

        //for (i = 0; i < sydamet.Length; i++)
        //{
        //    if (sydamet[i].GetComponent<pelaajaInfo>() != null)// == gameObject)
        //    {
        //        sydamet = sydamet.Take(i - 1).Concat(sydamet.Skip(i)).ToArrayPooled;
        //    }
        //}
        //foreach (UnityEngine.UI.Image im in gameObject)


        //Debug.Log(sydamet);
        i = 0;
        foreach (string p in player.relationships.Keys)
        {
            UnityEngine.UI.Image sydan = sydamet.ElementAt(i);
            Debug.Log(sydamet);
            sydan.color = new Color(player.relationships[p] / 100, 0, 0);
            Debug.Log("color l�ydetty");
            //sydan.GetComponentInChildren<Text>().text = "" + player.relationships[p];
            i += 1;
        }


        var texts = new HashSet<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>(true));
        TextMeshProUGUI muokattava = texts.First(t => t.name == "Happiness");
        muokattava.text = muokattava.text + "    " + player.happiness;
        muokattava = texts.First(t => t.name == "Vitutus");
        muokattava.text = muokattava.text + "    " + player.vitutus;
        muokattava = texts.First(t => t.name == "Luck");
        muokattava.text = muokattava.text + "    " + player.luck;

        muokattava = texts.First(t => t.name == "Name");
        muokattava.text = player.name;
        muokattava = texts.First(t => t.name == "Money");
        //muokattava.text = player.name;

        //Debug.Log(texts);




    }

    public void Close() 
    {
        
        var texts = new HashSet<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>(true));
        TextMeshProUGUI muokattava = texts.First(t => t.name == "Happiness");
        muokattava.text = "Happiness";
        muokattava = texts.First(t => t.name == "Vitutus");
        muokattava.text = "Vitutus";
        muokattava = texts.First(t => t.name == "Luck");
        muokattava.text = "Luck";

        muokattava = texts.First(t => t.name == "Name");
        muokattava.text = "";
        muokattava = texts.First(t => t.name == "Money");
        gameObject.SetActive(false);

        foreach (TextMeshProUGUI t in texts.Where(a => a.name == "New Game Object"))
        {
           Destroy(t.gameObject);
        }
    }







}




