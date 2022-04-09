
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koenig : Piece
{

    Vector2Int[] richtungen = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    };

    private Piece linkerTurm;
    private Piece rechterTurm;

    private Vector2Int linksRochade;
    private Vector2Int rechtsRochade;

    public override List<Vector2Int> GeneratePossibleMoves()
    {
        _possibleMoves.Clear();
        BerechneStandartBewegungen();
        BerechneRochadeMoeglichkeiten();
        return _possibleMoves;

    }

    private void BerechneRochadeMoeglichkeiten()
    {
        linksRochade = new Vector2Int(-1, -1);
        rechtsRochade = new Vector2Int(-1, -1);
        if (!WurdeBewegt)
        {
            linkerTurm = GetFigurInRichtung<Turm>(Team, Vector2Int.left);
            if (linkerTurm && !linkerTurm.WurdeBewegt)
            {
                linksRochade = Position + Vector2Int.left * 2;
                _possibleMoves.Add(linksRochade);
            }
            rechterTurm = GetFigurInRichtung<Turm>(Team, Vector2Int.right);
            if (rechterTurm && !rechterTurm.WurdeBewegt)
            {
                rechtsRochade = Position + Vector2Int.right * 2;
                _possibleMoves.Add(rechtsRochade);
            }
        }
    }

    private Piece GetFigurInRichtung<T>(Team team, Vector2Int direction)
    {
        for (int i = 1; i <= Playground.GesFeldGroesse; i++)
        {
            Vector2Int nextCoords = Position + direction * i;
            Piece piece = playground.GetFigurOnFeld(nextCoords);
            if (!playground.CheckObCoordsAufFeld(nextCoords))
                return null;
            if (piece != null)
            {
                if (piece.Team != team || !(piece is T))
                    return null;
                else if (piece.Team == team && piece is T)
                    return piece;
            }
        }
        return null;
    }

    private void BerechneStandartBewegungen()
    {
        float range = 1;
        foreach (var direction in richtungen)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = Position + direction * i;
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
    }

    public override void MoveToCoord(Vector2Int coords)
    {
        base.MoveToCoord(coords);
        if (coords == linksRochade)
        {
            playground.UpdateSchachbrettOnFigurBewegt(coords + Vector2Int.right, linkerTurm.Position, linkerTurm, null);
            linkerTurm.MoveToCoord(coords + Vector2Int.right);
        }
        else if (coords == rechtsRochade)
        {
            playground.UpdateSchachbrettOnFigurBewegt(coords + Vector2Int.left, rechterTurm.Position, rechterTurm, null);
            rechterTurm.MoveToCoord(coords + Vector2Int.left);
        }
    }

}