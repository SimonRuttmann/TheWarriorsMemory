using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Extensions;
using Scripts.Pieces.Interfaces;
using Scripts.Toolbox;
using UnityEngine;

namespace Scripts.GameField
{
    /// <summary>
    /// This class is responsible for granting easy access to the game field,
    /// by representing the hexagon field as a 2d array
    /// Therefore all calculation to hexagons are done by this class
    /// </summary>
    public sealed class GameFieldManager : MonoBehaviour, IGameFieldManager
    {
        
        private Hexagon[,] _hexField;
        private GameFieldPhysicalConfiguration _gameFieldPhysicalConfiguration;
        private GameFieldTerrainConfiguration _gameFieldTerrainConfiguration;
        
        
        public void Initialize(GameFieldPhysicalConfiguration gameFieldPhysicalConfiguration,
            GameFieldTerrainConfiguration gameConfigurationGameFieldTerrainConfiguration)
        {
            _hexField = new Hexagon[gameFieldPhysicalConfiguration.Size, gameFieldPhysicalConfiguration.Size];
          
            _gameFieldPhysicalConfiguration = gameFieldPhysicalConfiguration;
            _gameFieldTerrainConfiguration = gameConfigurationGameFieldTerrainConfiguration;
            // TODO _hexField.Length = 64 WHYYYYYYYY
            for (var i = 0; i < gameFieldPhysicalConfiguration.Size; i++)
                for (var j = 0; j < gameFieldPhysicalConfiguration.Size; j++)
                {
                    _hexField[i, j] = new Hexagon(i, j, IsInaccessibleTerrain(i,j));
                }
                    
        }

        private bool IsInaccessibleTerrain(int i, int j)
        {
            var inaccessible = _gameFieldTerrainConfiguration.inaccessibleTerrain;
            return inaccessible.Contains(new Vector2Int(i, j));
        }
        
        public Hexagon ResolveHexagonByRelativePosition(double x, double y)
        {
            var position = HexagonResolver.ResolveHexagonLogicalPosition(x, y, _gameFieldPhysicalConfiguration);
            var row = position.First;
            var column = position.Second;
            try
            {
                return _hexField[column, row];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public Hexagon ResolveHexagonByRelativePosition(Vector3 position)
        {
            //Note: In unity the z axis, describes the logical y axis
            return ResolveHexagonByRelativePosition(position.x, position.z);
        }

        public Vector3 ResolveAbsolutePositionOfHexagon(Hexagon hexagon)
        {
            var logicalPosX = hexagon.PosX;
            var logicalPosY = hexagon.PosY;

            var absolutePosX = _gameFieldPhysicalConfiguration.StartPointX +
                               logicalPosX * _gameFieldPhysicalConfiguration.HexagonWidthHeight;
            
            var absolutePosY = _gameFieldPhysicalConfiguration.StartPointY +
                               logicalPosY * _gameFieldPhysicalConfiguration.HexagonWidthHeight;
            
            if(logicalPosY.IsOdd()) absolutePosX += (_gameFieldPhysicalConfiguration.HexagonWidthHeight/2);
            
            return new Vector3((float)absolutePosX, _gameFieldPhysicalConfiguration.Height, (float)absolutePosY);
        }

        public void RemovePieceOnHexField(Hexagon hexagon)
        {
            hexagon.RemovePiece();
        }

        public void MovePieceToHexField(Hexagon origin, Hexagon target)
        {
            origin.MovePieceTo(target);
        }

        public void Clear()
        {
            foreach (var hexagon in _hexField)
            {
                hexagon.RemovePiece();
            }

            _hexField = null;
        }

        public Hexagon AddPiece(IPiece pieceToAdd, int x, int y)
        {
            _hexField[x, y].AddPiece(pieceToAdd);
            return _hexField[x, y];
        }

        public IEnumerable<Hexagon> GetSurroundingFields(Hexagon hexagon)
        {
            return GetSurroundingFields(hexagon.PosX, hexagon.PosY);
        }
        
        
        public IEnumerable<Hexagon> GetSurroundingFields(int x, int y)
        {
            IList<Hexagon> surroundingFields = new List<Hexagon>();
            
            EnumUtil.GetEnumValues<Directions>().ForEach(
                direction => surroundingFields.Add(GetPosition(direction, x, y)));

            return surroundingFields.Where(field => field != null);
        }

        
        public Hexagon GetPosition(Directions direction, Hexagon hexagon)
        {
            return GetPosition(direction, hexagon.PosX, hexagon.PosY);
        }

        
        public Hexagon GetPosition(Directions direction, int x, int y)
        {
           
            try
            {
                return
                    direction switch
                    {
                        Directions.TopLeft  => GetTopLeft(x, y),
                        Directions.Left     => GetLeft(x, y),
                        Directions.BotLeft  => GetBotLeft(x, y),
                        Directions.BotRight => GetBotRight(x, y),
                        Directions.Right    => GetRight(x, y),
                        Directions.TopRight => GetTopRight(x, y),
                        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                    };
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }
        

        private Hexagon GetTopRight(int x, int y)
        {
            return y.IsOdd() ? _hexField[x + 1, y + 1] : _hexField[x + 0, y + 1];
        }

        
        private Hexagon GetTopLeft(int x, int y)
        {
            return y.IsOdd() ? _hexField[x - 0, y + 1] : _hexField[x - 1, y + 1];
        }

        
        private Hexagon GetRight(int x, int y)
        {
            return _hexField[x + 1, y + 0];
        }

        
        private Hexagon GetLeft(int x, int y)
        {
            return _hexField[x - 1, y + 0];
        }

        
        private Hexagon GetBotLeft(int x, int y)
        {
            return y.IsOdd() ? _hexField[x - 0, y - 1] : _hexField[x - 1, y - 1];
        }
        
        
        private Hexagon GetBotRight(int x, int y)
        {
            return y.IsOdd() ? _hexField[x + 1, y - 1] : _hexField[x - 0, y - 1];
        }
        
        
    }
}

