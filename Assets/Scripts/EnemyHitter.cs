using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitter : MonoBehaviour
{
    public int atk;

    public static EventHandler<EnemyCollisionArgs> OnEnemyCollision;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy damage!");
            OnEnemyCollision?.Invoke(this, new EnemyCollisionArgs(other.gameObject.GetComponent<Enemy>(), this));
        }
    }
    
}

public class EnemyCollisionArgs : EventArgs 
{
    public EnemyCollisionArgs(Enemy e, EnemyHitter h)
    {
        enemy = e;
        hitter = h;
    }

    public Enemy enemy;
    public EnemyHitter hitter;
}