using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Rendering.FilterWindow;

public class Enemy : Character
{
    public Element enemyElement;
    public Type type;
    private Animator animator;
    public EventHandler OnEnemySpawn;
    public EventHandler OnEnemyDeath;


    
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        currentHP = 100;
        OnEnemySpawn?.Invoke(this, EventArgs.Empty);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        animator = GetComponent<Animator>();
        foreach (Element e in stats.elemAtk.Keys)
        {
            if (stats.elemAtk[e] > 0)
            {
                enemyElement = e;
                break;
            }
        }
        EnemyWeapon.OnPlayerCollision += DoDamage;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void DoDamage(object sender, PlayerCollisionArgs args)
    {

        args.player.TakeDamage(stats.elemAtk[enemyElement] - args.player.def, enemyElement);
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("ENEMY DEAD");
        animator.SetTrigger("Dead");
        
        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        StartCoroutine(DestroyAfterAnimationEnd("Death"));
        
    }

    private IEnumerator DestroyAfterAnimationEnd(string animationName)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    

}
