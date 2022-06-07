using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class LineMover : MonoBehaviour, IMover
    {
        private const float MovementSpeed = 30;

        public float MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            var distance = Vector3.Distance(targetPosition, pieceTransform.position);
            var duration = distance / MovementSpeed;
            pieceTransform.DOMove(targetPosition, duration);

            return duration;
        }
    }
}
