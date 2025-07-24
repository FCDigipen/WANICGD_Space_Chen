using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/*
    Class: MenuStateManager
    Purpose: Manage the current menu (state) the player is in within the menu
*/

public class MenuStateManager : MonoBehaviour
{
    /// <summary>
    /// Types of menu the player can be in
    /// </summary>
    public enum MenuState
    {
        MAIN_MENU,
        LEVEL_SELECT,
        SETTINGS,
        CREDITS,
    }

    /// <summary>
    /// The current menu the player is in
    /// </summary>
    private MenuState state = MenuState.MAIN_MENU;

    /// <summary>
    /// Getter for the current menu the player is in
    /// </summary>
    public MenuState getMenuState { get => state; }

    // self explanitory
    private AudioSource ButtonClickSFX;

    [SerializeField] private GameObject Canvas; // reference to canvas for getting GUI objects

    /// <summary>
    /// References to GUI objects initalized in Start()
    /// </summary>
    private GameObject MainMenu;
    private GameObject LevelSelect;
    private GameObject Settings;
    private GameObject Credits;
    
    /// <summary>
    /// Object with ButtonClickSFX. TODO: replace with prefab with play on awake
    /// </summary>
    [SerializeField] private GameObject ButtonClickSFXObject;

    /// <summary>
    /// Main audio controller for the game.
    /// </summary>
    [SerializeField] private AudioMixer mixer;

    /// <summary>
    /// Array of levelIDs. Used to manually reset level completions in the ResetProgress() function.
    /// </summary>
    [SerializeField] private string[] levels;
    void Start()
    {
        // set volume settings to player settings, or default max volume
        mixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 0f));
        mixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 0f));

        // get references to GUI objects
        MainMenu = Canvas.transform.Find("Main Menu").gameObject;
        LevelSelect = Canvas.transform.Find("Level Select").gameObject;
        Settings = Canvas.transform.Find("Settings").gameObject;
        Credits = Canvas.transform.Find("Credits").gameObject;

        // get reference to AudioSource attached to ButtonClickSFX
        ButtonClickSFX = ButtonClickSFXObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// plays the button click sfx
    /// </summary>
    public void PlaySFX() { ButtonClickSFX.Play(); }


    /// <summary>
    /// Switches from the main menu to the level select screen
    /// </summary>
    public void ToLevelSelect()
    {
        // toggle corresponding guis
        LevelSelect.SetActive(true);
        MainMenu.SetActive(false);

        // switch menu state
        state = MenuState.LEVEL_SELECT;
    }

    /// <summary>
    /// Switches from either the main menu or the credits screen to the settings screen
    /// Credit screen too because i was lazy
    /// </summary>
    public void ToSettings()
    {
        // toggle corresponding guis
        Settings.SetActive(true);
        Credits.SetActive(false);
        MainMenu.SetActive(false);
        state = MenuState.SETTINGS;
    }


    /// <summary>
    /// Switches from the settings menu to the credits menu
    /// </summary>
    public void ToCredits()
    {
        Settings.SetActive(false);
        Credits.SetActive(true);
        state = MenuState.CREDITS;
    }

    /// <summary>
    /// Switches from either the Level Select or the Settings menu to the main menu
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void ToMainMenu()
    {
        if (state == MenuState.LEVEL_SELECT) { LevelSelect.SetActive(false); }
        else if (state == MenuState.SETTINGS) { Settings.SetActive(false); }
        else { throw new NotImplementedException("Returning from non-impelmented scene"); }
        MainMenu.SetActive(true);
        state = MenuState.MAIN_MENU;
    }

    /// <summary>
    /// Quits application, only for app build
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Resets all level statistics
    /// </summary>
    public void ResetProgress()
    {
        for (int i = 0; i < levels.Length; ++i)
        {
            PlayerPrefs.DeleteKey(levels[i] + ":BestTime");
            PlayerPrefs.DeleteKey(levels[i] + ":BestShots");
            PlayerPrefs.DeleteKey(levels[i]);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // restart scene
    }
}
