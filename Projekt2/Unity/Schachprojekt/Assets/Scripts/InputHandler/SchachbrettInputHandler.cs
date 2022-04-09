using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Playground))]
public class SchachbrettInputHandler : MonoBehaviour, IInputHandler
{
    private Playground _playground;

    private void Awake()
    {
        _playground = GetComponent<Playground>();
    }

    public void VerarbeiteInput(Vector3 position, GameObject gewaehltesObjekt, Action onClick)
    {
        _playground.OnFeldAuswahl(position);
    }
}