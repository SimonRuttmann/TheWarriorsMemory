using UnityEngine;

namespace Scripts.GameField
{
    //Path to create the object --> Create --> Scriptable Object --> GameConfiguration --> Game Field Terrain Configuration
    [CreateAssetMenu(menuName = "Scriptable Objects/GameConfiguration/GameFieldTerrainConfiguration")]
    public class GameFieldTerrainConfiguration : ScriptableObject
    {
        
        [SerializeField] 
        public Vector2Int[] inaccessibleTerrain;
    }
}