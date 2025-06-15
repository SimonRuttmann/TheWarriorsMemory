using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class LineMover : MonoBehaviour, IMover
    {
        private const float MovementSpeed = 30;

        public float CalculateMovementDuration(Transform pieceTransform, Vector3 targetPosition)
        {
            var distance = Vector3.Distance(targetPosition, pieceTransform.position);
            return distance / MovementSpeed;
        }
        
        public void MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            var duration = CalculateMovementDuration(pieceTransform, targetPosition);
            pieceTransform.DOMove(targetPosition, duration);
        }
    }
}
