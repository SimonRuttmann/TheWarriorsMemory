using System;
using Scripts.InGameLogic;
using UnityEngine;

namespace Scripts.EventListener.InputHandler
{
    [RequireComponent(typeof(Playground))]
    public class PlaygroundInputHandler : MonoBehaviour, IInputHandler
    {
        private Playground _playground;

        private void Awake()
        {
            _playground = GetComponent<Playground>();
        }

        public void ProcessInput(Vector3 position, GameObject selectedObject, Action onClick)
        {
            _playground.HandleFieldSelection(position);
        }
    }
}