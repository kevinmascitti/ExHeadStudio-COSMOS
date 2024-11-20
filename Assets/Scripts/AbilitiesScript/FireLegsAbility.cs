using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLegsAbility : ActiveAbilities
{
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;

    private PlayerMovement movementScript;

    public override void Start()
    {
        base.Start();
        movementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();

    }

    public override void Update()
    {
        if (isContinous)
        {
            if (Input.GetKey(KeyCode.E) && abilityTimer >= 0.1f && !cooldown)
            {
                Ability();
                abilityTimer -= Time.deltaTime;
            }
            else
            {
                StopAbility();
                if (abilityTimer < 0.1f && !cooldown)
                {
                    cooldown = true;
                    StartCoroutine("AbilityCooldown");
                }
                else if (abilityTimer < timeForContinous && cooldown)
                {
                    abilityTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.E) && !cooldown)
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

    public override void Ability()
    {
        StartCoroutine(Dash());

    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            movementScript.playerController.Move(movementScript.getMoveDir() * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
