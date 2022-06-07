using System;
using System.Collections;
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

        
        #region BlockInput

        private bool _blocker;
        
        
        private void BlockInput(float time)
        {
            _blocker = true;
            StartCoroutine(EnableInputAfterTime(time));
        }

        private IEnumerator EnableInputAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            _blocker = false;

        }        

        #endregion
        
        private IPiece _previousSelectedPiece;
        
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
            _previousSelectedPiece = null; 
            
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
            piece.InitializePiece(field, team, this, _gameFieldManager);
        }
        
        
        public void HandleFieldSelection(Vector3 inputPosition)
        {
            if (_blocker) return;

            var localSpaceInputPosition = transform.InverseTransformPoint(inputPosition);
                                    
            var field = _gameFieldManager.ResolveHexagonByRelativePosition(inputPosition);  //TODO if this does not work, use localSpaceInputPosition!
            
            //Click is outside the playground
            if (field == null) return;

           

            //New logic
            // Piece was selected and click on movable field
            if (_inGameManager.ActivePiece.IsAnyMovementPossibleTo(field))
            { 
                OnSelectedPieceMove(field, _inGameManager.ActivePiece);
            }
            
            /* Old chesslike logic
             
              var piece = field.Piece;
             
            if (_previousSelectedPiece == null)
            {
                if (piece == null || !_inGameManager.IsTurnOf(piece.Team)) return;
             
                // No preselection and click on piece within the players turn -> select piece
                piece.IdleAnimation();
                SelectPiece(piece);
                return;
            }


            // Select piece twice -> deselect
            if (piece != null && _previousSelectedPiece == piece)
            {
                DeselectPiece();
                return;
            }
                
            // Select piece while in players turn
            if (piece != null && _inGameManager.IsTurnOf(piece.Team))
            { 
                piece.IdleAnimation();
                SelectPiece(piece);
                return;
            }

            // Piece was selected and click on movable field
            if (_previousSelectedPiece.IsAnyMovementPossibleTo(field))
            { 
                OnSelectedPieceMove(field, _previousSelectedPiece);
            }
            */

        }

        //Was private in chess mode
        public bool SelectPiece(IPiece piece)
        {
            _previousSelectedPiece = piece;
            
            var attackMovements = _previousSelectedPiece.GeneratePossibleAttackMovements();
            var attackPositions = attackMovements.Select(move => _gameFieldManager.ResolveAbsolutePositionOfHexagon(move));

            var moveMovements = _previousSelectedPiece.GeneratePossibleMoveMovements();
            var movePositions = moveMovements.Select(move => _gameFieldManager.ResolveAbsolutePositionOfHexagon(move));

            if (attackMovements.IsEmpty() && moveMovements.IsEmpty()) return false;
            
            _markerCreator.CreateAndShowMarkers(movePositions, attackPositions);
            return true;
        }
        
        private void DeselectPiece()
        {
            _previousSelectedPiece = null;
            _markerCreator.DestroyMarkers();
        }
        //Was private in chess mode
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
            //Remove piece on old hexagon, add piece on new hexagon and update piece hexagon
            _gameFieldManager.MovePieceToHexField(piece.Position, destination);
            
            //Update piece reference to hexagon and start animation
            return piece.MoveToPosition(destination);
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

            if (isKilled)
            {
                //Remove piece from hexagon and player
                _gameFieldManager.RemovePieceOnHexField(pieceToHit.Position);
                _inGameManager.OnPieceRemoved(pieceToHit);
            }
            
            var waitingTime = StartConflict(selectedPiece, pieceToHit, isKilled);

            return waitingTime;
        }
        
      private float StartConflict(IPiece attackingPiece, IPiece hitPiece, bool isKilled)
      {
            IPiece piecePlayer, pieceEnemy;

            if (attackingPiece.Team == Team.Player)
            {
                piecePlayer = attackingPiece;
                pieceEnemy = hitPiece;
            }
            else
            {
                piecePlayer = hitPiece;
                pieceEnemy = attackingPiece;
            }

            double rotationPointAttacker, rotationPointDefender;

            var absolutePositionAttacker = _gameFieldManager.ResolveAbsolutePositionOfHexagon(attackingPiece.Position);
            var absolutePositionHitPiece = _gameFieldManager.ResolveAbsolutePositionOfHexagon(hitPiece.Position);
            
            //Note: Z is Y in Unity Terms
            if (absolutePositionHitPiece.z - absolutePositionAttacker.z == 0)
            {
                //TODO There seems to be some newer version, we didn't use
                if (attackingPiece.Team == Team.Enemy)
                {
                    //TODO old note: Dame schwarz greif an und es passt sgoar
                    if (absolutePositionHitPiece.x > absolutePositionAttacker.x) rotationPointAttacker = 270;
                    else rotationPointAttacker = 90;
                }
                //TODO old note: Dame weiss greift an -> Beide figuren in die verkehrte richtung
                else
                {
                    if (absolutePositionHitPiece.x > absolutePositionAttacker.x) rotationPointAttacker = 90;
                    else rotationPointAttacker = 270;
                }
            }
            else
            {
                
                var absolutePositionPlayer = _gameFieldManager.ResolveAbsolutePositionOfHexagon(piecePlayer.Position);
                var absolutePositionEnemy = _gameFieldManager.ResolveAbsolutePositionOfHexagon(pieceEnemy.Position);
                
                var oppositeSide = absolutePositionHitPiece.x - absolutePositionAttacker.x;
                var adjacentSide = absolutePositionHitPiece.z - absolutePositionAttacker.z;
                
                if (absolutePositionPlayer.z > absolutePositionEnemy.z)
                {
                    rotationPointAttacker = 180 + (180 / Math.PI) * Math.Atan((oppositeSide) / (adjacentSide));
                }
                else
                {
                    rotationPointAttacker = (180 / Math.PI) * Math.Atan((oppositeSide) / (adjacentSide));
                }
                
            }

            rotationPointDefender = rotationPointAttacker;

            if (attackingPiece.Team == Team.Player) { rotationPointAttacker = rotationPointAttacker - 180; }
            if (hitPiece.Team == Team.Player) { rotationPointDefender = rotationPointDefender - 180; }


            //Rotate attacker and defender
            _animationScheduler.RotatePiece(0f, attackingPiece, (float)rotationPointAttacker);
            _animationScheduler.RotatePiece(0f, hitPiece, (float)rotationPointDefender);

            //Execute attack
            _animationScheduler.StartAnimation(1f, attackingPiece, AnimationStatus.Attack);

            //Dying animation and delete
            if (isKilled)
            {
                _animationScheduler.StartAnimation(1.5f, hitPiece, AnimationStatus.Die);
                _animationScheduler.StartAnimation(4f, hitPiece, AnimationStatus.Delete);
            }
            
            //Rotate pieces back
            int rotateBackAttacker, rotateBackHit;

            if (attackingPiece.Team == Team.Player)
            {
                rotateBackAttacker = 180;
                rotateBackHit = 0;
            }
            else
            {
                rotateBackAttacker = 0;
                rotateBackHit = 180;
            }
            
            _animationScheduler.RotatePiece(6f, attackingPiece, rotateBackAttacker);
            
            if (!isKilled)
            {
                _animationScheduler.RotatePiece(6f, hitPiece, rotateBackHit);   
            }
            
            BlockInput(7f);
            return 7f;
      }
      
      

      
      public void KillAndRemovePiece(IPiece dyingPiece) 
      {
            _animationScheduler.StartEndAnimation(1.5f, dyingPiece); 
      }
      
    }
    
}