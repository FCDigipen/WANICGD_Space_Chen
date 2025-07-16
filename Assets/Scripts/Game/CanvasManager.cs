using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{ 
    // class to handle all canvas interactions
     private GameObject LoseScreen;
     private GameObject WinScreen;
     private GameObject PauseScreen;
     private GameObject ShotCounter;

    private TextMeshProUGUI time;
    private TextMeshProUGUI bestTime;
    private TextMeshProUGUI shots;
    private TextMeshProUGUI bestShots;

    // Start is called before the first frame update
    void Start()
    {
        LoseScreen = transform.Find("Lose Screen").gameObject;
        WinScreen = transform.Find("Win Screen").gameObject;
        PauseScreen = transform.Find("Pause Screen").gameObject;
        ShotCounter = transform.Find("Counters/Shot Counter").gameObject;

        // manually iterating through children :)
        time = WinScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        bestTime = WinScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        shots = WinScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        bestShots = WinScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    public int GetShots() {return ShotCounter.GetComponent<ShotCounter>().getShots();}
    public void SetLoseScreen(bool active) {LoseScreen.SetActive(active);}
    public void SetWinScreen(bool active) {WinScreen.SetActive(active);}
    public void SetPauseScreen(bool active) {PauseScreen.SetActive(active);}
    public void SetTimeText(string t) {time.text = t;}
    public void SetBestTimeText(string t) {bestTime.text = t;}
    public void SetShotsText(string t) {shots.text = t;}
    public void SetBestShotsText(string t) {bestShots.text = t;}

}
