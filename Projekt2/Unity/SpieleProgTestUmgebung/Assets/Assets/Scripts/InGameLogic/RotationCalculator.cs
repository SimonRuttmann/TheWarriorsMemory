using Scripts.Enums;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using Scripts.Toolbox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RotationCalculator {
    public static Pair<Double> CalcAngelForRunner(GameFieldManager _gameFieldManager, IPiece runnerPiece, Hexagon moveToCoordinates)
    {
        return CalcAngelForRotation(_gameFieldManager, runnerPiece: runnerPiece, moveToCoordinates: moveToCoordinates);
    }

    public static Pair<Double> CalcAngelForConflict(GameFieldManager _gameFieldManager, IPiece attackingPiece, IPiece hitPiece)
    {
        return CalcAngelForRotation(_gameFieldManager, attackingPiece: attackingPiece, hitPiece: hitPiece);
    }

    private static Pair<Double> CalcAngelForRotation(GameFieldManager _gameFieldManager, IPiece attackingPiece = null, IPiece hitPiece = null, IPiece runnerPiece = null, Hexagon moveToCoordinates = null, bool attack = false)
    {
        IPiece piecePlayer, pieceEnemy;
        Vector3 absolutePositionAttackerOrStart, absolutePositionHitPieceOrFinish, absolutePositionRunner, absolutePostitionFinish;

        //R�ckgaben
        Double rotationPointAttacker, rotationPointDefender, rotationPointRunner;

        bool movePlayer;

        Hexagon attackORStart, hitORFinish;
        Hexagon piecePlayerPosition, pieceEnemyPosition;

        if (attack)
        {
            attackORStart = attackingPiece.Position;
            hitORFinish = hitPiece.Position;
            
            if (attackingPiece.Team == Team.Player)
            {
                piecePlayer = attackingPiece;
                pieceEnemy = hitPiece;
            }
            else
            {
                piecePlayer = hitPiece;
                pieceEnemy = attackingPiece;
            }
            pieceEnemyPosition = pieceEnemy.Position;
            piecePlayerPosition = piecePlayer.Position;

        }
        else
        {
            attackORStart = runnerPiece.Position;
            hitORFinish = moveToCoordinates;
            if (runnerPiece.Team == Team.Player)
            {
                movePlayer = true;
                piecePlayerPosition = runnerPiece.Position;
                pieceEnemyPosition = moveToCoordinates;
            }
            else
            {
                movePlayer = false;
                piecePlayerPosition = moveToCoordinates;
                pieceEnemyPosition = runnerPiece.Position;
               
            }
        }
        absolutePositionAttackerOrStart = _gameFieldManager.ResolveAbsolutePositionOfHexagon(attackORStart);
        absolutePositionHitPieceOrFinish = _gameFieldManager.ResolveAbsolutePositionOfHexagon(hitORFinish);
        // wichtig zu wissen wer wen angreift? 

        //Note: Z is Y in Unity Terms
        // nur nach vorne laufen/ schauen auf Horizontaner ebene
        if (absolutePositionHitPieceOrFinish.z - absolutePositionAttackerOrStart.z == 0)
        {
            //TODO There seems to be some newer version, we didn't use
            // i can get that
            if (attack)
            {
                if (attackingPiece.Team == Team.Enemy)
                {
                    //TODO old note: Dame schwarz greif an und es passt sgoar
                    if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 270;
                    else rotationPointAttacker = 90;
                }
                //TODO old note: Dame weiss greift an -> Beide figuren in die verkehrte richtung
                else
                {
                    if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 90;
                    else rotationPointAttacker = 270;
                }
            }
            else
            {
                if (runnerPiece.Team == Team.Enemy)
                {
                    //TODO old note: Dame schwarz greif an und es passt sgoar
                    if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 270;
                    else rotationPointAttacker = 90;
                }
                //TODO old note: Dame weiss greift an -> Beide figuren in die verkehrte richtung
                else
                {
                    if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 90;
                    else rotationPointAttacker = 270;
                }
            }
            
        }
        else
        {
           
            var absolutePositionPlayer = _gameFieldManager.ResolveAbsolutePositionOfHexagon(piecePlayerPosition);
            var absolutePositionEnemy = _gameFieldManager.ResolveAbsolutePositionOfHexagon(pieceEnemyPosition);

            var oppositeSide = absolutePositionHitPieceOrFinish.x - absolutePositionAttackerOrStart.x;
            var adjacentSide = absolutePositionHitPieceOrFinish.z - absolutePositionAttackerOrStart.z;

            if (absolutePositionPlayer.z > absolutePositionEnemy.z)
            {
                rotationPointAttacker = 180 + (180 / Math.PI) * Math.Atan((oppositeSide) / (adjacentSide));
            }
            else
            {
                rotationPointAttacker = (180 / Math.PI) * Math.Atan((oppositeSide) / (adjacentSide));
            }
            

            

        }

        rotationPointDefender = rotationPointAttacker;

        if(attack)
        {
            if (attackingPiece.Team == Team.Player) { rotationPointAttacker = rotationPointAttacker - 180; }
            if (hitPiece.Team == Team.Player) { rotationPointDefender = rotationPointDefender - 180; }
        }

        return new Pair<Double>(rotationPointAttacker, rotationPointDefender);

    }


}
