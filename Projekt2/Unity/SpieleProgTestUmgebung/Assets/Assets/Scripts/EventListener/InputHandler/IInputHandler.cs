using System;
using UnityEngine;

namespace Scripts.EventListener.InputHandler
{
    public interface IInputHandler
    {
        void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action onClick);
    }
}