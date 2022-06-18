using System.Collections;
using System.Collections.Generic;
using Scripts.AI;
using Scripts.Enums;
using Scripts.Extensions;
using Scripts.GameField;
using Scripts.PieceDeployment;
using Scripts.Pieces;
using Scripts.Pieces.Animation;
using Scripts.Pieces.Interfaces;
using Scripts.UI;
using UnityEngine;

namespace Scripts.InGameLogic
{
    public class InGameManager : MonoBehaviour
    {

        /// <summary>
        /// The GameConfiguration to configure the game
        /// </summary>
        [SerializeField] 
        private GameConfiguration gameConfiguration;
        
        private Playground _playground;
        private GameUiManager _gameUIManager;
        private PieceCreator _pieceCreator;
        private PieceDeploymentConfiguration _pieceDeploymentConfiguration;
        private IAnimationScheduler _animationScheduler;
        
        
        private Player _personPlayer;
        private Player _enemyPlayer;
        private Player _activePlayer;
        
        private GameState _gameState = GameState.Start;

        public GameState GameState => _gameState;

        private SortedDictionary<int, IPiece> _turnOrderActivePlayer = new SortedDictionary<int, IPiece>();
        private int _turnCounter;
        
        public IPiece ActivePiece { get; private set; }
        
        private IAi _ai= new Ai();

        public bool startGameWithUI = true;
        private void Awake()
        {
            InitializeConfiguration();
            CreatePlayers();
        }
        
        private void InitializeConfiguration()
        {
            _pieceCreator = gameConfiguration.pieceCreator;
            _playground = gameConfiguration.playground;
            _gameUIManager = gameConfiguration.gameUIManager;
            _pieceDeploymentConfiguration = gameConfiguration.pieceDeploymentConfiguration;
            _animationScheduler = gameConfiguration.animationScheduler;
            
            _playground.InitializeGameManager(this, gameConfiguration);
            _pieceCreator.Initialize(gameConfiguration.pieceModels);
        }

        private void CreatePlayers()
        {
            _personPlayer = new Player(Team.Player);
            _enemyPlayer = new Player(Team.Enemy);
        }

        /// <summary>
        /// Event function, starts the game
        /// </summary>
        private void Start()
        {
            if(startGameWithUI) _gameUIManager.StartUi();
            else StartNewGame();
        }
        
        public void StartNewGame()
        {
            _playground.PrepareStart();

            _activePlayer = _personPlayer;     
            
            DeployPieces(_pieceDeploymentConfiguration);
            _gameUIManager.SetTeamDisplay(Team.Player);

            PrepareTurn();
            GetNextTurn();
            _gameState = GameState.InGame;
        }
        
        /// <summary>
        /// This method needs to be called from the ui to restart the game
        /// </summary>
        public void RestartGame()
        {
            IList<IPiece> remainingPieces = new List<IPiece>();
            remainingPieces.AddAll(_personPlayer.RemainingPiecesOfPlayer);
            remainingPieces.AddAll(_enemyPlayer.RemainingPiecesOfPlayer);

            _playground.PrepareRestart();
            
            _personPlayer.OnRestartGame();
            _enemyPlayer.OnRestartGame();
            
            DestroyPieces(remainingPieces);
            
            StartNewGame();
        }
        
        
        private void DeployPieces(PieceDeploymentConfiguration deploymentConfiguration)
        {
            
            for (var i = 0; i < deploymentConfiguration.GetAmountOfPieces(); i++)
            {
                //Resolve data form deployment object
                var logicalPosition = deploymentConfiguration.GetPositionOfPiece(i);
                var team = deploymentConfiguration.GetTeamOfPiece(i);
                var pieceType = deploymentConfiguration.GetTypeOfPiece(i);

                //Create and initialize piece
                var createdPiece = _pieceCreator.CreatePiece(pieceType).GetComponent<Piece>();
                _playground.AddPieceToPlayground(createdPiece, logicalPosition.x, logicalPosition.y, team);
                
                //Add piece to player
                if(team == Team.Player) _personPlayer.AddPiece(createdPiece);
                else _enemyPlayer.AddPiece(createdPiece);
                
            }
        }

        private void DestroyPieces(IEnumerable<IPiece> pieces)
        {
            foreach (var piece in pieces) 
            { 
                if(piece == null) continue;
              
                var concretePiece = (Piece) piece; 
                if(concretePiece != null  && concretePiece.gameObject != null) 
                    Destroy(concretePiece.gameObject); 
            }
        }
        
