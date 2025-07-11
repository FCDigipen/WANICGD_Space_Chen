using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    // manages state of the game: winning, losing, playing, paused
    public enum GameState {
        WINNING,
        LOSING,
        PLAYING,
        PAUSED,
    }

    private GameState state = GameState.PLAYING;
    public GameState getState {get => state;}

    [Header("References")]
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject shotCounter;

    private TextMeshProUGUI time;
    private TextMeshProUGUI bestTime;
    private TextMeshProUGUI shots;
    private TextMeshProUGUI bestShots;

    private void Start()
    {
        Time.timeScale = 1;

        // manually iterating through children :)
        time = WinScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        bestTime = WinScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        shots = WinScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        bestShots = WinScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    public void Lose() {
        state = GameState.LOSING;
        Time.timeScale = 0; // pause the game
        LoseScreen.SetActive(true);
    }

    public void Win() {
        Time.timeScale = 0;
        float t = Time.timeSinceLevelLoad; // less accurate than display but shh :)
        int s = shotCounter.GetComponent<ShotCounter>().getShots();
        state = GameState.WINNING;
        WinScreen.SetActive(true);

        TimeSpan timeSpan = TimeSpan.FromSeconds(t);
        time.text = $"TIME={timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
        shots.text = $"SHOTS={s}";

        // check if bests should be overwritten
        if(PlayerPrefs.GetFloat("BestTime", float.MaxValue) > t) {PlayerPrefs.SetFloat("BestTime", t);}
        if(PlayerPrefs.GetInt("BestShots", Int32.MaxValue) > s) {PlayerPrefs.SetInt("BestShots", s);}

        // load personal bests
        TimeSpan timeSpan2 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("BestTime")); // overwite for copy paste :)
        bestTime.text = $"BEST={timeSpan2.Minutes:00}:{timeSpan2.Seconds:00}:{timeSpan2.Milliseconds:000}";
        bestShots.text = $"LOW={PlayerPrefs.GetInt("BestShots")}";
    }

    public void RestartKey(InputAction.CallbackContext ctx) {Restart();} // TODO: do people want to restart mid run?
    public void Restart() {
        // reset this scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
