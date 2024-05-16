using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Ax,
    Spear
}

public class Weapon : EnemyHitter
{
    private WeaponType type;
    [NonSerialized] private Element element;

}
