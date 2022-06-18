using UnityEngine;

namespace Scripts.UI
{
    public sealed class Billboard : MonoBehaviour
    {

        private Transform _cam;

        private void Awake()
        {
            _cam = GameObject.FindWithTag("MainCamera").transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _cam.forward);
        }
    }
}
