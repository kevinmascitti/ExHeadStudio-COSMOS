using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireArmAbility : LeftArm
{

    [SerializeField] float cooldownTime;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform bulletStartPosition;
    [SerializeField] GameObject bulletPrefab;

    private CinemachineImpulseSource cameraShake;

    private bool cooldown = false;

    public override void LeftArmAbility()
    {
        if(!cooldown)
        {
            base.LeftArmAbility();
            cooldown = true;
            ShootBullet();
            StartCoroutine("AbilityCooldown");
        }

    }

    public void ShootBullet()
    {
        Instantiate(bulletPrefab, bulletStartPosition.position, bulletStartPosition.rotation);
        bulletPrefab.GetComponent<Rigidbody>().velocity = bulletStartPosition.forward * bulletSpeed; //* Time.deltaTime; QUI UN PROBLEMA PER FARLO MUOVERE
        cameraShake.GenerateImpulse();
    }

    private IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
        //Debug.Log("Ho aspettato");
    }
}
