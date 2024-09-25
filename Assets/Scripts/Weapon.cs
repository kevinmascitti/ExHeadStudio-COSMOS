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
        BaseAttack1State.OnAttackBase1 += ActivateRxPiece;
        BaseAttack1State.OnAttackBase1Finished += DeactivateRxPiece;
        BaseAttack2State.OnAttackBase2 += ActivateSxPiece;
        BaseAttack2State.OnAttackBase2Exit += DeactivateSxPiece;
        GetComponent<BoxCollider>().enabled = false;
    }
    
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") /*&& playerCharacter.isFighting*/ && !playerCharacter.enemiesHit.Contains(other.gameObject.GetInstanceID()))//GetComponentInParent<PlayerCharacter>().isFighting)
        {
            playerCharacter.enemiesHit.Add(other.gameObject.GetInstanceID());
            //TimeResume();
            Debug.Log("Preso");
            OnEnemyCollision?.Invoke(this, new EnemyCollisionArgs(other.gameObject.GetComponent<Enemy>(), this));
        }
    }
    private void ActivateRxPiece(object sender, EventArgs args)
    {
        if(weaponType == WeaponType.Ax)
        {
            
             GetComponent<BoxCollider>().enabled = true;
           //Debug.Log(GetComponent<BoxCollider>().enabled);
        }
    }
    private void DeactivateRxPiece(object sender, EventArgs args)
    {
        if(weaponType == WeaponType.Ax)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void ActivateSxPiece(object sender, EventArgs args)
    {
        if(weaponType == WeaponType.Punch)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    private void DeactivateSxPiece(object sender, EventArgs args)
    {
        if (weaponType == WeaponType.Punch)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
/*
    private IEnumerator TimeResume()
    {
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1f;
    }
*/
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