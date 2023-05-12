using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Marker
{
    public interface IMarkerCreator
    {

        /// <summary>
        /// Initializes the marker creator with the given configuration 
        /// </summary>
        /// <param name="markerConfiguration">The configuration</param>
        public void Initialize(MarkerConfiguration markerConfiguration);

        /// <summary>
        /// Removes old markers and creates new markers based on the given collection
        /// </summary>
        /// <param name="moves">A collection of positions, where movement markers should be displayed</param>
        /// <param name="attackMoves">A collection of positions, where attack markers should be displayed</param>
        /// <param name="selectionPosition">A position, where the selection marker should be displayed</param>
        public void CreateAndShowMarkers(IEnumerable<Vector3> moves, IEnumerable<Vector3> attackMoves, Vector3 selectionPosition);
        
        /// <summary>
        /// Removes old markers and creates a new marker based on the given position
        /// </summary>
        /// <param name="selectionPosition">The position to display the enemy selection marker</param>
        public void CreateEnemySelectionMarker(Vector3 selectionPosition);
        
        /// <summary>
        /// Removes all markers on the playground
        /// </summary>
        public void DestroyMarkers();
        

    }
}