using System;
using Scripts.InGameLogic;
using UnityEngine;

namespace Scripts.UI
{
    public class Menu : MonoBehaviour,IMenu
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

        [SerializeField] private InGameManager inGameManager;
        private void Awake()
        {
            _menu = GameObject.FindGameObjectWithTag("Menu");
            var menuborder = _menu.transform.Find("Menuborder");

            var main =menuborder.transform.Find("MainMenu");
            var ingame = menuborder.transform.Find("IngameMenu");
        
            if(main == null || ingame == null) Debug.LogError("Submenus could not be fetched");

            _mainmenu = main.gameObject;
            _ingamemenu = ingame.gameObject;
        }

        public void NewGame()
        {
            if (inGameManager.GameState.Equals(GameState.InGame))
            {
                inGameManager.RestartGame();    
            }
            else
            {
                inGameManager.StartNewGame();
            }
            ChangeToMenuType(MenuState.Closed);
        }

        public void LoadGame()
        {
            Debug.Log("Load Game");
            //todo load selected game state
        }

        public void RestartGame()
        {
            inGameManager.RestartGame();
            ChangeToMenuType(MenuState.Closed);
        }

        public void CloseGame()
        {
            Debug.Log("Close Game, only applicable in a .exe");
            Application.Quit();
        }

        public void BackToTitleScreen()
        {
            ChangeToMenuType(MenuState.Main);
        }

        public void SaveGame()
        {
            Debug.Log("SaveGame");
            //todo run funtion to save game, maybe open file browser
        }

        public void ContinueGame()
        {
            _menu.SetActive(false);
        }

        public void OpenIngameMenu()
        {
            ChangeToMenuType(MenuState.Ingame);
        }

        public void OpenMainMenu()
        {
            _menu.SetActive(true);
            ChangeToMenuType(MenuState.Main);
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
}
