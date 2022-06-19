using Scripts.Enums;
using UnityEngine;

namespace Scripts.UI
{
	public class GameUiManager : MonoBehaviour
	{
		[SerializeField] private GameObject menuObject;
		private Menu _menu;
		
		private void Awake()
		{
			_menu = menuObject.GetComponent<Menu>();
		}

		/// <summary>
		/// Starts the main menu
		/// </summary>
		public void StartUi()
		{
			_menu.OpenMainMenu();
		}

		public void Update()
		{
			EscapeListener();
		}
		
		private void EscapeListener()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ToggleInGameMenu();
			}
		}

		private void ToggleInGameMenu()
		{

			switch (_menu.UiState)
			{
				case Menu.MenuState.Closed:
					_menu.OpenIngameMenu();
					return;
				case Menu.MenuState.Ingame:
					_menu.ContinueGame();
					return;
				case Menu.MenuState.Main:
				case Menu.MenuState.Endscreen:
				default: return;
			}
		}

		/// <summary>
		/// Sets the team display in the ui
		/// </summary>
		/// <param name="activeTeam"></param>
		/// <remarks>Called at every team change</remarks>
		public void SetTeamDisplay(Team activeTeam)
		{
			
		}

		/// <summary>
		/// Starts the end screen
		/// </summary>
		/// <param name="winner">The winning team</param>
		/// <remarks>Is invoked at the end of the game</remarks>
		public void OnGameFinished(Team winner)
		{
			_menu.OpenEndScreen();
		}
	}
}
