using System;
using Scripts.Enums;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using Scripts.Toolbox;
using UnityEngine;

namespace Scripts.InGameLogic
{
    public static class RotationCalculator {

        
        /// <summary>
        /// Resolves the rotations between the attacker and defender
        /// </summary>
        /// <param name="gameFieldManager">The game field manager to resolve absolute positions</param>
        /// <param name="attackingPiece">The piece, which attacks</param>
        /// <param name="defendingPiece">The piece, which gets attacked</param>
        /// <returns>
        /// A pair. The first contains the rotation from the attacker to the defender.
        /// The second contains the rotation from the defender to the attacker
        /// </returns>
        public static Pair<float> ResolveRotationsOnAttackMode(IGameFieldManager gameFieldManager,
            IPiece attackingPiece, IPiece defendingPiece)
        {
            
            var degreeAttackerToDefender = ResolveRotationToPosition(gameFieldManager, attackingPiece, defendingPiece.Position);
            var degreeDefenderToAttacker = ResolveRotationToPosition(gameFieldManager, attackingPiece, defendingPiece.Position);
            
            return new Pair<float>(degreeAttackerToDefender, degreeDefenderToAttacker);
        }
        
        /// <summary>
        /// Resolves the rotation from the pieces position to the destination field
        /// </summary>
        /// <param name="gameFieldManager">The game field manager, to resolve the absolute positions</param>
        /// <param name="piece">The piece to move</param>
        /// <param name="destination">The destination field</param>
        /// <returns>The degree between the piece and the destination</returns>
        public static float ResolveRotationToPosition(IGameFieldManager gameFieldManager, IPiece piece, Hexagon destination)
        {
            
            var piecePosition = gameFieldManager.ResolveAbsolutePositionOfHexagon(piece.Position);
            var destinationPosition = gameFieldManager.ResolveAbsolutePositionOfHexagon(destination);

            var degree =  GetDegreeFromPointAToBZeroedToRight(piecePosition, destinationPosition);
            
            var adjustedDegree = piece.Team == Team.Enemy ? 
                degree + 270 :       //Enemy looks to left
                degree + 90;         //Player looks to right
            
            return (float) adjustedDegree;
        }
        
        private static double GetDegreeFromPointAToBZeroedToRight(Vector3 a, Vector3 b)
        {

            var oppositeSide = b.y - a.y;
            var adjacentSide = b.x - b.y;
            
            var radians = Math.Atan((oppositeSide) / (adjacentSide));   //Radian
            return (180 / Math.PI) * radians;                           //Degree
            
        }

    }
}
