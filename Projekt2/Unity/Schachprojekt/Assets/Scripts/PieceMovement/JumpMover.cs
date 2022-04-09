using DG.Tweening;
using UnityEngine;

namespace Scripts.PieceMovement
{
    public class JumpMover: MonoBehaviour, IMover
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        public void MoveTo(Transform pieceTransform, Vector3 targetPosition)
        {
            var distance = Vector3.Distance(targetPosition, pieceTransform.position);
            pieceTransform.DOJump(targetPosition, jumpHeight, 1, distance / movementSpeed);
        }
    }
}
