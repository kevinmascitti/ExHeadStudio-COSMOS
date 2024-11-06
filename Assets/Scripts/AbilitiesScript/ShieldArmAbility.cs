using System.Collections;
using System.Collections.Generic;
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
        shield.SetActive(true);
        
        if(!shield.GetComponent<ParticleSystem>().isPlaying)
        {
            shield.GetComponent<ParticleSystem>().startLifetime = 0f;
            shield.GetComponent<ParticleSystem>().Play();
        }
            
    }

    public override void StopAbility()
    {
       
        shield.SetActive(false);
    }

}
