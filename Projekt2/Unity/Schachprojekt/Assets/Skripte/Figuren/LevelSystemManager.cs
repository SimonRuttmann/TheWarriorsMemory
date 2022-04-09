using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystemManager : MonoBehaviour
{

    public void Init(IEnumerable<IDynamicStats> figuren)
    {

        foreach(var figur in figuren)
        {
            figur.GetName();
            figur.GetType();
            figur.GetHealth();
            figur.GetAttackDamage();
            figur.GetMoveRange();
        }
      

        

    }

    public void ApplyStats(IEnumerable<IDynamicStats> figuren)
    {
        foreach(var figur in figuren)
        {
            figur.SetName("abc");
            figur.SetHealth(10);
            figur.SetAttackDamage(10);
            figur.SetMoveRange(10);
        }

    }
    



}
