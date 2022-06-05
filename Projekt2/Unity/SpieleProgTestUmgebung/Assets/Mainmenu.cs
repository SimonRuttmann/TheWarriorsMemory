using System;
using UnityEngine;

public class Mainmenu : MonoBehaviour
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

    public void ContinueGame()
    {
        //todo load selected game state
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
