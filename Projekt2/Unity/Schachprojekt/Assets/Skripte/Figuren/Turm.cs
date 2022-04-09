using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turm : Piece
{
    private Vector2Int[] richtungen = new Vector2Int[] { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
    public override List<Vector2Int> GeneratePossibleMoves()
    {
        _possibleMoves.Clear();

        float reichweite = Playground.GesFeldGroesse;
        foreach (var richtung in richtungen)
        {
            for (int i = 1; i <= reichweite; i++)
            {
                Vector2Int nextCoords = Position + richtung * i;
                Piece piece = playground.GetFigurOnFeld(nextCoords);
                if (!playground.CheckObCoordsAufFeld(nextCoords))
                    break;
                if (piece == null)
                    AddPossibleMove(nextCoords);
                else if (!piece.IsSameTeam(this))
                {
                    AddPossibleMove(nextCoords);
                    break;
                }
                else if (piece.IsSameTeam(this))
                    break;
            }
        }
        return _possibleMoves;
    }


}