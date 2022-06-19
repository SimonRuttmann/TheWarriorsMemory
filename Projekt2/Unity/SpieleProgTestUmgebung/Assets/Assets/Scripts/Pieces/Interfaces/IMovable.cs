using System.Collections.Generic;
using Scripts.GameField;
using UnityEngine;

namespace Scripts.Pieces.Interfaces
{
    public interface IMovable
    {
    
        public bool IsAnyMovementPossibleTo(Hexagon position);
    
        public ISet<Hexagon> GeneratePossibleMoveMovements();
    
        public void RotatePiece(float rotationAngle);

        public void MoveStraight(Hexagon targetPosition);

        public float RotatePiece(Hexagon targetPosition);

        public void RotatePieceBack();
    }
}
