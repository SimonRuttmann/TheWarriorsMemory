using Scripts.Extensions;
using Scripts.Toolbox;

namespace Scripts.GameField
{
    /// <summary>
    /// This class is responsible for resolving the correct hexagon based on a hexField 
    /// </summary>
    public static class HexagonResolver
    {
        public static Pair<int> ResolveHexagonLogicalPosition(
            double absoluteX, double absoluteY, GameFieldPhysicalConfiguration gameFieldPhysicalConfiguration)
        {
            //This implementation is based on the following post:
            //https://stackoverflow.com/questions/7705228/hexagonal-grids-how-do-you-find-which-hexagon-a-point-is-in
            
            var gridHeightOffset = gameFieldPhysicalConfiguration.HexagonWidthHeight / 4;
            
            var gridHeight = gameFieldPhysicalConfiguration.HexagonWidthHeight - gridHeightOffset;
            var gridWidth = gameFieldPhysicalConfiguration.HexagonWidthHeight;

            var y = absoluteY - gameFieldPhysicalConfiguration.StartPointX;
            var x = absoluteX - gameFieldPhysicalConfiguration.StartPointY;

            //Get the row and column, imagining that the hexagon grid is a square grid
            var rowAndColumn = GetRowAndColumn(x, y, gridHeight, gridWidth);
            var row = rowAndColumn.First;
            var column = rowAndColumn.Second;

            //Get the relative position in the imagined gird
            var relativeXAndY = GetRelativeXAndY(x, y, row, column, gridHeight, gridWidth);
            var relX = relativeXAndY.First;
            var relY = relativeXAndY.Second;
            
            //Resolve if the point in the grid is on the top left or top right triangle to
            //get the correct row and column
            var adjustedRowAndColumn = GetAdjustedRowAndColumn(relX, relY, row, column, gridHeight, gridWidth);
            var adjustedRow = adjustedRowAndColumn.First;
            var adjustedColumn = adjustedRowAndColumn.Second;

            return new Pair<int>(adjustedRow, adjustedColumn);
        }


        /// <summary>
        /// Calculates in which rectangle, e.g. row and column, the point is located
        /// </summary>
        /// <returns>Pair, containing the column and row</returns>
        private static Pair<int> GetRowAndColumn(
            double x, double y,
            double gridHeight, double gridWidth)
        {
            // Find the row and column of the box that the point falls in.
            var row = (int) (y / gridHeight);
            int column;
            
            var halfWidth = gridWidth / 2;

            if (row.IsOdd()) {
                //Offset x to match the indent of the row
                column = (int) ((x - halfWidth) / gridWidth); 
            }

            else {
                column = (int) (x / gridWidth);
            }

            return new Pair<int>(row, column);
        }

        /// <summary>
        /// Calculated the relative x and y position,
        /// e.g. the absolute position zeroed at the box level 
        /// </summary>
        /// <returns>Pair, containing the relative x and y position</returns>
        private static Pair<double> GetRelativeXAndY(
            double x, double y, 
            int row, int column, 
            double gridHeight, double gridWidth)
        {
            var halfWidth = gridWidth / 2;
            
            var relY = y - (row * gridHeight);
            double relX;

            if (row.IsOdd())
                relX = (x - (column * gridWidth)) - halfWidth;
            else
                relX = x - (column * gridWidth);

            
            return new Pair<double>(relX, relY);
        }

        /// <summary>
        /// Calculates based on the relative x and y coordinates
        /// within the given row and column, the new adjusted column and row  
        /// </summary>
        /// <returns>Pair containing the adjusted row and column</returns>
        private static Pair<int> GetAdjustedRowAndColumn(
            double relX, double relY, 
            int row, int column, 
            double gridHeight, double gridWidth)
        {
            var halfWidth = gridWidth / 2;
            
            var c = gridHeight / 4;
            var m = c / halfWidth;

            // Work out if the point is above either of the hexagon's top edges
            if (relY < (-m * relX) + c) // LEFT edge
            {
                row++;
                if (!row.IsOdd())
                    column--;
            }
            else if (relY < (m * relX) - c) // RIGHT edge
            {
                row++;
                if (row.IsOdd())
                    column++;
            }

            return new Pair<int>(row, column);
        }

    }
}