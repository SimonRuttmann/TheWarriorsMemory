using UnityEngine;

namespace Scripts.PieceMovement
{
    public class InstantMover : MonoBehaviour, IMover
    {
        public void MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            pieceTransform.position = targetPosition;
        }
    }
}