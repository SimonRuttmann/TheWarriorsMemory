using Scripts.GameField;
using Scripts.Marker;
using Scripts.PieceDeployment;
using Scripts.Pieces.Animation;
using Scripts.UI;
using UnityEngine;

namespace Scripts.InGameLogic
{
    /// <summary>
    /// This class holds all required configurations for the game
    /// </summary>
    public class GameConfiguration : MonoBehaviour
    {
        /// <summary>
        /// The piece deployment object, containing all pieces
        /// The pieces will be created after each start or restart of the game
        /// </summary>
        [SerializeField] 
        public PieceDeploymentConfiguration pieceDeploymentConfiguration;
        
        /// <summary>
        /// The piece deployment object, containing all pieces
        /// The pieces will be created after each start or restart of the game
        /// </summary>
        [SerializeField] 
        public GameFieldTerrainConfiguration gameFieldTerrainConfiguration;

        /// <summary>
        /// The reference to the playground used to execute game logic
        /// </summary>
        [SerializeField] 
        public Playground playground;
        
        /// <summary>
        /// The reference to the game ui manager
        /// </summary>
        [SerializeField] 
        public GameUiManager gameUIManager;
        
        /// <summary>
        /// The physical configuration of the game field
        /// </summary>
        [SerializeField] 
        public GameFieldPhysicalConfiguration gameFieldPhysicalConfiguration;
        
        /// <summary>
        /// The marker configurations, containing the attack and movement marker prefab
        /// </summary>
        [SerializeField]
        public MarkerConfiguration markerConfiguration;

        /// <summary>
        /// A reference to the piece creator
        /// </summary>
        [SerializeField] 
        public PieceCreator pieceCreator;
            
        /// <summary>
        /// A reference to the game field manager
        /// </summary>
        [SerializeField] 
        public GameFieldManager gameFieldManager;
        
        /// <summary>
        /// A reference to the marker creator
        /// </summary>
        [SerializeField] 
        public MarkerCreator markerCreator;

        /// <summary>
        /// A reference to the animation scheduler
        /// </summary>
        [SerializeField] 
        public AnimationScheduler animationScheduler;
        
        /// <summary>
        /// All prefabs of the pieces have to be added to this array 
        /// </summary>
        [SerializeField] 
        public GameObject[] pieceModels;
        
    }
}