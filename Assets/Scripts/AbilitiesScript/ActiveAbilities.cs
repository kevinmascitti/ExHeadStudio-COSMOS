using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public abstract class ActiveAbilities : MonoBehaviour
{
    [Tooltip("Per le abilità continue, meglio considerare un cooldown più lungo della durata stessa dell'abilità. C'è un bug.")]
    [SerializeField] float cooldownTime, timeForContinous = 5f;
    [SerializeField] Image frontAbilityImage;
    [SerializeField] TMPro.TextMeshProUGUI abilityText;
    [SerializeField] private bool isContinous = false;
    private float abilityTimer;
    private float abilityFraction;
    private bool cooldown = false;

    private void Awake()
    {
        frontAbilityImage.fillAmount = 1;
    }
    public void Start()
    {
        if (isContinous)
        {
            abilityTimer = timeForContinous;
        }
        else
        {
            abilityTimer = 1;
        }
        Mathf.Clamp(abilityTimer, 0, cooldownTime);
    }

    public virtual void Update()
    {
        if(isContinous)
        {
            if (Input.GetKey(KeyCode.Q) && abilityTimer >= 0.1f && !cooldown)
            {
                Ability();
                abilityTimer -= Time.deltaTime;
               // Debug.Log("Ability Timer" + abilityTimer);
            }
            else
            {
                if(abilityTimer < 0.1f && !cooldown)
                {
                    cooldown = true;
                    //Debug.Log("Inizio cooldown");
                    StartCoroutine("AbilityCooldown"); 
                }
                else if(abilityTimer < timeForContinous && cooldown)
                {
                    abilityTimer += Time.deltaTime;
                }
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
        
        if (isContinous)
        {
            yield return new WaitForSeconds(cooldownTime);
            //Debug.Log("FineCooldown");
            cooldown = false;
        }
        else
        {
            yield return new WaitForSeconds(cooldownTime);
            cooldown = false;
        }
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

 
}
