using System.Collections.Generic;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Pieces.Interfaces
{
    public interface IPiece : IDynamicStats, IAnimatable, IMoveable, IAttackable
    {

        public Vector2Int Position { get; set; }
        public Team Team { get; set; }

        public IEnumerable<Vector2Int> GetPossibleMoves();

        /// <summary>
        /// Sets all data, after the piece is created
        /// </summary>
        /// <param name="position">The logical position of the piece</param>
        /// <param name="team">The team of the piece</param>
        /// <param name="ground">The playground, the piece is attacked to</param>
        public void InitializePiece(Vector2Int position, Team team, Playground ground);

        public bool IsSameTeam(Piece piece);
    
    


    }
}
