using UnityEngine;

namespace Scripts.PieceMovement
{
    public class InstantMover : MonoBehaviour, IMover
    {
        public float MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            pieceTransform.position = targetPosition;
            return 0f;
        }
    }
}