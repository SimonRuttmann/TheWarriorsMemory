
namespace Scripts.Pieces
{
    public class Archer : Piece
    {
        
        protected override void AddDefaultStats()
        {
            Health = 50;
            
            AttackDamage = 10;
            
            AttackRange = 3;
            MoveRange = 3;
        }
        
    }
}