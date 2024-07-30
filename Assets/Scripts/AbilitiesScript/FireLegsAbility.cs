using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLegsAbility : ActiveAbilities
{
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private PlayerMovement movementScript;

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
