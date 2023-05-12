using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class JumpMover: MonoBehaviour, IMover
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;
        
        public float CalculateMovementDuration(Transform pieceTransform, Vector3 targetPosition)
        {
            var distance = Vector3.Distance(targetPosition, pieceTransform.position);
            return distance / movementSpeed;
        }

        public void MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            var duration = CalculateMovementDuration(pieceTransform, targetPosition);
            pieceTransform.DOJump(targetPosition, jumpHeight, 1, duration);
        }
    }
}
