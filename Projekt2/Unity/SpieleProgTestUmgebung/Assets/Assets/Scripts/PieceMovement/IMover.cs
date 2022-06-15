using UnityEngine;

namespace Scripts.PieceMovement
{
	public interface IMover
	{
		public float MoveTo(Transform pieceTransform, Vector3 targetPosition, float duration);
	}
}