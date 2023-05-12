using System;
using Scripts.EventListener.InputHandler;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.EventListener.InputReceiver
{
    public abstract class AbstractInputReceiver : MonoBehaviour, IInputReceiver
    {
        private IInputHandler[] InputHandlers { get; set; }

        private void Awake()
        {
            InputHandlers = GetComponents<IInputHandler>();
        }

        protected void OnInputReceived(Vector3 inputPosition, GameObject selectedObject = null, Action onClick = null)
        {
            InputHandlers.ForEach(handler => handler.ProcessInput(inputPosition, selectedObject, onClick));
        }
        
    }
    
}