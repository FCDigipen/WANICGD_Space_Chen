using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuStateManager : MonoBehaviour
{
    public enum MenuState {
        MAIN_MENU,
        LEVEL_SELECT,
        SETTINGS
    }

    private MenuState menuState = MenuState.MAIN_MENU;
    public MenuState getMenuState {get => menuState;}
    private AudioSource ButtonClickSFX;

    [Header("References")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject LevelSelect;
    [SerializeField] private GameObject Settings;
    [SerializeField] private GameObject ButtonClickSFXObject;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string[] levels;
    void Start()
    {
        mixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 0f));
        mixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 0f));
        ButtonClickSFX = ButtonClickSFXObject.GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        ButtonClickSFX.Play();
    }

    public void ToLevelSelect()
    {
        LevelSelect.SetActive(true);
        MainMenu.SetActive(false);
        menuState = MenuState.LEVEL_SELECT;
    }

    public void ToSettings() {
        Settings.SetActive(true);
        MainMenu.SetActive(false);
        menuState = MenuState.SETTINGS;
    }

    public void ToMainMenu() {
        if(menuState == MenuState.LEVEL_SELECT) {LevelSelect.SetActive(false);}
        else if(menuState == MenuState.SETTINGS) {Settings.SetActive(false);}
        else { throw new NotImplementedException("Returning from non-impelmented scene");}
        MainMenu.SetActive(true);
        menuState = MenuState.MAIN_MENU;
    }

    public void Quit() {
        Application.Quit();
    }

    // debug button to reset all progress
    public void ResetProgress() {
        for(int i = 0; i < levels.Length; ++i) {
            PlayerPrefs.DeleteKey(levels[i] + ":BestTime");
            PlayerPrefs.DeleteKey(levels[i] + ":BestShots");
            PlayerPrefs.DeleteKey(levels[i]);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // restart scene
    }
}
