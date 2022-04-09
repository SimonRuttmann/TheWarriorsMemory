using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springer : Piece
{
	Vector2Int[] springfelder = new Vector2Int[]
	{
		new Vector2Int(2, 1),
		new Vector2Int(2, -1),
		new Vector2Int(1, 2),
		new Vector2Int(1, -2),
		new Vector2Int(-2, 1),
		new Vector2Int(-2, -1),
		new Vector2Int(-1, 2),
		new Vector2Int(-1, -2),
	};

	public override List<Vector2Int> GeneratePossibleMoves()
	{
		_possibleMoves.Clear();

		for (int i = 0; i < springfelder.Length; i++)
		{
			Vector2Int nextCoords = Position + springfelder[i];
			Piece piece = playground.GetFigurOnFeld(nextCoords);
			if (!playground.CheckObCoordsAufFeld(nextCoords))
				continue;
			if (piece == null || !piece.IsSameTeam(this))
				AddPossibleMove(nextCoords);
		}
		return _possibleMoves;
	}
}