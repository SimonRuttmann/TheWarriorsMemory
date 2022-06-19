using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Scripts.GameField;
using Scripts.Pieces;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.AI
{
    public class Ai : IAi
    {
        public Hexagon GetAiMove(GameFieldManager gameFieldManager, IList<IPiece> playerPieces, IPiece ownPiece)
        {
            //Stupid ai, just for test..
            //With the gameFieldManager, the whole game field can easily transversed
            
            var attackMovements = ownPiece.GeneratePossibleAttackMovements();
            if (attackMovements.Count > 0) 
                return GetPriorityAttack(attackMovements);
            
            var moveMovements = ownPiece.GeneratePossibleMoveMovements();
            if (moveMovements.Count > 0)
                return GetHexagonClosestToEnemy(moveMovements, playerPieces);

            return null;
        }

        private Hexagon GetHexagonClosestToEnemy(ISet<Hexagon> possibleMoves, IList<IPiece> playerPieces)
        {
            KeyValuePair<int, Hexagon> closestMove = new KeyValuePair<int, Hexagon>(0, null);
            foreach (var possibleMove in possibleMoves)
            {
                foreach (var playerPiece in playerPieces)
                {
                    var distance = CalculateDistance(possibleMove, playerPiece.Position);
                    if (closestMove.Value == null || closestMove.Key > distance)
                    {
                        closestMove = new KeyValuePair<int, Hexagon>(distance, possibleMove);
                    }
                }
            }
            return closestMove.Value;
        }
        private int CalculateDistance(Hexagon hex1,Hexagon hex2)
        {
            var number1 = hex1.PosX - hex2.PosX;
            var number2 = hex1.PosY - hex2.PosY;
            
            if (number1 < 0) number1 = -number1;
            if (number2 < 0) number2 = -number2;

            return number1 + number2;
        }
        private Hexagon GetPriorityAttack(ISet<Hexagon> possibleAttacks)
        {
            KeyValuePair<int, Hexagon> priorityAttack = new KeyValuePair<int, Hexagon>(0,null);
            foreach (var possibleAttack in possibleAttacks)
            {
                var piece = possibleAttack.Piece;
                var priority = piece.Health - piece.AttackDamage;

                if (priorityAttack.Value == null || priorityAttack.Key > priority)
                    priorityAttack = new KeyValuePair<int, Hexagon>(priority, possibleAttack);
            }

            return priorityAttack.Value;
        }
        
    }
}