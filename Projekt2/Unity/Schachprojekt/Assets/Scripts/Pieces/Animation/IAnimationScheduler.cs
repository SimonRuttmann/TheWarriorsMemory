using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public interface IAnimationScheduler
    {
        public void CleanDelete(float time, Piece piece);
        
        public void MovePiece(float time, Piece piece, Vector2Int coordinates);
        
        public void StartAnimation(float time, Piece attackingPiece, Piece dyingPiece, AnimationStatus animationStatus);
        
        public void RotatePiece1(float time, Piece rotatingPiece, float rotationValue);
        
        public void RotatePiece2(float time, Piece rotationPiece, float rotationValue);
        
        public void StartEndAnimation(float time, Piece dyingPiece);
    }
}