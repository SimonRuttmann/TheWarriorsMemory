using System.Linq;
using Scripts.Enums;
using Scripts.Extensions;
using Scripts.GameField;
using Scripts.Marker;
using Scripts.Pieces;
using Scripts.Pieces.Animation;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.InGameLogic
{
    /// <summary>
    /// The playground is responsible for managing the game field
    /// and all incoming inputs on the playground
    ///
    /// This includes logic to select, move or attack a piece as well
    /// as the creation, movement and deletion of pieces on the game field
    /// </summary>
    public sealed class Playground : MonoBehaviour
    {
        
        #region Configruation
        
        private GameFieldPhysicalConfiguration _gameFieldPhysicalConfiguration;
        
        private MarkerConfiguration _markerConfiguration;

        private GameFieldTerrainConfiguration _gameFieldTerrainConfiguration; 
        
        #endregion

        
        #region Dependencies

        private IGameFieldManager _gameFieldManager;
        private IMarkerCreator _markerCreator;
        private IAnimationScheduler _animationScheduler;

        private InGameManager _inGameManager;
        
        #endregion


        #region Initialize
        
        /// <summary>
        /// Requires to be set by the overlying game manager
        /// </summary>
        /// <param name="inGameManager">The game manager controlling the current game</param>
        /// <param name="gameConfiguration">Configurations to initialize required properties</param>
        public void InitializeGameManager(InGameManager inGameManager, GameConfiguration gameConfiguration)
        {
            _inGameManager = inGameManager;
            
            _markerConfiguration = gameConfiguration.markerConfiguration;
            _gameFieldPhysicalConfiguration = gameConfiguration.gameFieldPhysicalConfiguration;
            _gameFieldManager = gameConfiguration.gameFieldManager;
            _markerCreator = gameConfiguration.markerCreator;
            _animationScheduler = gameConfiguration.animationScheduler;
            _gameFieldTerrainConfiguration = gameConfiguration.gameFieldTerrainConfiguration;
            
            _gameFieldManager.Initialize(_gameFieldPhysicalConfiguration, _gameFieldTerrainConfiguration);
            _markerCreator.Initialize(_markerConfiguration);
        }
        
        #endregion
        
        public bool BlockInput { get; set; }
        
        
        internal void PrepareStart()
        {
            ClearGameState();
        }
        
        internal void PrepareRestart()
        {
            ClearGameState();
        }

        private void ClearGameState()
        {

            _gameFieldManager.Clear();
            _gameFieldManager.Initialize(_gameFieldPhysicalConfiguration, _gameFieldTerrainConfiguration);
            
            _markerCreator.DestroyMarkers(); 
            _markerCreator.Initialize(_markerConfiguration);
            
        }

        /// <summary>
        /// Adds and initializes the piece to the current game field
        /// </summary>
        /// <seealso cref="GameFieldManager.AddPiece"/>
        /// <seealso cref="Piece.InitializePiece"/>
        public void AddPieceToPlayground(IPiece piece, int logicalX, int logicalY, Team team)
        {
            var field = _gameFieldManager.AddPiece(piece, logicalX, logicalY);
            piece.InitializePiece(field, team, this, _gameFieldManager, _animationScheduler);
        }
        
        
        public void HandleFieldSelection(Vector3 inputPosition)
        {

            //Block inputs when game did is not active
            if (_inGameManager.GameState != GameState.InGame) return;
            
            //Block inputs while animation
            if (BlockInput) return;
            
            //Block inputs, while enemy executes turns
            if (_inGameManager.IsTurnOf(Team.Enemy)) return;
            
            var localSpaceInputPosition = transform.InverseTransformPoint(inputPosition);
            var field = _gameFieldManager.ResolveHexagonByRelativePosition(localSpaceInputPosition); 
            
            
            //Click is outside the playground
            if (field == null) return;
            
            _inGameManager.ActivePiece.GenerateAllPossibleMovements();
            
            // Piece was selected from InGameManager and click on movable field
            if (_inGameManager.ActivePiece.IsAnyMovementPossibleTo(field))
            { 
                OnSelectedPieceMove(field, _inGameManager.ActivePiece);
            }
            
            
        }

        
        /// <summary>
        /// Creates attack, movement and selection markers for a given piece
        /// </summary>
        /// <param name="piece">The piece to select</param>
        /// <returns>True, if any moves or attacks are possible</returns>
        public bool SelectPiece(IPiece piece)
        {

            var attackMovements = piece.GeneratePossibleAttackMovements();
            var attackPositions = attackMovements.Select(move => _gameFieldManager.ResolveAbsolutePositionOfHexagon(move));

            var moveMovements = piece.GeneratePossibleMoveMovements();
            var movePositions = moveMovements.Select(move => _gameFieldManager.ResolveAbsolutePositionOfHexagon(move));

            if (attackMovements.IsEmpty() && moveMovements.IsEmpty()) return false;

            var currentPosition = _gameFieldManager.ResolveAbsolutePositionOfHexagon(piece.Position);
            
            _markerCreator.CreateAndShowMarkers(movePositions, attackPositions, currentPosition);
            return true;
        }
        
        private void DeselectPiece()
        {
            _markerCreator.DestroyMarkers();
        }
        
        public void OnSelectedPieceMove(Hexagon destination, IPiece piece)
        {
            float waitingTime;
            
            if (destination.HasPiece)
                waitingTime = HitEnemyPiece(piece, destination.Piece);
            
            else
                waitingTime = MovePiece(destination, piece);
            
            DeselectPiece();
            
            _inGameManager.EndTurn(waitingTime);

        }

        
        private float MovePiece(Hexagon destination, IPiece piece)
        {
            // Rotete, Move, Rotete Back a piece
            var travelTime = piece.RotatePiece(destination);
            piece.MoveStraight(destination);

            // TODO move animationSchedulat here from piece
            piece.RotatePieceBack();
           
            //Remove piece on old hexagon, add piece on new hexagon and update piece hexagon
            _gameFieldManager.MovePieceToHexField(piece.Position, destination);

            return travelTime;
        }

        /// <summary>
        /// This method will start a animation sequence within the StartConflict method
        /// and DESTROY the GameObject when it is killed
        /// </summary>
        /// <param name="selectedPiece"></param>
        /// <param name="pieceToHit"></param>
        private float HitEnemyPiece(IPiece selectedPiece, IPiece pieceToHit)
        {
            
            var attackDmg = selectedPiece.AttackDamage;
            pieceToHit.Health -= attackDmg;

            var isKilled = pieceToHit.Health <= 0;
            
            var waitingTime = StartConflict(selectedPiece, pieceToHit, isKilled);
            
            if (isKilled)
            {
                //Remove piece from hexagon and player
                _gameFieldManager.RemovePieceOnHexField(pieceToHit.Position);
                _inGameManager.OnPieceRemoved(pieceToHit);
            }
            return waitingTime;
        }
        
      private float StartConflict(IPiece attackingPiece, IPiece hitPiece, bool isKilled)
      {

            var rotationValues = RotationCalculator.ResolveRotationsOnAttackMode(_gameFieldManager, attackingPiece, hitPiece);
            var rotationAttacker = rotationValues.First;
            var rotationDefender = rotationValues.Second;
            
            //Rotate attacker and defender
            _animationScheduler.RotatePiece(0f, attackingPiece, rotationAttacker);
            _animationScheduler.RotatePiece(0f, hitPiece, rotationDefender);

            //Execute attack
            _animationScheduler.StartAnimation(1f, attackingPiece, AnimationStatus.Attack);

            //Dying animation and delete
            if (isKilled) {
                _animationScheduler.StartAnimation(1.5f, hitPiece, AnimationStatus.Die);
                _animationScheduler.StartAnimation(4f, hitPiece, AnimationStatus.Delete);
            }
            else {
                _animationScheduler.StartAnimation(1.5f, hitPiece, AnimationStatus.Pain);
            }
            
            //Rotate pieces back
            _animationScheduler.RotatePiece(6f, attackingPiece,  RotationCalculator.GetDefaultRotation(attackingPiece));
            
            if (!isKilled) {
                _animationScheduler.RotatePiece(6f, hitPiece,    RotationCalculator.GetDefaultRotation(hitPiece));   
            }
            
            return 7f;
      }
      
      

      
      public void KillAndRemovePiece(IPiece dyingPiece) 
      {
            _animationScheduler.StartEndAnimation(1.5f, dyingPiece); 
      }
      
    }
    
}