using System.Collections.Generic;
using System.Linq;
using Scripts.Pieces;
using Scripts.Pieces.Interfaces;

namespace Scripts.Toolbox
{
    public class PieceUtil
    {
        public IEnumerable<IPiece> GetPiecesOfType<T>(IEnumerable<IPiece> pieces) where T : Piece
        {
            return pieces.Where(piece => piece is T);
        }
    }
}