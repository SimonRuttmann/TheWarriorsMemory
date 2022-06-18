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
            Ingame,
            Endscreen
        }
        private GameObject _menu;
        private GameObject _mainmenu;
        private GameObject _ingamemenu;
        private GameObject _endScreen;
        
        
        [SerializeField] private InGameManager inGameManager;
        private void Awake()
        {
            _menu = GameObject.FindGameObjectWithTag("Menu");
            var menuborder = _menu.transform.Find("Menuborder");

            var main =menuborder.transform.Find("MainMenu");
            var ingame = menuborder.transform.Find("IngameMenu");
            var endscreen = _menu.transform.Find("EndScreen");
            
            if(main == null || ingame == null) Debug.LogError("Submenus could not be fetched");

            _mainmenu = main.gameObject;
            _ingamemenu = ingame.gameObject;
            _endScreen = endscreen.gameObject;
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
            _endScreen.SetActive(false);
            _menu.SetActive(true);
            ChangeToMenuType(MenuState.Main);
        }

        public void OpenEndScreen()
        {
            ChangeToMenuType(MenuState.Endscreen);
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
                case MenuState.Endscreen:
                    _endScreen.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
