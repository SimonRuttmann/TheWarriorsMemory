using System;
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
            Debug.Log("CALCULATION:");
            var piecePosition = gameFieldManager.ResolveAbsolutePositionOfHexagon(piece.Position);
            var destinationPosition = gameFieldManager.ResolveAbsolutePositionOfHexagon(destination);
            Debug.Log("Hexagons: " + piece.Position.PosX + "|" + piece.Position.PosY + " , " + destination.PosX + "|" + destination.PosY);
            Debug.Log("Positions: " + piecePosition + " , " +  destinationPosition);
            
            var degree =  GetDegreeFromPointAToBZeroedToRight(piecePosition, destinationPosition);
            Debug.Log("degree: " + degree);
        //    var adjustedDegree = piece.Team == Team.Enemy ? 
        //        degree + 270 :       //Enemy looks to left
        //        degree + 90;          //Player looks to right
            
        //    return (float) adjustedDegree;
        return (float)degree;
        }
        
        private static double GetDegreeFromPointAToBZeroedToRight(Vector3 a, Vector3 b)
        {
            
            var oppositeSide = b.x - a.x;
            var adjacentSide = b.z - a.z;

            if (adjacentSide == 0) return GetDegreeFromPointAToBSameZ(a, b);
            
            var radians = Math.Atan((oppositeSide) / (adjacentSide));   //Radian
            if (radians < 0) radians += 2 * Math.PI;
            
            var degree =  (180 / Math.PI) * radians;                    //Degree

            if (a.z > b.z) degree += 180;
            
            //If the opposite Side and | or the adjacent Side is below 0,
            //it is necessary to add or subtract 180 degree,

            //Case 1
            //E.g. A to B is the arcTan(2-1 / 2-1) --> 45 deg
            
            //     B to A is the arcTan(1-2 / 1-2) --> 45 deg => Adjustment required

            //              B (2,2) 
            // A (1,1) 


            //Case 2
            //E.g. A to B is the arcTan(1- -2 /  1 - -2) -> arcTan(1) --> 45 deg
            //     B to A is the arcTan(-2 -1 / -2 -1 )  -> arcTan(1) --> 45 deg => Adjustment required

            //  B (1,1)         
            //              A (-2,-2) 



      //       var adjustedDegree = adjacentSide < 0 || oppositeSide < 0 ? degree + 180 : degree;
      //       return adjustedDegree;
            return degree;
        }

        private static double GetDegreeFromPointAToBSameZ(Vector3 a, Vector3 b)
        {
            Debug.Log("Degree same X");
            // A --> B          B <-- A
            return a.x < b.x ? 90 : 270;
        }    

    }
}
