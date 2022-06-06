using System.Collections.Generic;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.Marker
{
    public class MarkerCreator : MonoBehaviour, IMarkerCreator
    {
        
        private GameObject _movementPrefab;
        private GameObject _attackPrefab;
        
        private readonly List<GameObject> _instantiatedMarkers = new List<GameObject>();

        public void Initialize(MarkerConfiguration markerConfiguration)
        {
            _movementPrefab = markerConfiguration.MovementPrefab;
            _attackPrefab = markerConfiguration.AttackPrefab;
        }

        public void CreateAndShowMarkers(IEnumerable<Vector3> moves, IEnumerable<Vector3> attackMoves)
        {
            DestroyMarkers();

            moves.ForEach(move => InstantiateMarker(move, true));
            attackMoves.ForEach(attack => InstantiateMarker(attack, false));
        }

        private void InstantiateMarker(Vector3 position, bool isMove)
        {
            var marker = Instantiate(isMove ? _movementPrefab : _attackPrefab);
            marker.transform.position = position;
            _instantiatedMarkers.Add(marker);
        }
        
        public void DestroyMarkers()
        {
            _instantiatedMarkers.ForEach(Destroy);
        }
    }
}