using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ActiveAbilities : MonoBehaviour
{
    [SerializeField] float cooldownTime;
    [SerializeField] Image frontAbilityImage;
    [SerializeField] TMPro.TextMeshProUGUI abilityText;

    private float abilityTimer;
    private float abilityFraction;
    private bool cooldown = false;

    private void Awake()
    {
        frontAbilityImage.fillAmount = 1;
    }
    public void Start()
    {
        abilityTimer = 0;
        Mathf.Clamp(abilityTimer, 0, cooldownTime);
    }

    public virtual void Update()
    {
        if(Input.GetKey(KeyCode.Q) && !cooldown)
        {
            Ability();
            cooldown = true;
            abilityTimer = 0;
            StartCoroutine("AbilityCooldown");
        }
        abilityTimer += Time.deltaTime;
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
