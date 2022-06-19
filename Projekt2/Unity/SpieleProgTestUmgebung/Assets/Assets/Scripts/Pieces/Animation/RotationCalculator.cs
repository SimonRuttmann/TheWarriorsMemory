using System;
using Scripts.Enums;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using Scripts.Toolbox;
using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public static class RotationCalculator
    {

        //Default rotation values e.g. right, left
        private const float DefaultDegreePlayer = 90;
        private const float DefaultDegreeEnemy = -90;
        public const float RotationSpeed = 2;

        /// <summary>
        /// Returns the default rotation value for a piece 
        /// </summary>
        public static float GetDefaultRotation(IPiece piece)
        {
            return piece.Team == Team.Player ? DefaultDegreePlayer : DefaultDegreeEnemy;
        }
        
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
            
            var degreeAttackerToDefender = ResolveRotationToPosition(gameFieldManager, attackingPiece.Position, defendingPiece.Position);
            var degreeDefenderToAttacker = ResolveRotationToPosition(gameFieldManager, defendingPiece.Position, attackingPiece.Position);
            
            return new Pair<float>(degreeAttackerToDefender, degreeDefenderToAttacker);
        }
        
        /// <summary>
        /// Resolves the rotation from the a source position to the a destination position
        /// </summary>
        /// <param name="gameFieldManager">The game field manager, to resolve the absolute positions</param>
        /// <param name="source">The source field</param>
        /// <param name="destination">The destination field</param>
        /// <returns>The degree between the piece and the destination</returns>
        public static float ResolveRotationToPosition(IGameFieldManager gameFieldManager, Hexagon source, Hexagon destination)
        {

            var piecePosition = gameFieldManager.ResolveAbsolutePositionOfHexagon(source);
            var destinationPosition = gameFieldManager.ResolveAbsolutePositionOfHexagon(destination);
            
            var degree =  GetDegreeFromPointAToBZeroedToRight(piecePosition, destinationPosition);
            return (float)degree;
        }
        
        
        /// <summary>
        /// Returns the degree between the two given points.
        /// The zeroing axis is the vertical axis of the first given point.
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <returns>The degree at point a between the top vertical and the point b </returns>
        /// <remarks>
        /// 
        ///      arcTan(0) = 0                arcTan(0) = 0
        ///      archTan(1) = pi/4            arcTan(-1) = - pi/4
        ///      archTan(infinite) = pi/2     arcTan(-infinite) = - pi/2
        ///     
        ///      
        ///                               C +0  
        ///                -Pi/4  B               D    +Pi/4
        ///          -P/2 A             Start            E   +Pi/2
        ///                      H                 F      
        ///                               G
        ///     
        ///      Start to F -> Would be Start to B  -> add 180 deg
        ///      Start to G -> Would be Start to G  -> add 180 deg
        ///      Start to H -> Would be Start to D  -> add 180 deg
        ///     
        /// </remarks>
        private static double GetDegreeFromPointAToBZeroedToRight(Vector3 a, Vector3 b)
        {
            
            var oppositeSide = b.x - a.x;
            var adjacentSide = b.z - a.z;

            if (adjacentSide == 0) return GetDegreeFromPointAToBSameZ(a, b);
            
            var radians = Math.Atan((oppositeSide) / (adjacentSide));   //Radian
            var degree =  (180 / Math.PI) * radians;                    //Degree

            if (a.z > b.z) degree += 180;
            
            return degree;
        }

        private static double GetDegreeFromPointAToBSameZ(Vector3 a, Vector3 b)
        {
            // A --> B          B <-- A
            return a.x < b.x ? 90 : 270;
        }    

    }
}
