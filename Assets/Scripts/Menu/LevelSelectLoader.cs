using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectLoader : MonoBehaviour
{
    // script to load access denied, incomplete, or pbs
    public enum State {
        UNDETERMINED,
        AccessDenied,
        Incomplete,
        Complete
    }

    [SerializeField] private string levelID;
    [Header("References")]
    [SerializeField] private GameObject AccessDenied;
    [SerializeField] private GameObject Incomplete;
    [SerializeField] private GameObject BestTime;
    [SerializeField] private GameObject BestShots;
    [Header("Dependencies")]
    [SerializeField] private string[] dependencies;
    private State state = State.UNDETERMINED;

    void Start()
    {
        for(int i = 0; i < dependencies.Length; ++i) {
            if(PlayerPrefs.GetInt(dependencies[i], 0) == 0) {
                state = State.AccessDenied;
                break;
            }
        }
        if(state == State.UNDETERMINED) { // not done yet
            if(PlayerPrefs.GetInt(levelID, 0) == 0) {
                state = State.Incomplete;
            } else {
                state = State.Complete;
            }
        }

        switch(state) {
            case State.AccessDenied:
                AccessDenied.SetActive(true);
                break;
            case State.Incomplete:
                Incomplete.SetActive(true);
                break;
            case State.Complete:
                TimeSpan timeSpan = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(levelID + ":BestTime")); // overwite for copy paste :)
                BestTime.GetComponent<TextMeshProUGUI>().text = $"TIME={timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
                BestShots.GetComponent<TextMeshProUGUI>().text = $"SHOTS={PlayerPrefs.GetInt(levelID + ":BestShots")}";
                BestTime.SetActive(true);
                BestShots.SetActive(true);
                break;
            default:
                throw new NotImplementedException("Undetermined Level State for level " + levelID);
        }
    }

    public void LoadLevel() {
        if(state != State.AccessDenied) {
            SceneManager.LoadScene(levelID);
        }
    }
}
