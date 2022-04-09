using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Pieces.Interfaces
{
    public interface IAttackable
    {
        public bool IsAttackPossibleOn(Vector2Int position);

        public List<Vector2Int> GeneratePossibleAttacks();
    }
    
}