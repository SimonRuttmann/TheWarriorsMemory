using System.Collections.Generic;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;

namespace Scripts.AI
{
    public class Ai : IAi
    {
        
        private readonly Dictionary<Hexagon, int> _distanceList = new Dictionary<Hexagon, int>();

        public Hexagon GetAiMove(GameFieldManager gameFieldManager, IList<IPiece> playerPieces, IPiece ownPiece)
        {
            //Stupid ai, just for test..
            //With the gameFieldManager, the whole game field can easily transversed
            
            var attackMovements = ownPiece.GeneratePossibleAttackMovements();
            if (attackMovements.Count > 0) 
                return GetPriorityAttack(attackMovements);
            
            var moveMovements = ownPiece.GeneratePossibleMoveMovements();
            if (moveMovements.Count > 0)
                return GetHexagonClosestToEnemy(moveMovements, playerPieces, ownPiece.AttackRange);

            return null;
        }


        /// <summary>
        /// Method is run when no attack able piece was found. 
        /// </summary>
        /// <returns>
        /// The first field from which an attack would be possible,
        /// or the closest field to an enemy
        /// </returns>
        private Hexagon GetHexagonClosestToEnemy(IEnumerable<Hexagon> possibleMoves, IList<IPiece> playerPieces, int attackRange)
        {
            _distanceList.Clear();
            foreach (var possibleMove in possibleMoves)
            {
                foreach (var playerPiece in playerPieces)
                {
                    var distance = CalculateDistance(possibleMove, playerPiece.Position);
                    if (_distanceList.ContainsKey(possibleMove))
                    {
                        if (_distanceList[possibleMove] > distance)
                            _distanceList[possibleMove] = distance;
                    }
                    else _distanceList.Add(possibleMove,distance);
                }
            }

            var closestNotReachingMove = new KeyValuePair<Hexagon, int>(null,0);
            
            foreach (var pair in _distanceList)
            {
                if (pair.Value == attackRange) return pair.Key;
                
                if (pair.Value > attackRange &&
                    (closestNotReachingMove.Key == null || closestNotReachingMove.Value > pair.Value))
                    closestNotReachingMove = pair;
            }
            return closestNotReachingMove.Key;
        }
        
        private int CalculateDistance(Hexagon hex1,Hexagon hex2)
        {
            var number1 = hex1.PosX - hex2.PosX;
            var number2 = hex1.PosY - hex2.PosY;
            
            if (number1 < 0) number1 = -number1;
            if (number2 < 0) number2 = -number2;

            return number1 + number2;
        }
        
        private Hexagon GetPriorityAttack(IEnumerable<Hexagon> possibleAttacks)
        {
            var priorityAttack = new KeyValuePair<Hexagon,int>(null,0);
            foreach (var possibleAttack in possibleAttacks)
            {
                var piece = possibleAttack.Piece;
                var priority = piece.Health - piece.AttackDamage;

                if (priorityAttack.Key == null || priorityAttack.Value > priority)
                    priorityAttack = new KeyValuePair<Hexagon,int>(possibleAttack, priority);
            }

            return priorityAttack.Key;
        }
        
    }
}