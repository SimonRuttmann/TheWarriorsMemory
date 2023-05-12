using UnityEngine;

namespace Scripts.PieceMovement
{
	public interface IMover
	{
		
		public float CalculateMovementDuration(Transform pieceTransform, Vector3 targetPosition);
		
		public void MoveTo(Transform pieceTransform, Vector3 targetPosition);
		
	}
}