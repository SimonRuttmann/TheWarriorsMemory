using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using UnityEngine;

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
        
        public IPiece Piece { get; }
        
        public AnimationStatus AnimationStatus { get; private set; }
        
        public float RotationValue { get; }
        
        public Hexagon TargetPosition { get; }

    }
}