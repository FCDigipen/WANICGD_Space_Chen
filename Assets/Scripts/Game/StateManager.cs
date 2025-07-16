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
    public enum GameState
    {
        WINNING,
        LOSING,
        PLAYING,
        PAUSED,
    }

    private GameState state = GameState.PLAYING;
    public GameState getState { get => state; }

    [Header("References")]
    [SerializeField] public string levelID;
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject shotCounter;
    [SerializeField] private GameObject loseSFXObject; // prefab
    [SerializeField] private float winTime;

    private TextMeshProUGUI time;
    private TextMeshProUGUI bestTime;
    private TextMeshProUGUI shots;
    private TextMeshProUGUI bestShots;
    private AudioSource gpMusic;

    private void Start()
    {
        Time.timeScale = 1;
        gpMusic = GetComponent<AudioSource>(); // attached to this object

        // manually iterating through children :)
        time = WinScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        bestTime = WinScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        shots = WinScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        bestShots = WinScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    public void Lose()
    {
        state = GameState.LOSING;
        gpMusic.Pause();
        Destroy(Instantiate(loseSFXObject),2f); // instantiate death sfx object and delte it after two seconds
        Time.timeScale = 0; // pause the game
        LoseScreen.SetActive(true);
    }

    public IEnumerator Win()
    {
        gpMusic.Pause();
        Time.timeScale = 0.2f; // how slow the slow down display is
        yield return new WaitForSeconds(winTime);
        Time.timeScale = 0;
        float t = Time.timeSinceLevelLoad; // less accurate than display but shh :)
        int s = shotCounter.GetComponent<ShotCounter>().getShots();
        state = GameState.WINNING;
        WinScreen.SetActive(true);

        TimeSpan timeSpan = TimeSpan.FromSeconds(t);
        time.text = $"TIME={timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
        shots.text = $"SHOTS={s}";

        // check if bests should be overwritten
        if (PlayerPrefs.GetFloat(levelID + ":BestTime", float.MaxValue) > t) { PlayerPrefs.SetFloat(levelID + ":BestTime", t); }
        if (PlayerPrefs.GetInt(levelID + ":BestShots", Int32.MaxValue) > s) { PlayerPrefs.SetInt(levelID + ":BestShots", s); }
        if (PlayerPrefs.GetInt(levelID, 0) == 0) { PlayerPrefs.SetInt(levelID, 1); } // TODO: replace with more secure storage method

        // load personal bests
        TimeSpan timeSpan2 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(levelID + ":BestTime")); // overwite for copy paste :)
        bestTime.text = $"BEST={timeSpan2.Minutes:00}:{timeSpan2.Seconds:00}:{timeSpan2.Milliseconds:000}";
        bestShots.text = $"LOW={PlayerPrefs.GetInt(levelID + ":BestShots")}";
    }

    public void TogglePause(InputAction.CallbackContext ctx) { if (state == GameState.PLAYING) { Pause(); } else if (state == GameState.PAUSED) { Play(); } }
    public void Pause()
    {
        if (state == GameState.PLAYING)
        {
            gpMusic.volume = 0.15f; // manually reduce volume. TODO: make seperate script to handle volume with changable variables
            gpMusic.pitch = 0.5f; // see above
            Time.timeScale = 0;
            state = GameState.PAUSED;
            PauseScreen.SetActive(true);
        }
    }
    public void Play()
    {
        if (state == GameState.PAUSED)
        {
            gpMusic.volume = 0.2f; // manually reduce volume. TODO: make seperate script to handle volume with changable variables
            gpMusic.pitch = 1f;
            Time.timeScale = 1;
            state = GameState.PLAYING;
            PauseScreen.SetActive(false);
        }
    }

    public void RestartKey(InputAction.CallbackContext ctx) { Restart(); }
    public void Restart() {
        // reset this scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
