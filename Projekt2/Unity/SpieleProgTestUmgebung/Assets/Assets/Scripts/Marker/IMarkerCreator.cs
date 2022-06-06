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
        public void CreateAndShowMarkers(IEnumerable<Vector3> moves, IEnumerable<Vector3> attackMoves);
        
        /// <summary>
        /// Removes all markers on the playground
        /// </summary>
        public void DestroyMarkers();

    }
}