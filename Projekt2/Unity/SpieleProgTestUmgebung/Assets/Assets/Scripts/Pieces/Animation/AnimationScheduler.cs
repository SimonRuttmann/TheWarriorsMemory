using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.GameField;
using Scripts.Pieces.Interfaces;
using UnityEngine;

namespace Scripts.Pieces.Animation
{
    public class AnimationScheduler : MonoBehaviour, IAnimationScheduler
    {
        
        private readonly List<AnimationSchedulerObject> _scheduledObjects = new List<AnimationSchedulerObject>();

        public void CleanDelete(float time, IPiece piece)
        {
            StartCoroutine(DeleteManager(time, piece));
        }

        private IEnumerator DeleteManager(float time, IPiece piece)
        {
       
            yield return new WaitForSeconds(time);

            DestroyPiece(piece);

        }

        public void MovePiece(float time, IPiece piece, Hexagon position)
        {
            StartCoroutine(MoveManager(time, piece, position));
        }

        private IEnumerator MoveManager(float time, IPiece piece, Hexagon targetPosition)
        {
            yield return new WaitForSeconds(time);
            
            _scheduledObjects.Add(new AnimationSchedulerObject(
                piece: piece, 
                animationStatus: AnimationStatus.Move, 
                targetPosition: targetPosition));
        }
        
        public void RotatePiece(float time, IPiece rotatingPiece, float rotationValue)
        {
            StartCoroutine(RotationManager(time, rotatingPiece, rotationValue));
        }

        private IEnumerator RotationManager(float time, IPiece rotationPiece, float rotationValue)
        {
            yield return new WaitForSeconds(time);

            _scheduledObjects.Add(new AnimationSchedulerObject(
                piece: rotationPiece, 
                animationStatus: AnimationStatus.Rotate, 
                rotationValue: rotationValue));

        }
        
        public void StartAnimation(float time, IPiece piece, AnimationStatus animationStatus)
        {
            StartCoroutine(AnimationManager(time: time, piece: piece, animationStatus: animationStatus));
        }

        public void MoveStraight(float time, IPiece piece, Vector3 targetCoordinates, float travelTime, Transform transform, AnimationStatus animationStatus)
        {
            StartCoroutine(AnimationManager(time: time, piece: piece, animationStatus: animationStatus, targetCoordinates: targetCoordinates, travelTime: travelTime, transform:transform));
        }


        private IEnumerator AnimationManager(float time, IPiece piece, AnimationStatus animationStatus, Vector3 targetCoordinates = new Vector3(), float travelTime = 0f, Transform transform = null)
        {
            yield return new WaitForSeconds(time);

            _scheduledObjects.Add(new AnimationSchedulerObject(
                piece: piece, 
                animationStatus: animationStatus,
                targetCoordinates: targetCoordinates,
                travelTime: travelTime,
                transform: transform));
        }


        public void Update()
        {

            foreach (var animationScheduledObject in _scheduledObjects)
            {
                switch (animationScheduledObject.AnimationStatus)
                {
                    case AnimationStatus.Nothing: break;
                    case AnimationStatus.Move:    animationScheduledObject.Piece.MoveToPosition(animationScheduledObject.TargetPosition); break;
                    case AnimationStatus.Rotate:  animationScheduledObject.Piece.RotatePiece(animationScheduledObject.RotationValue); break;
                    case AnimationStatus.Attack:  animationScheduledObject.Piece.AttackAnimation(); break;
                    case AnimationStatus.Select:  animationScheduledObject.Piece.SelectionAnimation(); break;
                    case AnimationStatus.Pain:    animationScheduledObject.Piece.PainAnimation(); break;
                    case AnimationStatus.Die:     animationScheduledObject.Piece.DyingAnimation(); break;
                    case AnimationStatus.Delete:  DestroyPiece(animationScheduledObject.Piece); break;
                    case AnimationStatus.MoveStraight: animationScheduledObject.Piece.MoveStraight(animationScheduledObject.TargetCoordinates, animationScheduledObject.TravelTime, animationScheduledObject.Transform); Debug.Log("here") ; break;
                    default: throw new ArgumentOutOfRangeException();
                }
                
                animationScheduledObject.ClearAnimationStatus();
            }
            _scheduledObjects.Clear();
        }

        public void StartEndAnimation(float time, IPiece dyingPiece)
        {
            StartCoroutine(EndAnimationsManager(time, dyingPiece));
        }


        private IEnumerator EndAnimationsManager(float time, IPiece dyingPiece)
        {
            dyingPiece.DyingAnimation();
            yield return new WaitForSeconds(time);

            DestroyPiece(dyingPiece);

        }

        private void DestroyPiece(IPiece dyingPiece)
        {
            Destroy(((Piece)dyingPiece).gameObject);
        }

    }
}
