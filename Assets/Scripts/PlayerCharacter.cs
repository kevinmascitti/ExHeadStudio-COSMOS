using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PartType
{
    Head,
    Body,
    RightArm,
    LeftArm,
    Legs
}

public class PlayerCharacter : Character
{
    public float MAX_HP = 100;
    public float def_HP = 100;
    public Slider sliderHP;

    public Dictionary<PartType, Piece> composition;
    
    public Animator animator;
    public float attackRange;
    [NonSerialized] public LayerMask enemyLayer;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [NonSerialized] public Scenario currentScenario;
    [NonSerialized] public Scenario defaultScenario;

    public static EventHandler OnPlayerDeath;
    public static EventHandler<ScenarioArgs> OnScenarioBegin;


    //Aggiunte per la Healthbar
    private float lerpTimer;
    private float chipSpeed;
    public Image frontHealthBar;
    public Image backHealthBar;


    // Start is called before the first frame update
    public void Awake()
    {
        isPlayer = true;

        if (sliderHP)
        {
            sliderHP.maxValue = MAX_HP;
        }
        
        UpdateHP(def_HP);
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.A))
        {
            Attack();
            nextAttackTime = Time.time + 1f;
        }

        def_HP = Mathf.Clamp(def_HP, 0, MAX_HP);
    }

    public override void UpdateHP(float newHP)
    {
        lerpTimer = 0f;
        base.UpdateHP(newHP);
        UpdateHPUI(currentHP);
    }
    
    public void UpdateHPUI(float HP)
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = HP / MAX_HP;

        if(fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if(fillFront < healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
        }

        //if(sliderHP)
        //    sliderHP.value = HP;
    }


    public void RestoreHealth(float healthAmount)
    {
        
    }

    public override void Die()
    {
        base.Die();
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        Debug.Log("DIED");
        Respawn();
    }

    public void Respawn()
    {
        UpdateHP(MAX_HP);
        currentScenario = defaultScenario;
        gameObject.transform.position = currentScenario.respawnPoint;
        Debug.Log("RESPAWNED");
    }
    
    private void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("Attack done!");
    }

}

public class ScenarioArgs : EventArgs
{
    public ScenarioArgs(Scenario a)
    {
        scenario = a;
    }
    public Scenario scenario;
}