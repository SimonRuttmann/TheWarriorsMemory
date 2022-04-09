using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public class AnimationScheduler : MonoBehaviour, IAnimationScheduler
    {
        
        private readonly List<AnimationSchedulerObject> _scheduledObjects = new List<AnimationSchedulerObject>();

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
            
            _scheduledObjects.Add(new AnimationSchedulerObject(
                piece: piece, 
                animationStatus: AnimationStatus.Move, 
                targetCoordinates: targetCoords));
        }




    
        public void RotatePiece(float time, Piece rotatingPiece, float rotationValue)
        {
            StartCoroutine(RotationManager(time, rotatingPiece, rotationValue));
        }

        private IEnumerator RotationManager(float time, Piece rotationPiece, float rotationValue)
        {
            yield return new WaitForSeconds(time);

            _scheduledObjects.Add(new AnimationSchedulerObject(
                piece: rotationPiece, 
                animationStatus: AnimationStatus.Rotate, 
                rotationValue: rotationValue));

        }

        
        public void StartAnimation(float time, Piece piece, AnimationStatus animationStatus)
        {
            StartCoroutine(AnimationManager(time, piece, animationStatus));
        }


        private IEnumerator AnimationManager(float time, Piece piece, AnimationStatus animationStatus)
        {
            yield return new WaitForSeconds(time);

            _scheduledObjects.Add(new AnimationSchedulerObject(
                piece: piece, 
                animationStatus: animationStatus));
        }


        public void Update()
        {

            foreach (var animationScheduledObject in _scheduledObjects)
            {
                switch (animationScheduledObject.AnimationStatus)
                {
                    case AnimationStatus.Nothing: break;
                    case AnimationStatus.Move:    animationScheduledObject.Piece.MoveToCoord(animationScheduledObject.TargetCoordinates); break;
                    case AnimationStatus.Rotate:  animationScheduledObject.Piece.RotatePiece(animationScheduledObject.RotationValue); break;
                    case AnimationStatus.Attack:  animationScheduledObject.Piece.AttackAnimation(); break;
                    case AnimationStatus.Idle:    animationScheduledObject.Piece.IdleAnimation(); break;
                    case AnimationStatus.Die:     animationScheduledObject.Piece.DyingAnimation(); break;
                    case AnimationStatus.Delete:  Destroy(animationScheduledObject.Piece.gameObject); break;
                    default: throw new ArgumentOutOfRangeException();
                }
                
                animationScheduledObject.ClearAnimationStatus();
                _scheduledObjects.Remove(animationScheduledObject);
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
