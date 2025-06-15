using System;
using Scripts.Pieces.Interfaces;

namespace Scripts.GameField
{
    /// <summary>
    /// This class is responsible for calculation based on the shape of a hexagon
    /// Within this class areas can be determined, where the position of the click events can be determined
    /// In addition this field holds information about what kind of terrain, or which piece is located here
    /// </summary>
    public class Hexagon
    {
        /// <summary>
        /// The logical x position in the game field
        /// </summary>
        public int PosX { get;}
        
        /// <summary>
        /// The logical y position in the game field
        /// </summary>
        public int PosY { get;}
        
        /// <summary>
        /// Indicates, if pieces can move onto this hexagon
        /// </summary>
        public bool Inaccssible { get; }
        
        /// <summary>
        /// The piece, if the hexagon contains one
        /// Otherwise null
        /// </summary>
        /// <remarks>
        /// Note: The piece also has a reference to the node,
        /// which needs to be adjusted whenever this reference changes
        /// </remarks>
        public IPiece Piece { get; private set; }
        
        public bool HasPiece => Piece != null;
        
        public bool IsDirectlyAccessible => !HasPiece && !Inaccssible;
        
        public Hexagon(int x, int y, bool inaccessible)
        {
            PosX = x;
            PosY = y;
            Inaccssible = inaccessible;
        }

        public void RemovePiece()
        {
            if (!HasPiece) return;
            
            Piece.Position = null;
            Piece = null;
        }

        public void MovePieceTo(Hexagon hexagon)
        {
            if (!HasPiece) throw new ArgumentException();
            
            hexagon.Piece = Piece;
            Piece.Position = hexagon;

            Piece = null;
        }

        public void AddPiece(IPiece pieceToAdd)
        {
            Piece = pieceToAdd;
            pieceToAdd.Position = this;
        }
    }
}