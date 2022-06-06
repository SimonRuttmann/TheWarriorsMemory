using System;
using Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class GameUiManager : MonoBehaviour
	{
		[SerializeField] private Menu _menu;

		//Wird beim Start ausgef�hrt
		//Beim ersten Start
		public void StartUi()
		{
			_menu.OpenMainMenu();
		}

		//Spiel starten/Fortsetzen button wird gedr�ckt
		//Alle weiteren spielstarts wird diese methode aufgerufen
		public void SpielStarten()
		{
			/*
			startText.text = "Fortsetzen";
			hauptmenue.SetActive(false);
		
			teamanzeige.SetActive(true);
			fadenkreuz.SetActive(true);
			neustartanzeige.SetActive(false);
			*/
		}

		//Wird bei nach einem Spielzug gesetzt
		public void SetTeamDisplay(Team farbe)
		{
			//todo 
			//if (farbe == Team.Player) teamtext.text = "Team Wei� ist an der Reihe";
			//else teamtext.text = "Team Schwarz ist an der Reihe";
		}

		//Wird nach dem Sieg eines Spielers angezeigt
		public void OnGameFinished(string winner)
		{
			//todo 
		}
	}
}
