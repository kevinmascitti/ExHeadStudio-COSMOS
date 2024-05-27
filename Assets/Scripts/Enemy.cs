using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

public class Enemy : Character
{
    public Element enemyElement;
    public Type type;
    private Animator animator;
    public EventHandler OnEnemySpawn;
    public EventHandler OnEnemyDeath;

    [SerializeField] float defHP = 100;
    [SerializeField] float MAX_HP = 100;

    //healthbar nemcio
    [SerializeField] private Image frontSprite;
    [SerializeField] private Image backSprite;
    [SerializeField] float chipSpeed;
    private float lerpTimer;



    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        UpdateHP(defHP);
        //currentHP = defHP;
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

        defHP = Mathf.Clamp(currentHP, 0, MAX_HP);
        UpdateHPUI();
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


    public override void UpdateHP(float newHP)
    {
        base.UpdateHP(newHP);
        lerpTimer = 0f;
    }


    public void UpdateHPUI()
    {
        if (frontSprite && backSprite)
        {
            float fillFront = frontSprite.fillAmount;
            float fillBack = backSprite.fillAmount;
            float healthFraction = currentHP / MAX_HP;

            if (fillBack > healthFraction)
            {
                frontSprite.fillAmount = healthFraction;
                backSprite.color = Color.red;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                frontSprite.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
            }

            if (fillFront < healthFraction)
            {
                backSprite.fillAmount = healthFraction;
                backSprite.color = Color.yellow;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                frontSprite.fillAmount = Mathf.Lerp(fillFront, backSprite.fillAmount, percentComplete);
            }
        }
    }

}
