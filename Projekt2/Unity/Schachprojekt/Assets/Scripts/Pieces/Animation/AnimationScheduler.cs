using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public class AnimationScheduler : MonoBehaviour, IAnimationScheduler
    {
        private IEnumerable<AnimationInformation> _animationInformations;
        
        private Piece _attackingPiece;
        private AnimationStatus _animationAttackingPiece = AnimationStatus.Nothing;
        
        private Piece _dyingPiece;
        private AnimationStatus _animationDyingPiece = AnimationStatus.Nothing;

        private Piece _movingPiece;
        private Vector2Int _targetCoordinates;
        private AnimationStatus _animationMovingPiece = AnimationStatus.Nothing;


        private Piece _rotatingPiece1;
        private float _rotationValuePiece1;
        private AnimationStatus _animationRotatePiece1 = AnimationStatus.Nothing;

        private Piece _rotatingPiece2;
        private float _rotationValuePiece2;
        private AnimationStatus _animationRotatePiece2 = AnimationStatus.Nothing;

        public void CleanDelete(float time, Piece piece)
        {
            StartCoroutine(DeleteManager(time, piece));
        }

        private IEnumerator DeleteManager(float time, Piece piece)
        {
       
            yield return new WaitForSeconds(time);

            Destroy(piece.gameObject);

        }

        public void MovePiece(float time, Piece piece, Vector2Int coordinates)
        {
            StartCoroutine(MoveManager(time, piece, coordinates));
        }

        private IEnumerator MoveManager(float time, Piece piece, Vector2Int targetCoords)
        {
            yield return new WaitForSeconds(time);
            
            _targetCoordinates = targetCoords;
            _movingPiece = piece;
            _animationMovingPiece = AnimationStatus.Move;

        }



        public void StartAnimation(float time, Piece attackingPiece, Piece dyingPiece, AnimationStatus animationStatus)
        {
            StartCoroutine(ConflictManager(time, attackingPiece, dyingPiece, animationStatus));
        }

    
        public void RotatePiece1(float time, Piece rotatingPiece, float rotationValue)
        {
            StartCoroutine(RotationManager(time, rotatingPiece, rotationValue, true));
        }

        public void RotatePiece2(float time, Piece rotationPiece, float rotationValue)
        {
            StartCoroutine(RotationManager(time, rotationPiece, rotationValue, false));
        }

        private IEnumerator RotationManager(float time, Piece rotationPiece, float rotationValue, bool isFirst)
        {
            yield return new WaitForSeconds(time);
            if (isFirst)
            {
                _rotationValuePiece1 = rotationValue;
                _rotatingPiece1 = rotationPiece;
                _animationRotatePiece1 = AnimationStatus.Rotate;
            }
            else
            {
                _rotationValuePiece2 = rotationValue;
                _rotatingPiece2 = rotationPiece;
                _animationRotatePiece2 = AnimationStatus.Rotate;
            }
        }


        private IEnumerator ConflictManager(float time, Piece attackingPiece, Piece dyingPiece, AnimationStatus animationStatus)
        {
            yield return new WaitForSeconds(time);

            if (attackingPiece != null)
            {
                _attackingPiece = attackingPiece;
                _animationAttackingPiece = animationStatus;
            }
            if (dyingPiece != null)
            {
                _dyingPiece = dyingPiece;
                _animationDyingPiece = animationStatus;
            }
       

        }


        public void Update()
        {
        
            switch (_animationAttackingPiece)
            {
                case AnimationStatus.Nothing:   break;
                case AnimationStatus.Attack:  _animationAttackingPiece = AnimationStatus.Nothing; _attackingPiece.AttackAnimation(); break;
                case AnimationStatus.Idle:     _animationAttackingPiece = AnimationStatus.Nothing; _attackingPiece.IdleAnimation(); break;
                case AnimationStatus.Die:  _animationAttackingPiece = AnimationStatus.Nothing; _attackingPiece.DyingAnimation(); break;
                case AnimationStatus.Delete: _animationAttackingPiece = AnimationStatus.Nothing; Destroy(_attackingPiece.gameObject); break;
            }
  
            switch (_animationDyingPiece)
            {
                case AnimationStatus.Nothing:   break;
                case AnimationStatus.Attack:  _animationDyingPiece = AnimationStatus.Nothing; _dyingPiece.AttackAnimation(); break;
                case AnimationStatus.Idle:     _animationDyingPiece = AnimationStatus.Nothing; _dyingPiece.IdleAnimation(); break;
                case AnimationStatus.Die:  _animationDyingPiece = AnimationStatus.Nothing; _dyingPiece.DyingAnimation(); break;
                case AnimationStatus.Delete: _animationDyingPiece = AnimationStatus.Nothing; Destroy(_dyingPiece.gameObject); break;
            }

            switch(_animationMovingPiece)
            {
                case AnimationStatus.Nothing:   break;
                case AnimationStatus.Move:  _animationMovingPiece = AnimationStatus.Nothing; _movingPiece.MoveToCoord(_targetCoordinates); break;
            }
            switch (_animationRotatePiece1)
            {
                case AnimationStatus.Nothing:   break;
                case AnimationStatus.Rotate:   _animationRotatePiece1 = AnimationStatus.Nothing; _rotatingPiece1.RotatePiece(_rotationValuePiece1); break;
            }
            switch (_animationRotatePiece2)
            {
                case AnimationStatus.Nothing: break;
                case AnimationStatus.Rotate: _animationRotatePiece2 = AnimationStatus.Nothing; _rotatingPiece2.RotatePiece(_rotationValuePiece2); break;
            }

        }

        public void StartEndAnimation(float time, Piece dyingPiece)
        {
            StartCoroutine(EndAnimationsManager(time, dyingPiece));
        }


        private IEnumerator EndAnimationsManager(float time, Piece dyingPiece)
        {
            dyingPiece.DyingAnimation();
            yield return new WaitForSeconds(time);

            Destroy(dyingPiece.gameObject);

        }




    }
}
