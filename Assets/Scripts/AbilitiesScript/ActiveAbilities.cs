using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public abstract class ActiveAbilities : MonoBehaviour
{


    [SerializeField] protected float cooldownTime, timeForContinous = 5f;
    [SerializeField] Image frontAbilityImage;
    [SerializeField] TMPro.TextMeshProUGUI abilityText;
    [SerializeField] protected bool isContinous = false;
    [NonSerialized] public Animator playerAnimator;
    protected float abilityTimer;
    protected float abilityFraction;
    protected bool cooldown = false;

    private void Awake()
    {
        frontAbilityImage.fillAmount = 1;
        playerAnimator = GetComponentInParent<Animator>();
    }
    public virtual void Start()
    {
        if (isContinous)
        {
            abilityTimer = timeForContinous-0.04f;
        }
        else
        {
            abilityTimer = 1;
        }

    }

    public virtual void Update()
    {
        if (isContinous)
        {
            if (Input.GetKey(KeyCode.Q) && abilityTimer >= 0.1f && !cooldown)
            {
                Ability();
                abilityTimer -= Time.deltaTime;
                abilityTimer = Mathf.Clamp(abilityTimer, 0, timeForContinous);
            }
            else
            {
                SetFalseObj();
                StopAbility();

                abilityTimer += Time.deltaTime;
                abilityTimer = Mathf.Clamp(abilityTimer, 0, timeForContinous);

                if (!cooldown && abilityTimer < 0.2f)
                {
                    cooldown = true;
                    StartCoroutine("AbilityCooldown");
                }
                /*else if (abilityTimer < timeForContinous && cooldown)
                {
                    abilityTimer += Time.deltaTime;
                }*/
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q) && !cooldown)
            {
                Ability();
                cooldown = true;
                abilityTimer = 0;
                StartCoroutine("AbilityCooldown");
            }
            abilityTimer += Time.deltaTime;
        }
        UpdateAbiltyColumn();
    }

    public abstract void Ability();



    private IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }
    public void UpdateAbiltyColumn()
    {
        if(isContinous)
        {
            if(cooldown) frontAbilityImage.color = Color.gray;
            else frontAbilityImage.color = Color.red;
            abilityFraction = abilityTimer / timeForContinous;
                frontAbilityImage.fillAmount = abilityFraction;
        }
        else
        {
            if (abilityTimer < cooldownTime)
            {
                abilityFraction = abilityTimer / cooldownTime;
                abilityText.color = new Color(abilityText.color.r, abilityText.color.g, abilityText.color.b, 0.5f);
            }
            else
            {
                abilityText.color = new Color(abilityText.color.r, abilityText.color.g, abilityText.color.b, 1);
                abilityFraction = 1;
            }

            frontAbilityImage.fillAmount = abilityFraction;
        }


    }

    public virtual void StopAbility() { } //la funzione serve a gestire l'abilità dello scudo
    public virtual void SetFalseObj() { } //la funzione serve per disattivare l'oggetto del braccio d'acqua
 
}
