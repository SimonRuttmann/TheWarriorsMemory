using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public class AnimationSchedulerObject
    {
        public AnimationSchedulerObject(
            Piece piece = null, 
            AnimationStatus animationStatus = AnimationStatus.Nothing, 
            float rotationValue = 0f, 
            Vector2Int targetCoordinates = new Vector2Int())
        {
            Piece = piece;
            AnimationStatus = animationStatus;
            RotationValue = rotationValue;
            TargetCoordinates = targetCoordinates;
        }

        
        public void ClearAnimationStatus()
        {
            this.AnimationStatus = AnimationStatus.Nothing;
        }
        
        public readonly Piece Piece;
        
        public AnimationStatus AnimationStatus;
        
        public readonly float RotationValue;
        
        public readonly Vector2Int TargetCoordinates;

    }
}