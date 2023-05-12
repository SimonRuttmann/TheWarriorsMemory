
namespace Scripts.Pieces
{
    public class Mage : Piece
    {

        protected override void AddDefaultStats()
        {
            Health = 35;
            
            AttackDamage = 20;
            
            AttackRange = 3;
            MoveRange = 1;
        }
    }
}