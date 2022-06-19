using Scripts.Pieces.Interfaces;

namespace Scripts.Pieces.Animation
{
    public class AnimationSchedulerObject
    {
        public AnimationSchedulerObject(
            IPiece piece = null, 
            AnimationStatus animationStatus = AnimationStatus.Nothing, 
            float rotationValue = 0f)
        {
            Piece = piece;
            AnimationStatus = animationStatus;
            RotationValue = rotationValue;
        }

        public void ClearAnimationStatus()
        {
            AnimationStatus = AnimationStatus.Nothing;
        }
        
        public IPiece Piece { get; }
        
        public AnimationStatus AnimationStatus { get; private set; }
        
        public float RotationValue { get; }

    }
}