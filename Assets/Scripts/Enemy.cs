using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LULEnemy : LULCharacter
{
    public Type type;
    public Stats stats;
    
    public EventHandler OnEnemySpawn;
    public EventHandler OnEnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        OnEnemySpawn?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        base.Die();
        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        Debug.Log("ENEMY DEAD");
        Destroy(gameObject);
    }
}
