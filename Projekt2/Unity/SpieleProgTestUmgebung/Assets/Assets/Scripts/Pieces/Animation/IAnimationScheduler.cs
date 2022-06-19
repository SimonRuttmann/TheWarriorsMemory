using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public interface IAnimationScheduler
    {
        public void CleanDelete(float time, IPiece piece);
        
        
        public void StartAnimation(float time, IPiece piece, AnimationStatus animationStatus);
        
        public void RotatePiece(float time, IPiece rotatingPiece, float rotationValue);
        
        public void StartEndAnimation(float time, IPiece dyingPiece);

    }
}