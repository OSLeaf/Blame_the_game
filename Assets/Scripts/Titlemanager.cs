using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titlemanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) {
                // Nothing
            } else {
                Application.Quit();
            }
        }
    }

    public void play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
