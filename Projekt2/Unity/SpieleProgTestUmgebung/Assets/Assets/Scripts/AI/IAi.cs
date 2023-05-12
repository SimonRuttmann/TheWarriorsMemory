using System.Collections.Generic;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;

namespace Scripts.AI
{
    public interface IAi
    {
        /// <summary>
        /// Determines an destination tile based on its current piece
        /// </summary>
        /// <param name="gameFieldManager">The game field manager for the current game</param>
        /// <param name="playerPieces">The player's pieces</param>
        /// <param name="ownPiece">The piece, which has to attack</param>
        /// <returns></returns>
        public Hexagon GetAiMove( GameFieldManager gameFieldManager, IList<IPiece> playerPieces, IPiece ownPiece);
        
    }
}