using System;
using Scripts.Enums;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using Scripts.Toolbox;
using UnityEngine;

namespace Scripts.InGameLogic
{
    public static class RotationCalculator {
        
        public static Pair<double> CalcAngelForRunner(IGameFieldManager gameFieldManager, IPiece runnerPiece, Hexagon moveToCoordinates)
        {
            return CalcAngelForRotation(gameFieldManager, runnerPiece: runnerPiece, moveToCoordinates: moveToCoordinates);
        }

        public static Pair<double> CalcAngelForConflict(IGameFieldManager gameFieldManager, IPiece attackingPiece, IPiece hitPiece)
        {
            return CalcAngelForRotation(gameFieldManager, attackingPiece: attackingPiece, hitPiece: hitPiece, attack: true);
        }

        private static Pair<double> CalcAngelForRotation(IGameFieldManager gameFieldManager, IPiece attackingPiece = null, IPiece hitPiece = null, IPiece runnerPiece = null, Hexagon moveToCoordinates = null, bool attack = false)
        {
            IPiece piecePlayer, pieceEnemy;
            Vector3 absolutePositionAttackerOrStart, absolutePositionHitPieceOrFinish;


            Double rotationPointAttacker;

            bool movePlayer;

            Hexagon attackOrStart, hitOrFinish;
            Hexagon piecePlayerPosition, pieceEnemyPosition;

            if (attack)
            {
                attackOrStart = attackingPiece.Position;
                hitOrFinish = hitPiece.Position;
            
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
                attackOrStart = runnerPiece.Position;
                hitOrFinish = moveToCoordinates;
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
            absolutePositionAttackerOrStart = gameFieldManager.ResolveAbsolutePositionOfHexagon(attackOrStart);
            absolutePositionHitPieceOrFinish = gameFieldManager.ResolveAbsolutePositionOfHexagon(hitOrFinish);
            // wichtig zu wissen wer wen angreift? 

            //Note: Z is Y in Unity Terms
            // nur nach vorne laufen/ schauen auf Horizontaner ebene
            if (absolutePositionHitPieceOrFinish.z - absolutePositionAttackerOrStart.z == 0)
            {
            
                if (attack)
                {
                    if (attackingPiece.Team == Team.Enemy)
                    {
                     
                        if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 270;
                        else rotationPointAttacker = 90;
                    }
                 
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

                        if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 270;
                        else rotationPointAttacker = 90;
                    }

                    else
                    {
                        if (absolutePositionHitPieceOrFinish.x > absolutePositionAttackerOrStart.x) rotationPointAttacker = 90;
                        else rotationPointAttacker = 270;
                    }
                }
            
            }
            else
            {
           
                var absolutePositionPlayer = gameFieldManager.ResolveAbsolutePositionOfHexagon(piecePlayerPosition);
                var absolutePositionEnemy = gameFieldManager.ResolveAbsolutePositionOfHexagon(pieceEnemyPosition);

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

            var rotationPointDefender = rotationPointAttacker;

            if(attack)
            {
                if (attackingPiece.Team == Team.Player) { rotationPointAttacker = rotationPointAttacker - 180; }
                if (hitPiece.Team == Team.Player) { rotationPointDefender = rotationPointDefender - 180; }
            }

            return new Pair<Double>(rotationPointAttacker, rotationPointDefender);

        }


    }
}
