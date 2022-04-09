using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Pieces.Interfaces
{
    public interface IMoveable
    {
    
        public bool IsMovePossibleTo(Vector2Int position);
    
        public List<Vector2Int> GeneratePossibleMoves();
    
        public void RotatePiece(float rotationAngle);

        public void MoveToCoord(Vector2Int coords);
    }
}
