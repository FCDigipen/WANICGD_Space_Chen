using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Lose() {
        state = GameState.LOSING;
        LoseScreen.SetActive(true);
    }
}
