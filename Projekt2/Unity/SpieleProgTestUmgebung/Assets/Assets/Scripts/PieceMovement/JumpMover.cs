using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class JumpMover: MonoBehaviour, IMover
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        // right now jumpMover is not used
        public float MoveTo(Transform pieceTransform, Vector3 targetPosition, float duration)
        {
            pieceTransform.DOJump(targetPosition, jumpHeight, 1, duration);
            return duration;
        }
    }
}
