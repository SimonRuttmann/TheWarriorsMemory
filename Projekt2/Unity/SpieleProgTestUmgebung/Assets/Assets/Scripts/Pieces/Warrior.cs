using Scripts.Enums;
using Scripts.GameField;
using Scripts.InGameLogic;

namespace Scripts.Pieces
{
    public class Warrior : Piece
    {

        public override void InitializePiece(Hexagon position, Team team, Playground ground, IGameFieldManager gameFieldManager)
        {
            base.InitializePiece(position, team, ground, gameFieldManager);
            AttackRange = 1;
        }
    }
}