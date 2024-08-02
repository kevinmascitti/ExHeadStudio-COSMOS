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
    Punch,
}

public class Weapon : Piece
{

    //test
    [SerializeField] PlayerCharacter playerCharacter;
    //
    public WeaponType weaponType;
    public float movementSpeed;
    public int atk;
    [SerializeField] private LayerMask enemyLayer;
    
    
    public static EventHandler<EnemyCollisionArgs> OnEnemyCollision;

    public void Awake()
    {
        if ((weaponType == WeaponType.Ax))
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && playerCharacter.isFighting)//GetComponentInParent<PlayerCharacter>().isFighting)
        {
            Debug.Log("Preso");
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