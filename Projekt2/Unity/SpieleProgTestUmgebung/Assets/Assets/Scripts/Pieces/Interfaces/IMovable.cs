using System.Collections.Generic;
using Scripts.GameField;

namespace Scripts.Pieces.Interfaces
{
    public interface IMovable
    {
    
        public bool IsAnyMovementPossibleTo(Hexagon position);
    
        public ISet<Hexagon> GeneratePossibleMoveMovements();
    
        public void RotatePiece(float rotationAngle);

        public float MoveToPosition(Hexagon position);
    }
}
