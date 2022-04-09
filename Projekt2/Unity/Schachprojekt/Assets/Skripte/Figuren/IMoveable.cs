using UnityEngine;

public interface IMoveable
{
    public void RotatePiece(float rotationAngle);

    public void MoveToCoord(Vector2Int coords);
}
