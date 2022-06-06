using System.Collections.Generic;
using Scripts.GameField;

namespace Scripts.Pieces.Interfaces
{
    public interface IAttackable
    {
        public bool IsAttackPossibleOn(Hexagon position);

        public ISet<Hexagon> GeneratePossibleAttackMovements();
    }
    
}