using Scripts.GameField;
using Scripts.Pieces.Interfaces;

namespace Scripts.Pieces.Animation
{
    public class AnimationSchedulerObject
    {
        public AnimationSchedulerObject(
            IPiece piece = null, 
            AnimationStatus animationStatus = AnimationStatus.Nothing, 
            float rotationValue = 0f, 
            Hexagon targetPosition = null)
        {
            Piece = piece;
            AnimationStatus = animationStatus;
            RotationValue = rotationValue;
            TargetPosition = targetPosition;
        }

        public void ClearAnimationStatus()
        {
            AnimationStatus = AnimationStatus.Nothing;
        }
        
        public readonly IPiece Piece;
        
        public AnimationStatus AnimationStatus;
        
        public readonly float RotationValue;
        
        public readonly Hexagon TargetPosition;

    }
}