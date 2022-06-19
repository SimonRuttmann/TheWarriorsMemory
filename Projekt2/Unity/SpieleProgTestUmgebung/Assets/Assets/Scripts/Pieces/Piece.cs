using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Enums;
using Scripts.Extensions;
using Scripts.GameField;
using Scripts.InGameLogic;
using Scripts.PieceMovement;
using Scripts.Pieces.Animation;
using Scripts.Pieces.Interfaces;
using Scripts.Toolbox;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Pieces
{
	public abstract class Piece : MonoBehaviour, IPiece
	{

		private const string HealthBarTag = "HealthBar";
		private const string ActingModelTag = "ActingModel";
		private HealthBar _healthBar;
		
		#region Stats

		public int Health
		{
			get => _health;
			set
			{
				if (value < 0) value = 0;
				_healthBar.SetHealth(value);
				_health = value;
			}
		}

		private int _health;
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
		public AudioSource selectionSound;
		public AudioSource painSound;

		
		#endregion

		
		#region StatusOfRotation

		private Quaternion _startRotationValue;
		private Quaternion _endRotationValue;

		private float _timeCount;
		private bool _isRotationActive;

		#endregion
		

		#region Animation

		//Animator, will be used to set triggers to start the animation
		
		[NonSerialized]
		private Animator _animator;

		private IAnimationScheduler _animationScheduler;
		
		private static readonly int SelectionTrigger = Animator.StringToHash("SelectionTrigger");
		private static readonly int DyingTrigger = Animator.StringToHash("DyingTrigger");
		private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
		private static readonly int MoveTrigger = Animator.StringToHash("MoveTrigger");
		private static readonly int PainTrigger = Animator.StringToHash("PainTrigger");
		private static readonly int IdleTrigger = Animator.StringToHash("IdleTrigger");
		
		//Animation implementation

		public void SelectionAnimation()
		{
			_animator.SetTrigger(SelectionTrigger);
			selectionSound.Play();
		}

		public void DyingAnimation()
		{
			_animator.SetTrigger(DyingTrigger);
			dyingSound.Play();
		}

		public void AttackAnimation()
		{
			_animator.SetTrigger(AttackTrigger);
			attackSound.Play();
		}

		public void MoveAnimation(float timeToMove)
		{
			_animator.SetTrigger(MoveTrigger);
			moveSound.Play();
			ScheduleIdleAfterMove(timeToMove);
		}
		
		private void ScheduleIdleAfterMove(float time)
		{
			StartCoroutine(StartNextTurnAfterTime(time));
		}

		private IEnumerator StartNextTurnAfterTime(float time)
		{
			yield return new WaitForSeconds(time);
			_animator.SetTrigger(IdleTrigger);
		}       


		public void PainAnimation()
        {
			_animator.SetTrigger(PainTrigger);
			painSound.Play();
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
		
		
		public void MoveStraight(Hexagon targetPosition)
        {
			var targetCoordinates = _gameFieldManager.ResolveAbsolutePositionOfHexagon(targetPosition);
			var travelTime = _mover.CalculateMovementDuration(transform, targetCoordinates);
			MoveAnimation(travelTime);
			_mover.MoveTo(this.transform, targetCoordinates);
		}

		public float RotatePiece(Hexagon targetPosition)
		{
			var targetCoordinates = _gameFieldManager.ResolveAbsolutePositionOfHexagon(targetPosition);
			var travelTime = _mover.CalculateMovementDuration(transform, targetCoordinates);

			if (travelTime == 0f) return travelTime;

			Pair<Double> rotationAngel = RotationCalculator.CalcAngelForRunner(_gameFieldManager, this, targetPosition);

			var adjustedAngel = this.Team == Team.Player ? rotationAngel.First : rotationAngel.First - 180;

			RotatePiece((float) adjustedAngel);
			//_animationScheduler.RotatePiece(0f, this, (float)adjustedAngel);

			return travelTime;
		}
		public void RotatePieceBack()
        {
			var adjustedAngel = this.Team == Team.Player ? 90 : -90;
			_animationScheduler.RotatePiece(2f, this, (float)adjustedAngel);
		}
		#endregion


		#region Initialize

		private void Awake()
		{
			InitializeComponents();
		}
		
		
		private void InitializeComponents()
		{

			InitializeChildren();
			
			_mover = GetComponent<IMover>();
			moveSound.volume = 0.10f;
			
		}

		private GameObject _actingModel;

		private void InitializeChildren()
		{
			try
			{
				IList<GameObject> children = new List<GameObject>();
				
				for (var i = 0; i < transform.childCount; i++) 
					children.Add(transform.GetChild(i).gameObject);
				
				_actingModel = children.First(child => ActingModelTag.EqualsIgnoreCase(child.tag));
				var healthBarModel = children.First(child => HealthBarTag.EqualsIgnoreCase(child.tag));
				
				_healthBar = healthBarModel.transform.GetChild(0).gameObject.GetComponent<HealthBar>();
				
				_animator = _actingModel.GetComponent<Animator>();
			}
			catch (Exception)
			{
				throw new InvalidAmountChildrenException();
			}
		}
	
		public void InitializePiece(Hexagon position, Team team, Playground ground, 
			IGameFieldManager gameFieldManager, IAnimationScheduler animationScheduler)
		{
			Team = team;
			Position = position;
			playground = ground;
			_gameFieldManager = gameFieldManager;
			_animationScheduler = animationScheduler;
			
			//Place piece on the correct position in the playground
			transform.position = _gameFieldManager.ResolveAbsolutePositionOfHexagon(position);
			
			transform.Rotate(0, Team == Team.Player ? 90 : 270, 0);

			AddDefaultStats();
			_healthBar.SetMaxHealth(Health);
			_healthBar.SetTeam(Team);
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
		
		protected abstract void AddDefaultStats();
		
		public ISet<Hexagon> GenerateAllPossibleMovements()
		{
			_allPossibleMovements.Clear();
			
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
				ISet<Hexagon> hexagonsToCalculate = new HashSet<Hexagon>();
				hexagonsToCalculate.AddAll(hexagons);
				
				foreach (var hexagon in hexagonsToCalculate)
				{
					if (isMovement) AddOnMovement(hexagons, hexagon);
					else AddOnAttack(hexagons, hexagon);
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
