
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour, IMover
{
    private float bewegungsGeschwindigkeit = 30;

    public void MoveTo(Transform transform, Vector3 zielPos)
    {
        float distance = Vector3.Distance(zielPos, transform.position);
        transform.DOMove(zielPos, distance / bewegungsGeschwindigkeit);
    }
}
