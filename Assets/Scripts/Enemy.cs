using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Character
{
    public Type type;
    
    public EventHandler OnEnemySpawn;
    public EventHandler OnEnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = 100;
        OnEnemySpawn?.Invoke(this, EventArgs.Empty);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        Debug.Log("ENEMY DEAD");
        Destroy(gameObject);
    }
}
