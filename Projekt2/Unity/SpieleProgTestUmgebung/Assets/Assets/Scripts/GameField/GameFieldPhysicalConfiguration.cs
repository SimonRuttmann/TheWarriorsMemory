using System;
using UnityEngine;

namespace Scripts.GameField
{
    /// <summary>
    /// This class holds information about the physical representation of the game field
    /// </summary>
    [Serializable]
    public class GameFieldPhysicalConfiguration
    {
        
        /// <summary>
        /// The width and height of a hexagon
        /// </summary>
        ///
        /// <remarks>
        /// <list>
        /// <listheader>Regular hexagon:</listheader>
        /// <item> - All sides are equal in length </item>
        /// <item> - All interior angles measure 120 degree </item>
        /// <item> - All exterior angles measure in 60 degree </item>
        /// </list>
        /// </remarks>
        ///
        /// <value>Required to be set in the inspector</value> 
        [SerializeField] 
        private double hexagonWidthHeight;
        
        /// <summary>
        /// The size of the square game field
        /// </summary>
        [SerializeField] 
        private int size;
        
        /// <summary>
        /// The transformation point on the bottom left
        /// </summary>
        /// <value>Required to be set in the inspector</value>
        [SerializeField] 
        private Transform startPoint;

        
        
        /// <summary>
        /// Indicates the width and height of a hexagon.
        /// Note that the hexagon requires to be regular
        /// </summary>
        /// <seealso cref="hexagonWidthHeight"/>
        public double HexagonWidthHeight
        {
            get => hexagonWidthHeight;
            set => hexagonWidthHeight = value;
        }
        
        /// <summary>
        /// The size of the square game field
        /// </summary>
        /// <seealso cref="size"/>
        public int Size
        {
            get => size;
            set => size = value;
        }

        /// <summary>
        /// The zeroing point of the game field
        /// </summary>
        /// <see cref="startPoint"/>
        private Transform StartPoint
        {
            get => startPoint;
            set => startPoint = value;
        }

        /// <summary>
        /// The logical zeroing x coordinate for the game field
        /// </summary>
        public float StartPointX => StartPoint.position.x;
        
        /// <summary>
        /// The logical zeroing y coordinate for the game field
        /// In the unity scene, this corresponds to the z coordinate
        /// </summary>
        public float StartPointY => StartPoint.position.z;

        /// <summary>
        /// The logical zeroing height for the game field
        /// In the unity scene, this corresponds to the y coordinate
        /// </summary>
        public float Height => StartPoint.position.y;

    }
}