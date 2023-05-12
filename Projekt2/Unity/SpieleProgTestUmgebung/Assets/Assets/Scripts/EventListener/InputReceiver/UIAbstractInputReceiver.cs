using UnityEngine;
using UnityEngine.Events;

namespace Scripts.EventListener.InputReceiver
{
    /// <summary>
    /// Listens on the given UnityEvent onClick event
    /// </summary>
    public class UIInputReceiver : AbstractInputReceiver
    {
        
        /// <summary>
        /// The ui event to listen to
        /// Requires to be connected within the inspector
        /// </summary>
        [SerializeField] 
        private UnityEvent onClick;
        
        /// <summary>
        /// Requires to be connected within the inspector
        /// </summary>
        public void OnInputReceived()
        {
            base.OnInputReceived(Input.mousePosition, gameObject, () => onClick.Invoke());
        }
    }
}