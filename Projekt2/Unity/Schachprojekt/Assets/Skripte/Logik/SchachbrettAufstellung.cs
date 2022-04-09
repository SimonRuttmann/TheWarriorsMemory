using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Pfad um das Objekt zu erstellen -> Create -> Scriptable Objects -> Schach -> Schachbrett
[CreateAssetMenu(menuName = "Scriptable Objects/Schach/SchachbrettAufstellung")]
public class SchachbrettAufstellung : ScriptableObject
{
    
    [Serializable] private class Feld
    {
        public Vector2Int Position;     //x und y
        public Figurtyp Figurtyp;
        public Team team;
    }

    [SerializeField] private Feld[] Spielfeld;

    // 0        x = 1 y = 1 Turm Wei?
    // .... 
    //

    public int GetFigurenAnzahl()
    {
        return Spielfeld.Length;
    }

    // 0
    // -> Vector x = Turm x Position -1
    // -> Vector y = Turm y Position -1
    // -> (0,0) = "Logische" Position Turm Wei?


    public Vector2Int Get_XY_VonAufstellungsFigur(int index)
    {
        return new Vector2Int(Spielfeld[index].Position.x - 1, Spielfeld[index].Position.y - 1);
    }

    //Returnt den Namen von Prefab
    public string Get_Name_VonAufstellungsFigur(int index)
    {
        return Spielfeld[index].Figurtyp.ToString();
    }
    public Team Get_Farbe_VonAufstellungsFigur(int index)
    {
        return Spielfeld[index].team;
    }
}

