using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private enum MenuState
    {
        Closed,
        Main,
        Ingame
    }
    private GameObject _menu;
    private GameObject _mainmenu;
    private GameObject _ingamemenu;
    
    public void Start()
    {
        _menu = GameObject.FindGameObjectWithTag("Menu");
        
        Transform main = _menu.transform.Find("MainMenu");
        Transform ingame = _menu.transform.Find("IngameMenu");
        
        if(main == null || ingame == null) Debug.LogError("Submenus could not be fetched");

        _mainmenu = main.gameObject;
        _ingamemenu = ingame.gameObject;
        
        
    }

    public void NewGame()
    {
        ChangeToMenuType(MenuState.Closed);
        //todo depending on the backend implementation this might need more code
    }

    public void LoadGame()
    {
        Debug.Log("Load Game");
        //todo load selected game state
    }

    public void CloseGame()
    {
        Debug.Log("Close Game");
        Application.Quit();
    }

    public void BackToTitlescreen()
    {
        Debug.Log("Back to Titlescreen");
        ChangeToMenuType(MenuState.Main);
    }

    public void SaveGame()
    {
        Debug.Log("SaveGame");
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game");
        _menu.SetActive(false);
    }

    public void OpenIngameMenu()
    {
        ChangeToMenuType(MenuState.Ingame);
    }

    private void ChangeToMenuType(MenuState state)
    {
        switch (state)
        {
            case MenuState.Main:
                _ingamemenu.SetActive(false);
                _mainmenu.SetActive(true);
                break;
            case MenuState.Ingame:
                _mainmenu.SetActive(false);
                _ingamemenu.SetActive(true);
                break;
            case MenuState.Closed:
                _menu.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
