using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Marker
{
    public class MarkerCreator : MonoBehaviour
    {
        
        [SerializeField] private GameObject movementPrefab;
        [SerializeField] private GameObject attackPrefab;
        
        private readonly List<GameObject> _instantiatedMarkers = new List<GameObject>();

        /// <summary>
        /// Removes old markers and creates new markers based on the given dictionary
        /// </summary>
        /// <param name="markers">Dictionary containing the position
        /// and the information which marker should be displayed.
        /// True = movement marker, False = attack marker</param>
        public void CreateAndShowMarkers(Dictionary<Vector3, bool> markers)
        {
            DestroyMarkers();
            
            foreach (var pair in markers)
            {
                var marker = Instantiate(pair.Value ? movementPrefab : attackPrefab);
                marker.transform.position = pair.Key;
                
                _instantiatedMarkers.Add(marker);
            }
        }

        public void DestroyMarkers()
        {
            _instantiatedMarkers.ForEach(Destroy);
        }
    }
}