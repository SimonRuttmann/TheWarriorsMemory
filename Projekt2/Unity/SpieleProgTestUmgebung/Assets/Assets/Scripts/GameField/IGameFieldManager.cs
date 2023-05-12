using System.Collections.Generic;
using JetBrains.Annotations;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.GameField
{
    public interface IGameFieldManager
    {
        /// <summary>
        /// Initializes the game field for a given size
        /// by creating hexagon fields with it's position on the game field
        /// </summary>
        /// <param name="gameFieldPhysicalConfiguration">A data object storing physical attributes of the game field</param>
        /// <param name="gameConfigurationGameFieldTerrainConfiguration"></param>
        /// <seealso cref="GameFieldPhysicalConfiguration"/>
        public void Initialize(GameFieldPhysicalConfiguration gameFieldPhysicalConfiguration,
            GameFieldTerrainConfiguration gameConfigurationGameFieldTerrainConfiguration);

        /// <summary>
        /// Determines the hexagon next to the hexagon on the given position,
        /// relative to the given direction
        /// </summary>
        /// <param name="direction">The direction to move to</param>
        /// <param name="hexagon">The hexagon to move from</param>
        /// <returns>The optional hexagon next to the hexagon on the given position</returns>
        [CanBeNull]
        public Hexagon GetPosition(Directions direction, Hexagon hexagon);
        
        /// <summary>
        /// Determines the hexagon next to the hexagon on the given position,
        /// relative to the given direction
        /// </summary>
        /// <param name="direction">The direction to move to</param>
        /// <param name="x">The logical x position of the hexagon</param>
        /// <param name="y">The logical y position of the hexagon</param>
        /// <returns>The optional hexagon next to the hexagon on the given position</returns>
        [CanBeNull]
        public Hexagon GetPosition(Directions direction, int x, int y);

        /// <summary>
        /// Determines all hexagon fields next to the hexagon on the given position
        /// </summary>
        /// <param name="hexagon">The hexagon to get the surrounding fields from</param>
        /// <returns>A collection of surrounding hexagons</returns>
        public IEnumerable<Hexagon> GetSurroundingFields(Hexagon hexagon);

        /// <summary>
        /// Determines all hexagon fields next to the hexagon on the given position
        /// </summary>
        /// <param name="x">The logical y position of the hexagon</param>
        /// <param name="y">The logical x position of the hexagon</param>
        /// <returns>A collection of surrounding hexagons</returns>
        public IEnumerable<Hexagon> GetSurroundingFields(int x, int y);
        
        /// <summary>
        /// Resolves the Hexagon by a absolute coordinates
        /// </summary>
        /// <param name="x">The absolute y coordinate</param>
        /// <param name="y">The absolute x coordinate</param>
        [CanBeNull]
        public Hexagon ResolveHexagonByRelativePosition(double x, double y);
        
        /// <summary>
        /// Resolves the Hexagon by an absolute position
        /// </summary>
        /// <param name="position">The absolute position</param>
        [CanBeNull]
        public Hexagon ResolveHexagonByRelativePosition(Vector3 position);

        /// <summary>
        /// Resolves the absolute position of the given hexagon
        /// </summary>
        /// <param name="hexagon">The hexagon to get the position from</param>
        /// <returns>A Vector3 object, representing the absolute position</returns>
        public Vector3 ResolveAbsolutePositionOfHexagon(Hexagon hexagon);

        /// <summary>
        /// Removes a piece on a hexagon
        /// </summary>
        /// <param name="hexagon">The hexagon to clear the piece from</param>
        public void RemovePieceOnHexField(Hexagon hexagon);

        /// <summary>
        /// Moves a piece from a hexagon to another hexagon
        /// </summary>
        /// <param name="origin">The origin hexagon</param>
        /// <param name="target">The target hexagon</param>
        public void MovePieceToHexField(Hexagon origin, Hexagon target);

        /// <summary>
        /// Clears the board, within all its references
        /// Does not destroy the game objects
        /// </summary>
        public void Clear();

        /// <summary>
        /// Adds the given piece to the field
        /// </summary>
        /// <param name="pieceToAdd">The piece to ad</param>
        /// <param name="x">The logical x position</param>
        /// <param name="y">The logical y position</param>
        /// <returns></returns>
        public Hexagon AddPiece(IPiece pieceToAdd, int x, int y);
    }
}