using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Ax,
    Spear,
    Projectile,
}

public class Weapon : EnemyHitter
{
    public WeaponType type;
    public Element element;
    public float movementSpeed;
    public AnimationClip baseAttack;
    public AnimationClip strongAttack;
}
