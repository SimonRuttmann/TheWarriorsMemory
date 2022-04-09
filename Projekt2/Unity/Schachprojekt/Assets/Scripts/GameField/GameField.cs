using System.Collections.Generic;
using Scripts.Extensions;

namespace Scripts.GameField
{
    /// <summary>
    /// This class is responsible for granting easy access to the game field,
    /// by representing the hexagon field as a 2d array
    /// Therefore all calculation to hexagons are done by this class
    /// </summary>
    public class GameField
    {
        private readonly Hexagon[,] _hexField = new Hexagon[10,10];

        //the hexField has to start with 0,0 which is graphically higher than 0,1

        public IEnumerable<Hexagon> GetSurroundingFields(int x, int y){
            IList<Hexagon> surroundingFields = new List<Hexagon>();
            
            surroundingFields.Add(this.GetTop( x, y));
            surroundingFields.Add(this.GetTopRight( x, y));
            surroundingFields.Add(this.GetBotRight( x, y));
            surroundingFields.Add(this.GetBot( x, y));
            surroundingFields.Add(this.GetBotLeft( x, y));
            surroundingFields.Add(this.GetTopLeft( x, y));

            return surroundingFields;
        }


        public Hexagon GetTopLeft(int x, int y)
        {
            return x.IsOdd() ? _hexField[x - 1, y] : _hexField[x - 1, y - 1];
        }




        public Hexagon GetTopRight(int x, int y)
        {
            return x.IsOdd() ? _hexField[x + 1,y] : _hexField[x + 1,y - 1];
        }



        public Hexagon GetBotLeft(int x, int y)
        {
            return x.IsOdd() ? _hexField[x - 1,y + 1] : _hexField[x - 1,y];
        }


        public Hexagon GetBotRight(int x, int y)
        {
            return x.IsOdd() ? _hexField[x + 1,y + 1] : _hexField[x + 1, y];
        }



        public Hexagon GetTop(int x, int y)
        {
            return _hexField[x, y - 1];
        }



        public Hexagon GetBot(int x, int y)
        {
            return _hexField[x, y + 1];
        }

        }
}

