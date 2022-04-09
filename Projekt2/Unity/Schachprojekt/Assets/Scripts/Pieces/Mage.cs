using Scripts.Enums;
using UnityEngine;

namespace Scripts.Pieces
{
    public class Mage : Piece
    {

        public override void InitializePiece(Vector2Int position, Team team, Playground ground)
        {
            base.InitializePiece(position, team, ground);
            AttackRange = 5;
        }
    }
}