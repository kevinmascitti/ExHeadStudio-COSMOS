using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTriggerEnemy : MonoBehaviour
{


    [SerializeField] public List<GameObject> vfxList;
    
    public void DeathVFX(){
        vfxList[0].GetComponent<ParticleSystem>().Play();
    }

    public void StunOpportunityVFX(){
        vfxList[1].GetComponent<ParticleSystem>().Play();
    }

    public void SlashVFX(){
        vfxList[2].GetComponent<ParticleSystem>().Play();
        vfxList[3].GetComponent<ParticleSystem>().Play();
        vfxList[4].GetComponent<ParticleSystem>().Play();
    }



}
