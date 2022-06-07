
namespace Scripts.Pieces
{
    public class Paladin : Piece
    {

        protected override void AddDefaultStats()
        {
            Health = 150;
            
            AttackDamage = 10;
            
            AttackRange = 1;
            MoveRange = 1;
        }
    }
}