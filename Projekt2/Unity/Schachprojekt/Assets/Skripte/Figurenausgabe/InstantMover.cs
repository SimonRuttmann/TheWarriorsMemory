using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantMover : MonoBehaviour, IMover
{
    public void MoveTo(Transform transform, Vector3 zielPos)
    {
        transform.position = zielPos;
    }
}