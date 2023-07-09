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
using System.Drawing;
using System.Net.Security;
//using Image = UnityEngine.UI.Image;

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

        int playerId = -1;


        int j;
        for (j = 0; j < 4; j++)
        {
            if (manager.playerValues[j] == player)
            {
                playerId = j;
            }
        }


        gameObject.GetComponent<UnityEngine.UI.Image>().color = new UnityEngine.Color(manager.playerColors[playerId].r, manager.playerColors[playerId].g, manager.playerColors[playerId].b, 0.75f);
        //       gameObject.GetComponent<UnityEngine.UI.Image>().color = ;



        int i;
        for (i = 1; i <= player.relationships.Count; i++)
        {
            //Instantiate()

            GameObject tekstiObjekti = new GameObject();
            TextMeshProUGUI t = tekstiObjekti.AddComponent<TextMeshProUGUI>();
            t.SetText(manager.playerValues[(playerId + 1) % 4].name);
            t.fontSize = 12f;
            t.color = UnityEngine.Color.white;
            tekstiObjekti.transform.SetParent(gameObject.transform);
            //tekstiObjekti.transform.position = new Vector3(250, 100, 0);
            tekstiObjekti.name = "TemporaryText";
            //float right =  
            tekstiObjekti.transform.localPosition = new Vector3(60, -76 + 28 * i, 0);
            Debug.Log(" " + tekstiObjekti.transform.localPosition.x + " , " + tekstiObjekti.transform.localPosition.y);


            //GameObject kuvaObjekti = new GameObject();
            //UnityEngine.UI.Image img = kuvaObjekti.AddComponent<UnityEngine.UI.Image>();
            //img.sprite = Resources.Load<Sprite>("heart.png");
            //kuvaObjekti.name = "GeneratedHeart";
            //kuvaObjekti.transform.SetParent(gameObject.transform);
            //float varikerroin = player.relationships["" + i] / 100;
            //img.color = new UnityEngine.Color(varikerroin, varikerroin / 4, varikerroin / 4);
            //tekstiObjekti.transform.localPosition = new Vector3(65, -70 + 25 * i, 0);


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
            //Debug.Log(sydamet);
            float varikerroin = player.relationships[p] / 100;
            //img.color = new UnityEngine.Color(varikerroin, varikerroin / 4, varikerroin / 4)
            sydan.color = new UnityEngine.Color(varikerroin, varikerroin / 3, varikerroin / 3);
            //Debug.Log("color löydetty");
            //sydan.GetComponentInChildren<Text>().text = "" + player.relationships[p];


            GameObject tekstiObjekti = new GameObject();
            TextMeshProUGUI t = tekstiObjekti.AddComponent<TextMeshProUGUI>();
            t.SetText("" + player.relationships[p]);
            t.fontSize = 12f;
            t.color = UnityEngine.Color.white;
            tekstiObjekti.transform.SetParent(sydan.transform);
            //tekstiObjekti.transform.position = new Vector3(250, 100, 0);
            tekstiObjekti.name = "LovePros";
            //float right =  
            tekstiObjekti.transform.localPosition = new Vector3(43, -7, 0);
            // Debug.Log(" " + tekstiObjekti.transform.localPosition.x + " , " + tekstiObjekti.transform.localPosition.y);




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
        muokattava.text = muokattava.text + "    " + player.money;

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
        muokattava.text = "Money";
        gameObject.SetActive(false);

        foreach (TextMeshProUGUI t in texts.Where(a => a.name == "TemporaryText" || a.name == "LovePros"))
        {
            Destroy(t.gameObject);
        }
    }







}




