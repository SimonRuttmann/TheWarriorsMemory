
namespace Scripts.Pieces
{
    public class Warrior : Piece
    {

        protected override void AddDefaultStats()
        {
            Health = 100;
            
            AttackDamage = 20;
            
            AttackRange = 1;
            MoveRange = 2;
        }
    }
}