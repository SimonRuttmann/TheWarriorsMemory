using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class LineMover : MonoBehaviour, IMover
    {
        private const float MovementSpeed = 30;

        public float MoveTo(Transform pieceTransform, Vector3 targetPosition, float duration)
        {
            pieceTransform.DOMove(targetPosition, duration);

            return duration;
        }
    }
}
