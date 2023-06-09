using System;
using System.Collections;
using System.Collections.Generic;
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

        

        private IEnumerator AnimationManager(float time, IPiece piece, AnimationStatus animationStatus)
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
                    case AnimationStatus.Rotate:  animationScheduledObject.Piece.RotatePiece(animationScheduledObject.RotationValue); break;
                    case AnimationStatus.Attack:  animationScheduledObject.Piece.AttackAnimation(); break;
                    case AnimationStatus.Pain:    animationScheduledObject.Piece.PainAnimation(); break;
                    case AnimationStatus.Die:     animationScheduledObject.Piece.DyingAnimation(); break;
                    case AnimationStatus.Delete:  DestroyPiece(animationScheduledObject.Piece); break;
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
