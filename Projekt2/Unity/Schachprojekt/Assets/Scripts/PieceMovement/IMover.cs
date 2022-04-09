using UnityEngine;

namespace Scripts.PieceMovement
{
	public interface IMover
	{
		public void MoveTo(Transform pieceTransform, Vector3 targetPosition);
	}
}