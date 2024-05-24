using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FireArmAbility : LeftArm
{

    [SerializeField] float cooldownTime;
    [SerializeField] Transform bulletStartPosition;
    [SerializeField] GameObject bulletPrefab;
    //[SerializeField] Image frontAbilityImage;

    //private int maxAbilityValue = 100;
    //private float abilityValue;
    //private float abilityTimer;

    //private CinemachineImpulseSource cameraShake;

    private bool cooldown = false;

    //private void Start()
    //{
    //    //abilityValue = maxAbilityValue;
    //    //abilityTimer = cooldownTime;
    //    //Mathf.Clamp(abilityValue, 0, maxAbilityValue);
    //}

    //private void Update()
    //{
    //    //abilityTimer += Time.deltaTime;
    //    //abilityValue += Time.deltaTime;
    //    //UpdateAbiltyColumn();
    //}

    public override void LeftArmAbility()
    {
        if(!cooldown)//(abilityTimer >= cooldownTime)
        {
            base.LeftArmAbility();
            cooldown = true;
            //abilityValue = 0;
            //abilityTimer = 0;
            ShootBullet();
            StartCoroutine("AbilityCooldown");
        }

    }

    public void ShootBullet()
    {
        Debug.Log("Ho sparato");
        Instantiate(bulletPrefab, bulletStartPosition.position, bulletStartPosition.rotation);
        //cameraShake.GenerateImpulse();
    }

    private IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
        Debug.Log("Ho aspettato");
    }

    //public void UpdateAbiltyColumn()
    //{
    //    float abilityFraction = abilityValue/maxAbilityValue;
    //    frontAbilityImage.fillAmount = abilityFraction;

    //}
}
