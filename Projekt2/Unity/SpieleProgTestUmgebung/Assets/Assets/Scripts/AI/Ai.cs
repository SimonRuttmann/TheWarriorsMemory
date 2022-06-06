using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;

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
                return attackMovements.First();
            
            var moveMovements = ownPiece.GeneratePossibleMoveMovements();
            if (moveMovements.Count > 0)
                return moveMovements.First();

            //TODO The ai cant do any move
            throw new ArgumentNullException();
        }
    }
}