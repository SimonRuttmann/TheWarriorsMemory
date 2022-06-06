using System.Collections.Generic;
using Scripts.Enums;
using Scripts.GameField;
using Scripts.InGameLogic;

namespace Scripts.Pieces.Interfaces
{
    public interface IPiece : IDynamicStats, IAnimatable, IMovable, IAttackable
    {

        public Hexagon Position { get; set; }
        public Team Team { get; set; }

        public ISet<Hexagon> GenerateAllPossibleMovements();

        /// <summary>
        /// Sets all data, after the piece is created
        /// </summary>
        /// <param name="position">The logical position of the piece</param>
        /// <param name="team">The team of the piece</param>
        /// <param name="ground">The playground, the piece is attacked to</param>
        /// <param name="gameFieldManager">The game field manager of the current game</param>
        public void InitializePiece(Hexagon position, Team team, Playground ground, IGameFieldManager gameFieldManager);

        public bool IsSameTeam(Piece piece);
        
    }
}