        public bool IsTurnOf(Team team)
        {
            return _activePlayer.Team == team;
        }
    
        /// <summary>
        /// This methods ends a turn after a given time,
        /// within the time all user inputs are blocked.
        /// After the time elapses a new turn is started
        /// </summary>
        /// <param name="timeToWait"></param>
        /// <remarks>
        /// Regardless if the player or the ai is active we
        /// schedule the next turn based on a waiting time,
        /// to avoid multiple actions at the same time
        /// </remarks>
        public void EndTurn(float timeToWait)
        {

            if (OtherPlayerOf(_activePlayer).HasNoMorePieces)
            {
                EndGame();
                return;
            }
            
            ScheduleNextTurn(timeToWait);
        }

        
        private void ScheduleNextTurn(float time)
        {
            _playground.BlockInput = true;
            StartCoroutine(StartNextTurnAfterTime(time));
        }

        private IEnumerator StartNextTurnAfterTime(float time)
        {
            yield return new WaitForSeconds(time);

            //Start next turn and enable user inputs on field
            _playground.BlockInput = false;
            GetNextTurn();
        }       


        private void PrepareTurn()
        {
            
            _turnOrderActivePlayer.Clear();
            
            _turnCounter = _activePlayer.RemainingPiecesOfPlayer.Count;
            for (var i = 0; i < _turnCounter; i++)
                _turnOrderActivePlayer.Add(i, _activePlayer.RemainingPiecesOfPlayer[i]);
            
        }

        private void GetNextTurn()
        {
            while (true)
            {
                if (_turnCounter == 0)
                {
                    ChangeActiveTeam();
                    PrepareTurn();
                }

                _turnCounter--;
                ActivePiece = _turnOrderActivePlayer[_turnCounter];
                
                ActivePiece.SelectionAnimation();
                
                //If a time a required use this animation scheduler instead
                //_animationScheduler.StartAnimation(0f, ActivePiece, AnimationStatus.Select);
                
                if (_activePlayer.Team == Team.Player)
                {
                    var areMovesAvailable = _playground.SelectPiece(ActivePiece);
                    
                    //Player can execute his turn
                    if (areMovesAvailable) return; 

                    //Players selected piece cant move 
                    continue;
                }

                var destination = _ai.GetAiMove(gameConfiguration.gameFieldManager, _personPlayer.RemainingPiecesOfPlayer, ActivePiece);
                ScheduleAiMove(AnimationConstants.SelectAnimationDuration, destination, ActivePiece);
                
                return;
            }
        }
        
        
        private void ScheduleAiMove(float time, Hexagon destination, IPiece piece)
        {
            StartCoroutine(StartMoveAfterTime(time, destination, piece));
        }

        private IEnumerator StartMoveAfterTime(float time, Hexagon destination, IPiece piece)
        {
            yield return new WaitForSeconds(time);

            //Start the ai move
            _playground.OnSelectedPieceMove(destination, piece);
        }   
        
        private void EndGame()
        {
            _gameUIManager.OnGameFinished(_activePlayer.Team.ToString());
            if (_activePlayer.Team == Team.Player)
            {
                _enemyPlayer.RemainingPiecesOfPlayer.ForEach(
                    p => _playground.KillAndRemovePiece(p));
            }
            else
            {
                _personPlayer.RemainingPiecesOfPlayer.ForEach(
                    p => _playground.KillAndRemovePiece(p));
            }
            
            _gameState = GameState.Finished;
        }

        private Player OtherPlayerOf(Player player)
        {
            return player == _personPlayer ? _enemyPlayer : _personPlayer;
        }

        //TODO adjustment for ...
        private void ChangeActiveTeam()
        {
        
            if (_activePlayer == _personPlayer) { 
                _activePlayer = _enemyPlayer; 
                _gameUIManager.SetTeamDisplay(Team.Enemy);
            }
            else {
                _activePlayer = _personPlayer;   
                _gameUIManager.SetTeamDisplay(Team.Player);
            }
        }

       
        /// <summary>
        /// Will be called from the Playground to remove the piece from the player
        /// The playground itself will clear all references to and from the piece,
        /// therefore we only need to remove the piece from the player
        /// </summary>
        /// <param name="piece"></param>
        internal void OnPieceRemoved(IPiece piece)
        {
            var pieceOwner = (piece.Team == Team.Player) ? _personPlayer : _enemyPlayer;
            pieceOwner.RemovePiece(piece);
        }

    }
}

