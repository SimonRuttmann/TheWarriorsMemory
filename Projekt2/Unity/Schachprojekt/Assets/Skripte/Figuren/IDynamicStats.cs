using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDynamicStats
{

    void SetHealth(int health);

    void SetAttackDamage(int dmg);

    void SetMoveRange(int range);

    string SetName(string name);

    int GetHealth();

    int GetAttackDamage();

    int GetMoveRange();

    string GetName();

    string GetType();

}
