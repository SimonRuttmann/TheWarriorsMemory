using System.Collections.Generic;
using Scripts.Enums;
using Scripts.Extensions;
using Scripts.Pieces;
using Scripts.Pieces.Interfaces;

namespace Scripts.InGameLogic
{
	
	public class Player
	{
		public readonly Team Team;
		
		public IList<IPiece> RemainingPiecesOfPlayer { get; } = new List<IPiece>();
		
		public Player(Team team) {
			Team = team;
		}

		public bool HasNoMorePieces => RemainingPiecesOfPlayer.IsEmpty();

		public void AddPiece(Piece piece)
		{
			if (!RemainingPiecesOfPlayer.Contains(piece)) 
				RemainingPiecesOfPlayer.Add(piece);
		}

		public void RemovePiece(IPiece piece)
		{
			if (RemainingPiecesOfPlayer.Contains(piece))
				RemainingPiecesOfPlayer.Remove(piece);
		}
		

		internal void OnRestartGame()
		{
			RemainingPiecesOfPlayer.Clear();
		}
		
	}
}

