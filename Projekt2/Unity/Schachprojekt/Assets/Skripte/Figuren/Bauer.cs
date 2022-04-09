using System.Collections.Generic;
using UnityEngine;

public class Bauer : Piece
{
    
    public override List<Vector2Int> GeneratePossibleMoves()
    {
        _possibleMoves.Clear();
        Vector2Int richtung = Team == Team.Player ? Vector2Int.up : Vector2Int.down;
        float reichweite = WurdeBewegt ? 1 : 2;
        for (int i = 1; i <= reichweite; i++)
        {
            Vector2Int nextCoords = Position + richtung * i;
            Piece piece = playground.GetFigurOnFeld(nextCoords);
            if (!playground.CheckObCoordsAufFeld(nextCoords))
                break;
            if (piece == null)
                AddPossibleMove(nextCoords);
            else
                break;
        }

        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, richtung.y), new Vector2Int(-1, richtung.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = Position + takeDirections[i];
            Piece piece = playground.GetFigurOnFeld(nextCoords);
            if (!playground.CheckObCoordsAufFeld(nextCoords))
                continue;
            if (piece != null && !piece.IsSameTeam(this))
            {
                AddPossibleMove(nextCoords);
            }
        }
        return _possibleMoves;
    }

    public override void MoveToCoord(Vector2Int coords)
    {
        base.MoveToCoord(coords);
        CheckBeförderung();
    }

    private void CheckBeförderung()
    {
        int endOfBrettYCoord = Team == Team.Player ? Playground.GesFeldGroesse - 1 : 0;
        if (Position.y == endOfBrettYCoord)
        {
            playground.BefoerdereFigur(this);
        }
    }
    
}