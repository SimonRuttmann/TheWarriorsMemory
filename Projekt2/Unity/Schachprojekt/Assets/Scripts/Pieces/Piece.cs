using System;
using System.Collections.Generic;
using Scripts.Enums;
using Scripts.PieceMovement;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.Pieces
{
	public abstract class Piece : MonoBehaviour, IPiece
	{
	
		//Stats

		public int Health { get; set; }
		public int AttackDamage { get; set; }
		public int MoveRange { get; set; }
		public int AttackRange { get; set; }
		public string Name { get; set; }
		public string DisplayType { get; set; }
	
	
	
		//Audio sources 
	
		public AudioSource moveSound;
		public AudioSource attackSound;
		public AudioSource dyingSound;
		public AudioSource idleSound;
	
	
	
		//Current status of rotation
	
		private Quaternion _startRotationValue;
		private Quaternion _endRotationValue;
	
		private float _timeCount;
		private bool _isRotationActive;
	
	
	
		//Animator, will be used to set triggers to start the animation
		public Animator animator;
		private static readonly int IdleTrigger = Animator.StringToHash("IdleTrigger");
		private static readonly int DyingTrigger = Animator.StringToHash("DyingTrigger");
		private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
		private static readonly int MoveTrigger = Animator.StringToHash("MoveTrigger");

	
		// Mover, will be called to physically move a piece
		private IMover _mover;
	
		public Playground playground;
	
		public Team Team { get; set; }
	
		public IEnumerable<Vector2Int> GetPossibleMoves()
		{
			return PossibleMoves;
		}

		public Vector2Int Position { get; set; }
	
		protected List<Vector2Int> PossibleMoves;
		protected List<Vector2Int> PossibleAttacks;



	
		public virtual void InitializePiece(Vector2Int position, Team team, Playground ground)
		{
			Team = team;
			Position = position;
			playground = ground;

			//Place piece on the correct position in the playground
			transform.position = playground.RelativePositionZumSchachbrettfeld(position);
		
			if (Team == Team.Player) transform.Rotate(0, 180, 0); 
		
		}

	
		// "Unity C-Tor"
		private void Awake()
		{
			animator = GetComponent<Animator>();
			_mover = GetComponent<IMover>();
			PossibleMoves = new List<Vector2Int>();
			moveSound.volume = 0.10f;
		}
	

		public void Update()
		{

			if (transform.rotation != _endRotationValue && _isRotationActive) 
			{
				transform.rotation = Quaternion.Slerp(_startRotationValue, _endRotationValue, _timeCount);
				_timeCount += Time.deltaTime;
			}
		
			if (transform.rotation == _endRotationValue) _isRotationActive = false;
        
		}
	
		//Movement implementation
	
		public void RotatePiece(float rotationAngle)
		{
			_endRotationValue = Quaternion.Euler(0, rotationAngle, 0);
			_startRotationValue = transform.localRotation;
			_isRotationActive = true;
			_timeCount = 0;
		}
	
	
		public virtual void MoveToCoord(Vector2Int coords)
		{
			var targetPosition = playground.KalkulierePosVonCoords(coords);
			Position = coords;
			moveSound.Play();
			_mover.MoveTo(transform, targetPosition);
		
		}
	
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


		//Logical implementation 

		public bool IsSameTeam(Piece piece)
		{
			return Team == piece.Team;
		}

		public bool IsMovePossibleTo(Vector2Int position)
		{
			return PossibleMoves.Contains(position);
		}

		protected void AddPossibleMove(Vector2Int position)
		{
			PossibleMoves.Add(position);
		}

		public List<Vector2Int> GeneratePossibleMoves()
		{
			throw new NotImplementedException();
			return null;
		}

		public bool IsAttackPossibleOn(Vector2Int position)
		{
			return PossibleAttacks.Contains(position);
		}
	
		protected void AddPossibleAttack(Vector2Int position)
		{
			PossibleAttacks.Add(position);
		}
	
		public List<Vector2Int> GeneratePossibleAttacks()
		{
			throw new NotImplementedException();
			return null;
		}

	}
}