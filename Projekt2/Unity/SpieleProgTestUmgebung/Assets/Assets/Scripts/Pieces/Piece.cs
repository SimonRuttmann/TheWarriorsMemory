using System.Collections.Generic;
using System.Linq;
using Scripts.Enums;
using Scripts.Extensions;
using Scripts.GameField;
using Scripts.InGameLogic;
using Scripts.PieceMovement;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.Pieces
{
	public abstract class Piece : MonoBehaviour, IPiece
	{


		#region Stats

		public int Health { get; set; }
		public int AttackDamage { get; set; }
		public int MoveRange { get; set; }
		public int AttackRange { get; set; }
		public string Name { get; set; }
		public string DisplayType { get; set; }		

		#endregion
		

		#region AudioSources

		public AudioSource moveSound;
		public AudioSource attackSound;
		public AudioSource dyingSound;
		public AudioSource idleSound;
		
		#endregion

		
		#region StatusOfRotation

		private Quaternion _startRotationValue;
		private Quaternion _endRotationValue;

		private float _timeCount;
		private bool _isRotationActive;

		#endregion
		

		#region Animation

		//Animator, will be used to set triggers to start the animation
		
		public Animator animator;
		private static readonly int IdleTrigger = Animator.StringToHash("IdleTrigger");
		private static readonly int DyingTrigger = Animator.StringToHash("DyingTrigger");
		private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
		private static readonly int MoveTrigger = Animator.StringToHash("MoveTrigger");

		//Animation implementation
	
		public void IdleAnimation()
		{
			animator.SetTrigger(IdleTrigger);
			idleSound.Play();    
		}

		public void DyingAnimation()
		{
			animator.SetTrigger(DyingTrigger);
			dyingSound.Play();
		}

		public void AttackAnimation()
		{
			animator.SetTrigger(AttackTrigger);
			attackSound.Play();
		}

		public void MoveAnimation()
		{
			animator.SetTrigger(MoveTrigger);
		}
		
		#endregion

		
		#region Movement

		// Mover, will be called to physically move a piece
		private IMover _mover;
		
		//Movement implementation
	
		public void RotatePiece(float rotationAngle)
		{
			_endRotationValue = Quaternion.Euler(0, rotationAngle, 0);
			_startRotationValue = transform.localRotation;
			_isRotationActive = true;
			_timeCount = 0;
		}
		
		
		public void MoveToPosition(Hexagon position)
		{
			var targetCoordinates = _gameFieldManager.ResolveAbsolutePositionOfHexagon(position);
			Position = position;
			moveSound.Play();
			_mover.MoveTo(transform, targetCoordinates);
		}

		#endregion
		
		
		#region Initialize

		private void Awake()
		{
			InitializeComponents();
		}
		
		
		private void InitializeComponents()
		{
			animator = GetComponent<Animator>();
			_mover = GetComponent<IMover>();
			moveSound.volume = 0.10f;
		}
	
		public virtual void InitializePiece(Hexagon position, Team team, Playground ground, IGameFieldManager gameFieldManager)
		{
			Team = team;
			Position = position;
			playground = ground;
			_gameFieldManager = gameFieldManager;
			
			//Place piece on the correct position in the playground
			transform.position = _gameFieldManager.ResolveAbsolutePositionOfHexagon(position);

			if (Team == Team.Player) transform.Rotate(0, 180, 0); 
		
		}		

		#endregion


		#region Update

		public void Update()
		{

			if (transform.rotation != _endRotationValue && _isRotationActive) 
			{
				transform.rotation = Quaternion.Slerp(_startRotationValue, _endRotationValue, _timeCount);
				_timeCount += Time.deltaTime;
			}
		
			if (transform.rotation == _endRotationValue) _isRotationActive = false;
        
		}		

		#endregion

		
		public Hexagon Position { get; set; }
		
		public Playground playground;

		private IGameFieldManager _gameFieldManager;
		
		public Team Team { get; set; }

		private readonly ISet<Hexagon> _allPossibleMovements = new HashSet<Hexagon>();
		
		private readonly ISet<Hexagon> _possibleMoves = new HashSet<Hexagon>();
		
		private readonly ISet<Hexagon> _possibleAttacks = new HashSet<Hexagon>();
		
		//Logical implementation 
		
		public ISet<Hexagon> GenerateAllPossibleMovements()
		{
			var attacks = GeneratePossibleAttackMovements();
			var moves = GeneratePossibleMoveMovements();
			
			_allPossibleMovements.AddAll(attacks);
			_allPossibleMovements.AddAll(moves);
			
			return _allPossibleMovements;
		}

		public ISet<Hexagon> GeneratePossibleAttackMovements()
		{
			_possibleAttacks.Clear();

			var hexagons = AddSurroundArea(AttackRange, Position, false);
			var attackMoves = hexagons.Where(field => field.HasPiece && !field.Piece.IsSameTeam(this));
			
			_possibleAttacks.AddAll(attackMoves);

			return _possibleAttacks;
		}
		
		public ISet<Hexagon> GeneratePossibleMoveMovements()
		{
			_possibleMoves.Clear();

			var hexagons = AddSurroundArea(MoveRange, Position, true);
			var moves = hexagons.Where(field => field.IsDirectlyAccessible);
			
			_possibleMoves.AddAll(moves);

			return _possibleMoves;
		}

		private IEnumerable<Hexagon> AddSurroundArea(int range, Hexagon start, bool isMovement)
		{
			ISet<Hexagon> hexagons = new HashSet<Hexagon>();
			
			hexagons.Add(start);
			
			for (var i = 0; i < range; i++)
			{
				//TODO Check if this foreach is mutated by the inner conditions
				//TODO If so, use imperative loop
				foreach (var hexagon in hexagons)
				{
					if (isMovement)
					{
						AddOnMovement(hexagons, hexagon);
					}
					else
					{
						AddOnAttack(hexagons, hexagon);
					}

				}
			}
			
			hexagons.Remove(start);

			return hexagons;
		}

		private void AddOnMovement(ISet<Hexagon> hexagons, Hexagon hexagon)
		{
			hexagons.AddAll(_gameFieldManager.
				GetSurroundingFields(hexagon).Where(field => field.IsDirectlyAccessible));
		}
		
		private void AddOnAttack(ISet<Hexagon> hexagons, Hexagon hexagon)
		{
			hexagons.AddAll(_gameFieldManager.GetSurroundingFields(hexagon));
		}
		
		
		public bool IsSameTeam(Piece piece)
		{
			return Team == piece.Team;
		}

		public bool IsAnyMovementPossibleTo(Hexagon position)
		{
			return _allPossibleMovements.Contains(position);
		}

		public bool IsAttackPossibleOn(Hexagon position)
		{
			return _possibleAttacks.Contains(position);
		}
		
	}
}