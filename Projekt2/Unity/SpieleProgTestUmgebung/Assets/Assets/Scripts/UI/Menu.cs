using System;
using Scripts.InGameLogic;
using UnityEngine;

namespace Scripts.UI
{
    public class Menu : MonoBehaviour
    {
        public enum MenuState
        {
            Closed,
            Main,
            Ingame,
            Endscreen
        }

        private enum Children
        {
            Menu,
            Menuborder,
            MainMenu,
            IngameMenu,
            EndScreen
        }
        
        private GameObject _menu;
        private GameObject _mainmenu;
        private GameObject _ingamemenu;
        private GameObject _endScreen;
        
        
        [SerializeField] private InGameManager inGameManager;
        
        
        private void Awake()
        {
            _menu = GameObject.FindGameObjectWithTag(Children.Menu.ToString());
            var border = _menu.transform.Find(Children.Menuborder.ToString());

            var main =border.transform.Find(Children.MainMenu.ToString());
            var ingame = border.transform.Find(Children.IngameMenu.ToString());
            var endscreen = _menu.transform.Find(Children.EndScreen.ToString());
            
            if(main == null || ingame == null) Debug.LogError("Submenus could not be fetched");

            _mainmenu = main.gameObject;
            _ingamemenu = ingame.gameObject;
            _endScreen = endscreen.gameObject;
        }

        /// <summary>
        /// Opens the main menu, called by the game ui manager
        /// </summary>
        public void OpenMainMenu()
        {
            ChangeToMenuType(MenuState.Main);
        }

        
        /// <summary>
        /// Is called by the menu
        /// </summary>
        public void NewGame()
        {
            inGameManager.RestartGame();  
            ChangeToMenuType(MenuState.Closed);
        }

        /// <summary>
        /// Closes the game, called within the menu
        /// </summary>
        public void CloseGame()
        {
            Debug.Log("Close Game, only applicable in a .exe");
            Application.Quit();
        }
        
        /// <summary>
        /// Requires to be called by some button from the player
        /// </summary>
        public void OpenIngameMenu()
        {
            ChangeToMenuType(MenuState.Ingame);
        }
        
        /// <summary>
        /// Is called within the InGame-Menu
        /// </summary>
        public void ContinueGame()
        {
            ChangeToMenuType(MenuState.Closed);
        }
        
        /// <summary>
        /// Is called by the game ui manager, when the game is finished
        /// </summary>
        public void OpenEndScreen()
        {
            ChangeToMenuType(MenuState.Endscreen);
        }

        public MenuState UiState { get; private set; }
        
        private void NotifyGameOnUiChange(MenuState uiState)
        {
            inGameManager.IsUiOpen = uiState != MenuState.Closed;
        }

        private void ChangeToMenuType(MenuState state)
        {
            NotifyGameOnUiChange(state);
            
            switch (state)
            {
                case MenuState.Main:
                    
                    _menu.SetActive(true);
                    _mainmenu.SetActive(true);
                    
                    _ingamemenu.SetActive(false);
                    _endScreen.SetActive(false);
                    
                    UiState = MenuState.Main;
                    break;
                
                case MenuState.Ingame:
                    
                    _menu.SetActive(true);
                    _ingamemenu.SetActive(true);
                    
                    _mainmenu.SetActive(false);
                    _endScreen.SetActive(false);
                    
                    UiState = MenuState.Ingame;
                    break;
                
                case MenuState.Closed:
                    
                    _menu.SetActive(false);
                    _ingamemenu.SetActive(false);
                    
                    _mainmenu.SetActive(false);
                    _endScreen.SetActive(false);
                    
                    UiState = MenuState.Closed;
                    break;
                
                case MenuState.Endscreen:
                    
                    _menu.SetActive(true);
                    _endScreen.SetActive(true);
                    
                    _ingamemenu.SetActive(false);
                    _mainmenu.SetActive(false);

                    UiState = MenuState.Endscreen;
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
