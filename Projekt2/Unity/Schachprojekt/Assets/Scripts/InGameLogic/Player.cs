using System.Collections.Generic;
using System.Linq;
using Scripts.Enums;
using Scripts.Pieces;

namespace Scripts.InGameLogic
{
	[System.Serializable] 
	public class Player
	{
		public Team team;
		public Playground playground;
		public List<Piece> remainingPiecesOfPlayer;

    
		public Player(Team team, Playground playground)
		{
			remainingPiecesOfPlayer = new List<Piece>();
			this.playground = playground;
			this.team = team;
		}

		public void AddPiece(Piece piece)
		{
			if (!remainingPiecesOfPlayer.Contains(piece)) 
				remainingPiecesOfPlayer.Add(piece);
		}

		public void RemovePiece(Piece piece)
		{
			if (remainingPiecesOfPlayer.Contains(piece))
				remainingPiecesOfPlayer.Remove(piece);
		}

		public void GenerateAllPossibleMoves()
		{ 
			remainingPiecesOfPlayer.
				Where(piece => playground.ContainsPiece(piece)).
				ToList().
				ForEach(piece => piece.GetPossibleMoves());
		}

		public IEnumerable<Piece> GetPiecesOfType<T>() where T : Piece
		{
			return remainingPiecesOfPlayer.Where(piece => piece is T);
		}

		internal void OnRestartGame()
		{
			remainingPiecesOfPlayer.Clear();
		}
		
	}
}

