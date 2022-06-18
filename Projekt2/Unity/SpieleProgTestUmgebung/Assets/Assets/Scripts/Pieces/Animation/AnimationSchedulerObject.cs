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
            Hexagon targetPosition = null,
            Vector3 targetCoordinates = new Vector3(),
            float travelTime = 0f,
            Transform transform = null )
        {
            Piece = piece;
            AnimationStatus = animationStatus;
            RotationValue = rotationValue;
            TargetPosition = targetPosition;
            TargetCoordinates = targetCoordinates;
            TravelTime = travelTime;
            Transform = transform;
        }

        public void ClearAnimationStatus()
        {
            AnimationStatus = AnimationStatus.Nothing;
        }
        
        public IPiece Piece { get; }
        
        public AnimationStatus AnimationStatus { get; private set; }
        
        public float RotationValue { get; }
        
        public Hexagon TargetPosition { get; }

        public Vector3 TargetCoordinates { get; }

        public float TravelTime { get; }

        public Transform Transform { get; }

    }
}