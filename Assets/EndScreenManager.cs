using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject hoverdisabler;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject lossScreen;
    
    public void LoseGame() {
        hoverdisabler.SetActive(true);
        lossScreen.SetActive(true);
    }
    public void WinGame() {
        hoverdisabler.SetActive(true);
        winScreen.SetActive(true);
    }
}
