using System.Collections.Generic;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Marker
{
    public class MarkerCreator : MonoBehaviour, IMarkerCreator
    {
        
        private GameObject _movementPrefab;
        private GameObject _attackPrefab;
        private GameObject _selectionPrefab;
        private GameObject _enemySelectionPrefab;
        
        private readonly List<GameObject> _instantiatedMarkers = new List<GameObject>();

        public void Initialize(MarkerConfiguration markerConfiguration)
        {
            _movementPrefab = markerConfiguration.MovementPrefab;
            _attackPrefab = markerConfiguration.AttackPrefab;
            _selectionPrefab = markerConfiguration.SelectionPrefab;
            _enemySelectionPrefab = markerConfiguration.EnemySelectionPrefab;
        }

        public void CreateAndShowMarkers(IEnumerable<Vector3> moves, IEnumerable<Vector3> attackMoves, Vector3 selectionPosition)
        {
            DestroyMarkers();

            moves.ForEach(move => InstantiateMarker(move, _movementPrefab));
            attackMoves.ForEach(attack => InstantiateMarker(attack, _attackPrefab));
            InstantiateMarker(selectionPosition, _selectionPrefab);
        }

        private void InstantiateMarker(Vector3 position, GameObject prefab)
        {
            var marker = Instantiate(prefab);
            marker.transform.position = position;
            
            _instantiatedMarkers.Add(marker);
        }
        
        public void CreateEnemySelectionMarker(Vector3 selectionPosition)
        {
            DestroyMarkers();
            InstantiateMarker(selectionPosition, _enemySelectionPrefab);
        }
        
        public void DestroyMarkers()
        {
            _instantiatedMarkers.ForEach(Destroy);
        }
        
    }
}