using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class LineMover : MonoBehaviour, IMover
    {
        private const float MovementSpeed = 30;

        public void MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            float distance = Vector3.Distance(targetPosition, pieceTransform.position);
            pieceTransform.DOMove(targetPosition, distance / MovementSpeed);
        }
    }
}
