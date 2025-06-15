using UnityEngine;

namespace Scripts.PieceMovement
{
    public class InstantMover : MonoBehaviour, IMover
    {
        public float CalculateMovementDuration(Transform pieceTransform, Vector3 targetPosition)
        {
            return 0f;
        }

        public void MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            pieceTransform.position = targetPosition;
        }
        
    }
}