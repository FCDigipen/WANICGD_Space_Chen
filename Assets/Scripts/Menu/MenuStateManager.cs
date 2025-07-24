using System;
using System.Collections.Generic;
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
    /// Map from current GameState to currently active menu object.
    /// Initalized in Start()
    /// </summary>
    private Dictionary<MenuState, GameObject> stateToUIObject;
    
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
        stateToUIObject[MenuState.MAIN_MENU] = Canvas.transform.Find("Main Menu").gameObject;
        stateToUIObject[MenuState.LEVEL_SELECT] = Canvas.transform.Find("Level Select").gameObject;
        stateToUIObject[MenuState.SETTINGS] = Canvas.transform.Find("Settings").gameObject;
        stateToUIObject[MenuState.CREDITS] = Canvas.transform.Find("Credits").gameObject;

        // get reference to AudioSource attached to ButtonClickSFX
        ButtonClickSFX = ButtonClickSFXObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// plays the button click sfx
    /// </summary>
    public void PlaySFX() { ButtonClickSFX.Play(); }


    /// <summary>
    /// State interface for buttons.
    /// Switches from the current state to the target MenuState
    /// </summary>
    public void SwitchToMenu(MenuState target)
    {
        // disable old ui object
        stateToUIObject[state].SetActive(false);

        // switch state and enable new ui object
        state = target;
        stateToUIObject[state].SetActive(true);
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
