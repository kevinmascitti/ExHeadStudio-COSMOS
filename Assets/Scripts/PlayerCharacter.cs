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
    [SerializeField] float chipSpeed;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Image characterIcon;
    public Image[] icons;

    // Start is called before the first frame update
    public void Awake()
    {
        isPlayer = true;

        //if (sliderHP)
        //{
        //    sliderHP.maxValue = MAX_HP;
        //}
        
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

        def_HP = Mathf.Clamp(currentHP, 0, MAX_HP);
        UpdateHPUI();
    }

    public override void UpdateHP(float newHP)
    {
        base.UpdateHP(newHP);
        lerpTimer = 0f;
    }
    
    public void UpdateHPUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = currentHP / MAX_HP;

        if (healthFraction >= 0.75f)
        {
            //characterIcon.color = new Color(characterIcon.color.r, characterIcon.color.g, characterIcon.color.b, 1f); //Cambio trasparenza
            //characterIcon.color = Color.green; //Cambio colore
            icons[0].enabled = true;//Cambio sprite
            icons[1].enabled = false;

        }

        else if (healthFraction >= 0.50f && healthFraction < 0.75f)
        {
            //characterIcon.color = new Color(characterIcon.color.r, characterIcon.color.g, characterIcon.color.b, 0.8f);
            //characterIcon.color = Color.yellow;
            icons[0].enabled = false;//Cambio sprite
            icons[1].enabled = true;
        }
        else if (healthFraction >= 0.25f && healthFraction < 0.50f)
        {
            characterIcon.color.WithAlpha(healthFraction);
        }
        else if (healthFraction < 0.25f)
        {
            characterIcon.color.WithAlpha(healthFraction);
        }

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if(fillFront < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.yellow;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete*percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete); 
        }

        //if(sliderHP)
        //    sliderHP.value = HP;
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
        //animator.SetTrigger("Attack");
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