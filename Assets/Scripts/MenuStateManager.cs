using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class MenuStateManager : MonoBehaviour
{
    public enum MenuState {
        MAIN_MENU,
        LEVEL_SELECT,
        SETTINGS
    }

    private MenuState menuState = MenuState.MAIN_MENU;
    public MenuState getMenuState {get => menuState;}

    [Header("References")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject LevelSelect;
    [SerializeField] private GameObject Settings;

    public void ToLevelSelect() {
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
}
