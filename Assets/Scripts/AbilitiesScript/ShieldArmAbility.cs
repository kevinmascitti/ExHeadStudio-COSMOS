using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldArmAbility : ActiveAbilities
{
    [SerializeField] private GameObject shield;
    private ShieldObj shieldObj;


    private void Awake()
    {
        shield.SetActive(false);
    }
    public override void Ability()
    {
        if (shield.activeSelf) {
            return;
        }
        shield.SetActive(true);
        PlayShield();


        if (!shield.GetComponent<ParticleSystem>().isPlaying)
        {
            shield.GetComponent<ParticleSystem>().startLifetime = 0f;
            shield.GetComponent<ParticleSystem>().Play();
        }

    }

    public override void StopAbility()
    {

        shield.SetActive(false);
        soundShield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private FMOD.Studio.EventInstance soundShield;

    private void PlayShield()
    {
        soundShield = RuntimeManager.CreateInstance("event:/Shield");
        soundShield.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        soundShield.start();
        soundShield.release();
    }
}