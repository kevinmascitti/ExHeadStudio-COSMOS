using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : Character
{
    public Element enemyElement;
    public Type type;
    private Animator animator;
    public EventHandler OnEnemySpawn;
    public EventHandler OnEnemyDeath;
    public static EventHandler<EnemyTr> OnEnemyDestroyed; //evento per eliminare i nemici dal target group

    [SerializeField] float defHP = 100;
    [SerializeField] float MAX_HP = 100;

    [Header("HelthBar nemico")]
    [SerializeField] private Image frontSprite;
    [SerializeField] private Image backSprite;
    [Tooltip("Velocit� con cui diminuisce la salute")]
    [SerializeField] float chipSpeed;
    private float lerpTimer;


    void Awake()
    {
        base.Awake();
        UpdateHP(defHP);
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
        EnemyWeapon.OnPlayerCollision += DoDamage;//QUESTO EVENTO NON ESISTE????
        
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
        if (this.GetInstanceID() == args.id)
        {                
            if (args.player.def > stats.elemAtk[enemyElement] + atk)
            {
                args.player.TakeDamage(0, enemyElement);
            }
            else args.player.TakeDamage(stats.elemAtk[enemyElement] + atk - args.player.def - args.player.stats.elemDef[enemyElement], enemyElement);
        }
    }

    public override void Die()
    {
        base.Die();
        animator.SetBool("isDead", true);
        StartCoroutine(DestroyAfterAnimationEnd("Nemico_Base_Morte"));
        
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
                backSprite.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
            }

            if (fillFront < healthFraction)
            {
                backSprite.fillAmount = healthFraction;
                backSprite.color = Color.red;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                frontSprite.fillAmount = Mathf.Lerp(fillFront, backSprite.fillAmount, percentComplete);
            }
        }
    }

    public void OnDestroy()
    {
        OnEnemyDestroyed?.Invoke(this, new EnemyTr(this.gameObject.transform));
    }

}

public class EnemyTr : EventArgs //l'evento serve a rimuovere i nemici dalla lista del lock
{
    public EnemyTr(Transform t)
    {
        tr = t;
    }
    public Transform tr;
}

