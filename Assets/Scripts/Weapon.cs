using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Ax,
    Spear,
    Projectile
}

public class Weapon : Piece
{
    public WeaponType weaponType;
    public float movementSpeed;
    public int atk;
    [SerializeField] private LayerMask enemyLayer;
    
    public static EventHandler<EnemyCollisionArgs> OnEnemyCollision;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            Debug.Log("Enemy damage!");
            OnEnemyCollision?.Invoke(this, new EnemyCollisionArgs(other.gameObject.GetComponent<Enemy>(), this));
        }
    }
}

public class EnemyCollisionArgs : EventArgs 
{
    public EnemyCollisionArgs(Enemy e, Weapon h)
    {
        enemy = e;
        hitter = h;
    }

    public Enemy enemy;
    public Weapon hitter;
}