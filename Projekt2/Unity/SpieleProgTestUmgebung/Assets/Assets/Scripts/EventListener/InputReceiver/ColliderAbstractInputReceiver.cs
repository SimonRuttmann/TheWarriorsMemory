using UnityEngine;

namespace Scripts.EventListener.InputReceiver
{
    /// <summary>
    /// Listens on MouseDownButton Events and executes processing
    /// with the hit position of the mouse down event
    /// </summary>
    public class ColliderInputReceiver : AbstractInputReceiver
    {
        private Vector3 _clickPosition;
        
        public void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
        
            _clickPosition = hit.point;
            
            OnInputReceived(_clickPosition);
        }
        
    }
}