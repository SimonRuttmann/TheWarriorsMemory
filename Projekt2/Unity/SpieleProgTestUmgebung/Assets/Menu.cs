using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private GameObject _mainmenu;

    public void Start()
    {
        _mainmenu = GameObject.FindGameObjectWithTag("Menu");
    }

    public void NewGame()
    {
        _mainmenu.SetActive(false);
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
    }

    public void SaveGame()
    {
        Debug.Log("SaveGame");
    }

    public void ContinueGame()
    {
        Debug.Log("Continue Game");
    }
}
