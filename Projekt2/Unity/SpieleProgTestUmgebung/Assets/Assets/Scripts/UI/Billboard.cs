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

/**
  //private const float FixedRotation = 0;
        private Transform _cam;
        private Transform _myTransform;
        private readonly Quaternion _fixedRotation =Quaternion.Euler(0f, 180f, 0f);

        private void Awake()
        {
            var e =             Quaternion.Euler(0f, 0f, 0f);
            ;
            _cam = GameObject.FindWithTag("MainCamera").transform;
        }

        private void Start()
        {
            _myTransform = transform;
        }

        private void LateUpdate()
        {
            Debug.Log("LateUpdate");
            var parentRotation = transform.parent.rotation;
            //var inverseRotation = Quaternion.Euler(-parentRotation.x, -parentRotation.y, -parentRotation.z);
            var inverseRotation = Quaternion.Inverse(parentRotation);
            inverseRotation.y = inverseRotation.y - 120f;
           // transform.LookAt(transform.position + _cam.forward);
            //transform.rotation = _fixedRotation;
            //transform.parent.rotation.
            transform.rotation = inverseRotation;
            //var eulerAngles = _myTransform.eulerAngles;
            //eulerAngles = new Vector3 (eulerAngles.x, FixedRotation, eulerAngles.z);
            //_myTransform.eulerAngles = eulerAngles;
        }
        */