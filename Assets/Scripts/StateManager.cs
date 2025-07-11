using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        Time.timeScale = 1;   
    }

    public void Lose() {
        state = GameState.LOSING;
        Time.timeScale = 0; // pause the game
        LoseScreen.SetActive(true);
    }

    public void RestartKey(InputAction.CallbackContext ctx) {if(state == GameState.LOSING) Restart();} // TODO: do people want to restart mid run?
    public void Restart() {
        // reset this scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
